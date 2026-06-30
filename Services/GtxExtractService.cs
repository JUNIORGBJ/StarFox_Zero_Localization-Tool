using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StarFoxZeroLocalizationTool.Services
{
    internal enum GtxColorProfile
    {
        AutoFromOriginal,
        Unorm,
        Srgb
    }

    internal sealed record GtxAnalysisResult(
        bool Success,
        string Format,
        int? TileMode,
        string Swizzle,
        int? FullSwizzleValue,
        int? InitialSwizzleValue,
        string ComponentSelector,
        bool IsSrgb,
        bool HasLosslessRoundtripRisk,
        string AdvisoryMessage,
        string ToolOutput);

    internal sealed record GtxCommandResult(
        bool Success,
        int ExitCode,
        string Output,
        string ExecutablePath,
        string Arguments);

    internal static class GtxExtractService
    {
        private static readonly string[] KnownExecutablePaths =
        {
            "gtx_extract.exe",
            Path.Combine("ui_msg_corneria_es", "title_wta", "gtx_extract_x86_v5.3", "gtx_extract.exe"),
            Path.Combine("ui_msg_corneria_es", "messmsg_corneria_wta", "gtx_extract_x86_v5.3", "gtx_extract.exe")
        };

        private static readonly Regex FormatRegex = new(@"format\s*=\s*(.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex TileModeRegex = new(@"tileMode\s*=\s*(\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SwizzleRegex = new(@"swizzle\s*=\s*(.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SwizzleHexRegex = new(@"0x([0-9a-fA-F]+)", RegexOptions.Compiled);
        private static readonly Regex ComponentSelectorRegex =
            new(@"GX2 Component Selector:\s+Red Channel:\s*(.+)\s+Green Channel:\s*(.+)\s+Blue Channel:\s*(.+)\s+Alpha Channel:\s*(.+)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static string? TryFindExecutable()
        {
            var startingDirectories = new[]
            {
                AppContext.BaseDirectory,
                Environment.CurrentDirectory
            };

            foreach (var startingDirectory in startingDirectories.Where(Directory.Exists))
            {
                foreach (var directory in EnumerateParentDirectories(startingDirectory))
                {
                    foreach (var relativePath in KnownExecutablePaths)
                    {
                        var candidate = Path.Combine(directory, relativePath);
                        if (File.Exists(candidate))
                        {
                            return candidate;
                        }
                    }
                }
            }

            return null;
        }

        public static GtxAnalysisResult AnalyzeGtx(string executablePath, string gtxPath)
        {
            if (NativeR8G8GtxCodec.TryAnalyze(gtxPath, out var nativeResult))
            {
                return nativeResult;
            }

            ValidateToolAndInput(executablePath, gtxPath, ".gtx");

            var tempDirectory = Path.Combine(Path.GetTempPath(), "StarFoxZeroLocalizationTool", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);
            var temporaryDds = Path.Combine(tempDirectory, "analysis.dds");

            try
            {
                var result = Execute(executablePath, $"-o {Quote(temporaryDds)} {Quote(gtxPath)}");
                if (!result.Success)
                {
                    return new GtxAnalysisResult(
                        false,
                        string.Empty,
                        null,
                        string.Empty,
                        null,
                        null,
                        string.Empty,
                        false,
                        false,
                        "Nao foi possivel analisar o arquivo GTX.",
                        result.Output);
                }

                var format = MatchValue(FormatRegex, result.Output);
                var swizzle = MatchValue(SwizzleRegex, result.Output);
                var componentSelector = BuildComponentSelector(result.Output);
                var fullSwizzleValue = ParseSwizzleHexValue(swizzle);
                int? initialSwizzleValue = fullSwizzleValue.HasValue
                    ? (fullSwizzleValue.Value >> 8) & 0xFF
                    : null;
                var tileMode = int.TryParse(MatchValue(TileModeRegex, result.Output), out var parsedTileMode)
                    ? parsedTileMode
                    : (int?)null;
                var isSrgb = format.Contains("SRGB", StringComparison.OrdinalIgnoreCase);
                var hasLosslessRisk = format.Contains("R8_G8_UNORM", StringComparison.OrdinalIgnoreCase);

                var advisory = BuildAdvisoryMessage(format, isSrgb, hasLosslessRisk);
                return new GtxAnalysisResult(
                    true,
                    format,
                    tileMode,
                    swizzle,
                    fullSwizzleValue,
                    initialSwizzleValue,
                    componentSelector,
                    isSrgb,
                    hasLosslessRisk,
                    advisory,
                    result.Output);
            }
            finally
            {
                TryDeleteDirectory(tempDirectory);
            }
        }

        public static GtxCommandResult ConvertGtxToDds(string executablePath, string inputGtxPath, string outputDdsPath)
        {
            if (NativeR8G8GtxCodec.TryConvertGtxToDds(inputGtxPath, outputDdsPath, out var nativeResult))
            {
                return nativeResult;
            }

            ValidateToolAndInput(executablePath, inputGtxPath, ".gtx");
            EnsureOutputDirectory(outputDdsPath);
            return Execute(executablePath, $"-o {Quote(outputDdsPath)} {Quote(inputGtxPath)}");
        }

        public static GtxCommandResult ConvertDdsToGtx(
            string executablePath,
            string inputDdsPath,
            string outputGtxPath,
            GtxColorProfile profile,
            string? originalGtxPath)
        {
            ValidateToolAndInput(executablePath, inputDdsPath, ".dds");
            EnsureOutputDirectory(outputGtxPath);

            GtxAnalysisResult? originalAnalysis = null;
            var useSrgb = profile switch
            {
                GtxColorProfile.Srgb => true,
                GtxColorProfile.Unorm => false,
                _ => ResolveAutoProfile(executablePath, originalGtxPath, out originalAnalysis)
            };

            if (originalAnalysis == null && !string.IsNullOrWhiteSpace(originalGtxPath) && File.Exists(originalGtxPath))
            {
                originalAnalysis = AnalyzeGtx(executablePath, originalGtxPath);
            }

            var argumentBuilder = new StringBuilder();
            if (originalAnalysis is { Success: true })
            {
                if (originalAnalysis.TileMode.HasValue)
                {
                    argumentBuilder.Append($"-tileMode {originalAnalysis.TileMode.Value} ");
                }

                if (originalAnalysis.InitialSwizzleValue.HasValue)
                {
                    argumentBuilder.Append($"-swizzle {originalAnalysis.InitialSwizzleValue.Value} ");
                }
            }

            if (useSrgb)
            {
                argumentBuilder.Append("-SRGB 1 ");
            }

            argumentBuilder.Append($"-o {Quote(outputGtxPath)} {Quote(inputDdsPath)}");
            return Execute(executablePath, argumentBuilder.ToString());
        }

        public static GtxCommandResult ConvertDdsToGtxPreservingOriginalContainer(
            string executablePath,
            string inputDdsPath,
            string outputGtxPath,
            GtxColorProfile profile,
            string originalGtxPath)
        {
            if (NativeR8G8GtxCodec.TryConvertDdsToGtxPreservingOriginalContainer(inputDdsPath, outputGtxPath, originalGtxPath, out var nativeResult))
            {
                return nativeResult;
            }

            ValidateToolAndInput(executablePath, inputDdsPath, ".dds");
            ValidateToolAndInput(executablePath, originalGtxPath, ".gtx");
            EnsureOutputDirectory(outputGtxPath);

            var tempDirectory = Path.Combine(Path.GetTempPath(), "StarFoxZeroLocalizationTool", "GtxSave", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);
            var rebuiltGtxPath = Path.Combine(tempDirectory, "rebuilt.gtx");

            try
            {
                var convertResult = ConvertDdsToGtx(executablePath, inputDdsPath, rebuiltGtxPath, profile, originalGtxPath);
                if (!convertResult.Success)
                {
                    return convertResult;
                }

                PreserveOriginalContainer(originalGtxPath, rebuiltGtxPath, outputGtxPath);
                var output = string.Join(
                    Environment.NewLine,
                    new[]
                    {
                        convertResult.Output,
                        "Original GTX container preserved by transplanting texture data blocks."
                    }.Where(value => !string.IsNullOrWhiteSpace(value)));

                return new GtxCommandResult(
                    true,
                    convertResult.ExitCode,
                    output,
                    convertResult.ExecutablePath,
                    convertResult.Arguments);
            }
            finally
            {
                TryDeleteDirectory(tempDirectory);
            }
        }

        private static bool ResolveAutoProfile(string executablePath, string? originalGtxPath, out GtxAnalysisResult? analysis)
        {
            analysis = null;
            if (string.IsNullOrWhiteSpace(originalGtxPath))
            {
                return false;
            }

            analysis = AnalyzeGtx(executablePath, originalGtxPath);
            return analysis.Success && analysis.IsSrgb;
        }

        private static void PreserveOriginalContainer(string originalGtxPath, string rebuiltGtxPath, string outputGtxPath)
        {
            var originalBytes = File.ReadAllBytes(originalGtxPath);
            var rebuiltBytes = File.ReadAllBytes(rebuiltGtxPath);

            var originalBlocks = ParseGtxBlocks(originalBytes);
            var rebuiltBlocks = ParseGtxBlocks(rebuiltBytes);

            var originalDataBlocks = originalBlocks.Where(static block => block.Type is 0x0C or 0x0D).ToArray();
            var rebuiltDataBlocks = rebuiltBlocks.Where(static block => block.Type is 0x0C or 0x0D).ToArray();

            if (originalDataBlocks.Length == 0 || rebuiltDataBlocks.Length == 0)
            {
                throw new InvalidOperationException("Nao foi possivel localizar os blocos de dados da textura no GTX.");
            }

            if (originalDataBlocks.Length != rebuiltDataBlocks.Length)
            {
                throw new InvalidOperationException("A estrutura de blocos do GTX gerado nao corresponde ao GTX original.");
            }

            var outputBytes = new byte[originalBytes.Length];
            Buffer.BlockCopy(originalBytes, 0, outputBytes, 0, originalBytes.Length);

            for (var index = 0; index < originalDataBlocks.Length; index++)
            {
                var originalBlock = originalDataBlocks[index];
                var rebuiltBlock = rebuiltDataBlocks[index];

                if (originalBlock.Type != rebuiltBlock.Type)
                {
                    throw new InvalidOperationException("Os tipos de blocos do GTX gerado nao correspondem ao GTX original.");
                }

                if (originalBlock.Size != rebuiltBlock.Size)
                {
                    throw new InvalidOperationException("O tamanho do bloco de textura gerado nao corresponde ao GTX original.");
                }

                Buffer.BlockCopy(
                    rebuiltBytes,
                    rebuiltBlock.DataOffset,
                    outputBytes,
                    originalBlock.DataOffset,
                    originalBlock.Size);
            }

            File.WriteAllBytes(outputGtxPath, outputBytes);
        }

        private static GtxCommandResult Execute(string executablePath, string arguments)
        {
            var invocation = BuildInvocation(executablePath, arguments);
            var processStartInfo = new ProcessStartInfo
            {
                FileName = invocation.FileName,
                Arguments = invocation.Arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = invocation.WorkingDirectory,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            var combinedOutput = string.Join(
                Environment.NewLine,
                new[] { output, error }.Where(value => !string.IsNullOrWhiteSpace(value)));

            return new GtxCommandResult(
                process.ExitCode == 0,
                process.ExitCode,
                combinedOutput,
                invocation.DisplayPath,
                invocation.DisplayArguments);
        }

        private static IEnumerable<string> EnumerateParentDirectories(string startingDirectory)
        {
            var current = new DirectoryInfo(startingDirectory);
            while (current != null)
            {
                yield return current.FullName;
                current = current.Parent;
            }
        }

        private static void ValidateToolAndInput(string executablePath, string inputPath, string expectedExtension)
        {
            if (!IsValidToolPath(executablePath))
            {
                throw new FileNotFoundException("Nao foi possivel localizar o backend do gtx_extract (.exe ou .py).", executablePath);
            }

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("O arquivo informado nao foi encontrado.", inputPath);
            }

            if (!string.Equals(Path.GetExtension(inputPath), expectedExtension, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"O arquivo informado precisa ter extensao {expectedExtension}.");
            }
        }

        private static string MatchValue(Regex regex, string input)
        {
            var match = regex.Match(input);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        private static string BuildComponentSelector(string output)
        {
            var match = ComponentSelectorRegex.Match(output);
            if (!match.Success)
            {
                return string.Empty;
            }

            return $"{match.Groups[1].Value.Trim()}, {match.Groups[2].Value.Trim()}, {match.Groups[3].Value.Trim()}, {match.Groups[4].Value.Trim()}";
        }

        private static int? ParseSwizzleHexValue(string swizzleText)
        {
            if (string.IsNullOrWhiteSpace(swizzleText))
            {
                return null;
            }

            var match = SwizzleHexRegex.Match(swizzleText);
            if (!match.Success)
            {
                return null;
            }

            return int.TryParse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber, null, out var value)
                ? value
                : null;
        }

        private static string BuildAdvisoryMessage(string format, bool isSrgb, bool hasLosslessRisk)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return "Nao foi possivel identificar o formato pelo gtx_extract.";
            }

            if (hasLosslessRisk)
            {
                return "Atencao: o formato R8_G8_UNORM usado em fonte/UI exige preservar tileMode e swizzle do GTX base na recriacao.";
            }

            if (isSrgb)
            {
                return "O arquivo original usa SRGB. Para recriar um GTX equivalente, use o perfil SRGB.";
            }

            return "O arquivo original usa UNORM. Em geral, o perfil UNORM e o mais adequado para a recriacao.";
        }

        private static void EnsureOutputDirectory(string outputPath)
        {
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static string Quote(string value)
        {
            return $"\"{value}\"";
        }

        public static bool IsValidToolPath(string? toolPath)
        {
            if (string.IsNullOrWhiteSpace(toolPath) || !File.Exists(toolPath))
            {
                return false;
            }

            var extension = Path.GetExtension(toolPath);
            return string.Equals(extension, ".exe", StringComparison.OrdinalIgnoreCase)
                || string.Equals(extension, ".py", StringComparison.OrdinalIgnoreCase);
        }

        private static ToolInvocation BuildInvocation(string toolPath, string arguments)
        {
            var extension = Path.GetExtension(toolPath);
            if (string.Equals(extension, ".py", StringComparison.OrdinalIgnoreCase))
            {
                var pythonExecutable = FindPythonExecutable();
                var scriptDirectory = Path.GetDirectoryName(toolPath) ?? AppContext.BaseDirectory;
                var launcherArguments = IsPythonLauncher(pythonExecutable)
                    ? $"-3 {Quote(toolPath)} {arguments}"
                    : $"{Quote(toolPath)} {arguments}";
                return new ToolInvocation(
                    pythonExecutable,
                    launcherArguments,
                    scriptDirectory,
                    toolPath,
                    arguments);
            }

            return new ToolInvocation(
                toolPath,
                arguments,
                Path.GetDirectoryName(toolPath) ?? AppContext.BaseDirectory,
                toolPath,
                arguments);
        }

        private static string FindPythonExecutable()
        {
            foreach (var candidate in EnumeratePythonCandidates())
            {
                if (CanRunPython(candidate))
                {
                    return candidate;
                }
            }

            throw new FileNotFoundException(
                "Nao foi possivel localizar um interpretador Python para executar o gtx_extract.py embutido. Instale o Python 3 ou informe um gtx_extract.exe.",
                "python");
        }

        private static IEnumerable<string> EnumeratePythonCandidates()
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var candidate in new[]
                     {
                         "py",
                         "python",
                         "python3"
                     })
            {
                if (seen.Add(candidate))
                {
                    yield return candidate;
                }
            }

            var windowsPy = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "py.exe");
            if (seen.Add(windowsPy))
            {
                yield return windowsPy;
            }

            var path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (var directory in path.Split(Path.PathSeparator).Where(static value => !string.IsNullOrWhiteSpace(value)))
            {
                foreach (var fileName in new[] { "py.exe", "python.exe", "python3.exe" })
                {
                    var fullPath = Path.Combine(directory.Trim(), fileName);
                    if (seen.Add(fullPath))
                    {
                        yield return fullPath;
                    }
                }
            }
        }

        private static bool IsRunnableExecutable(string candidate)
        {
            if (candidate.IndexOf(Path.DirectorySeparatorChar) >= 0 || candidate.IndexOf(Path.AltDirectorySeparatorChar) >= 0)
            {
                return File.Exists(candidate);
            }

            return true;
        }

        private static bool IsPythonLauncher(string candidate)
        {
            return string.Equals(Path.GetFileName(candidate), "py", StringComparison.OrdinalIgnoreCase)
                || string.Equals(Path.GetFileName(candidate), "py.exe", StringComparison.OrdinalIgnoreCase);
        }

        private static bool CanRunPython(string candidate)
        {
            if (!IsRunnableExecutable(candidate))
            {
                return false;
            }

            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = candidate,
                        Arguments = IsPythonLauncher(candidate) ? "-3 --version" : "--version",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        WorkingDirectory = AppContext.BaseDirectory
                    }
                };

                process.Start();
                if (!process.WaitForExit(3000))
                {
                    try
                    {
                        process.Kill(entireProcessTree: true);
                    }
                    catch
                    {
                        // Ignore cleanup failure.
                    }

                    return false;
                }

                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private static GtxBlockInfo[] ParseGtxBlocks(byte[] bytes)
        {
            if (bytes.Length < 0x20 || bytes[0] != (byte)'G' || bytes[1] != (byte)'f' || bytes[2] != (byte)'x' || bytes[3] != (byte)'2')
            {
                throw new InvalidOperationException("O arquivo GTX informado possui cabecalho invalido.");
            }

            var blocks = new List<GtxBlockInfo>();
            var offset = 0x20;
            while (offset + 0x20 <= bytes.Length)
            {
                if (!(bytes[offset] == (byte)'B' && bytes[offset + 1] == (byte)'L' && bytes[offset + 2] == (byte)'K' && bytes[offset + 3] == (byte)'{'))
                {
                    break;
                }

                var blockType = ReadUInt32BigEndian(bytes, offset + 0x10);
                var blockSize = checked((int)ReadUInt32BigEndian(bytes, offset + 0x14));
                var dataOffset = offset + 0x20;
                if (blockSize < 0 || dataOffset + blockSize > bytes.Length)
                {
                    throw new InvalidOperationException("O arquivo GTX contem um bloco invalido ou truncado.");
                }

                blocks.Add(new GtxBlockInfo(blockType, offset, dataOffset, blockSize));

                offset = checked(dataOffset + blockSize);
                if (blockType == 0x01)
                {
                    break;
                }
            }

            return blocks.ToArray();
        }

        private static uint ReadUInt32BigEndian(byte[] bytes, int offset)
        {
            return ((uint)bytes[offset] << 24)
                 | ((uint)bytes[offset + 1] << 16)
                 | ((uint)bytes[offset + 2] << 8)
                 | bytes[offset + 3];
        }

        private static void TryDeleteDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            try
            {
                Directory.Delete(directoryPath, recursive: true);
            }
            catch
            {
                // Ignore temporary cleanup failures.
            }
        }

        private readonly record struct ToolInvocation(
            string FileName,
            string Arguments,
            string WorkingDirectory,
            string DisplayPath,
            string DisplayArguments);

        private readonly record struct GtxBlockInfo(uint Type, int HeaderOffset, int DataOffset, int Size);
    }
}
