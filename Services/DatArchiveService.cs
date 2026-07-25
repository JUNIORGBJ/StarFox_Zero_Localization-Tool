using System.Text;
using System.Text.Json;

namespace StarFoxZeroLocalizationTool.Services;

public sealed class DatArchiveService
{
    private static readonly Dictionary<string, int> Alignments = new(StringComparer.OrdinalIgnoreCase)
    {
        ["dat"] = 0x2000,
        ["wmb"] = 0x1000,
        ["wtb"] = 0x1000,
        ["wtp"] = 0x1000,
        ["wta"] = 0x40,
        ["exp"] = 0x1000,
        ["sop"] = 0x40,
        ["eff"] = 0x1000,
        ["sdx"] = 0x1000,
        ["bxm"] = 0x40
    };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public DatArchiveManifest Extract(string inputDatPath, string outputDirectory)
    {
        if (!File.Exists(inputDatPath))
        {
            throw new FileNotFoundException("Arquivo .dat não encontrado.", inputDatPath);
        }

        if (string.IsNullOrWhiteSpace(outputDirectory))
        {
            throw new ArgumentException("Informe uma pasta de saída.");
        }

        Directory.CreateDirectory(outputDirectory);

        var data = File.ReadAllBytes(inputDatPath);
        if (!TryReadHeader(data, out var header, out var isBigEndian, out var error))
        {
            throw new InvalidDataException(error);
        }

        var entries = new List<DatArchiveEntry>();
        var fileOffsets = new uint[header.NumFiles];
        var fileExtensions = new string[header.NumFiles];
        var fileNames = new string[header.NumFiles];
        var fileSizes = new uint[header.NumFiles];

        for (var i = 0; i < header.NumFiles; i++)
        {
            var offset = (int)(header.OffsetFileOffsets + (i * 4));
            fileOffsets[i] = ReadUInt32(data, offset, isBigEndian);

            var extensionOffset = (int)(header.OffsetFileExtensions + (i * 4));
            fileExtensions[i] = ReadAsciiCString(data, extensionOffset, 4);

            var nameOffset = (int)(header.OffsetFileNames + 4 + (i * header.FileNameLength));
            fileNames[i] = ReadAsciiCString(data, nameOffset, (int)header.FileNameLength);

            var sizeOffset = (int)(header.OffsetFileSizes + (i * 4));
            fileSizes[i] = ReadUInt32(data, sizeOffset, isBigEndian);
        }

        for (var i = 0; i < header.NumFiles; i++)
        {
            var fileName = fileNames[i].Trim();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new InvalidDataException("Um dos arquivos no .dat não possui nome válido.");
            }

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new InvalidDataException($"O arquivo '{fileName}' não possui extensão válida.");
            }

            var targetPath = Path.Combine(outputDirectory, fileName.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar));
            var targetDir = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrWhiteSpace(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            var size = (int)fileSizes[i];
            if (size > 0 && fileOffsets[i] > 0)
            {
                var sourceOffset = (int)fileOffsets[i];
                var bytes = new byte[size];
                Buffer.BlockCopy(data, sourceOffset, bytes, 0, size);
                File.WriteAllBytes(targetPath, bytes);
            }
            else
            {
                File.WriteAllBytes(targetPath, Array.Empty<byte>());
            }

            entries.Add(new DatArchiveEntry
            {
                Name = fileName,
                Extension = extension.TrimStart('.'),
                Size = size,
                Offset = (int)fileOffsets[i]
            });
        }

        // Read Hash Map if present
        uint preHashShift = 0;
        List<uint> hashValues = new();
        if (header.OffsetHashMap > 0 && header.OffsetHashMap < data.Length)
        {
            var hmOffset = (int)header.OffsetHashMap;
            preHashShift = ReadUInt32(data, hmOffset, isBigEndian);
            var hmOffHashes = ReadUInt32(data, hmOffset + 8, isBigEndian);
            var hmOffIndices = ReadUInt32(data, hmOffset + 12, isBigEndian);

            var tempHashes = new uint[header.NumFiles];
            for (var i = 0; i < header.NumFiles; i++)
            {
                var h = ReadUInt32(data, (int)(hmOffset + hmOffHashes + (i * 4)), isBigEndian);
                var idx = ReadUInt16(data, (int)(hmOffset + hmOffIndices + (i * 2)), isBigEndian);
                if (idx < header.NumFiles)
                {
                    tempHashes[idx] = h;
                }
            }
            hashValues = tempHashes.ToList();
        }

        var metadataDirectory = Path.Combine(outputDirectory, ".metadata");
        Directory.CreateDirectory(metadataDirectory);
        var manifest = new DatArchiveManifest
        {
            IsBigEndian = isBigEndian,
            SourcePath = inputDatPath,
            Entries = entries,
            PreHashShift = preHashShift,
            Hashes = hashValues
        };

        File.WriteAllText(Path.Combine(metadataDirectory, "manifest.json"), JsonSerializer.Serialize(manifest, JsonOptions));
        return manifest;
    }

    public void Repack(string inputDirectory, string outputDatPath)
    {
        if (!Directory.Exists(inputDirectory))
        {
            throw new DirectoryNotFoundException("Pasta de origem não encontrada.");
        }

        var metadataPath = Path.Combine(inputDirectory, ".metadata", "manifest.json");
        DatArchiveManifest? manifest = null;
        if (File.Exists(metadataPath))
        {
            manifest = JsonSerializer.Deserialize<DatArchiveManifest>(File.ReadAllText(metadataPath));
        }

        var isBigEndian = manifest?.IsBigEndian ?? false;

        var files = Directory.EnumerateFiles(inputDirectory, "*", SearchOption.AllDirectories)
            .Where(path => !path.Replace(Path.DirectorySeparatorChar, '/').Contains("/.metadata/", StringComparison.OrdinalIgnoreCase))
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (files.Count == 0)
        {
            throw new InvalidOperationException("A pasta selecionada não contém arquivos para reempacotar.");
        }

        // Match the original Ruby tool: when metadata exists, only repack the files
        // declared in the extracted layout and preserve that exact order.
        List<string> orderedFilePaths;
        if (manifest != null)
        {
            if (manifest.Entries.Count == 0)
            {
                throw new InvalidDataException("O manifest do .dat não possui entradas para reempacotar.");
            }

            orderedFilePaths = new List<string>(manifest.Entries.Count);
            foreach (var entry in manifest.Entries)
            {
                var localPath = Path.Combine(inputDirectory, entry.Name.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar));
                if (!File.Exists(localPath))
                {
                    throw new FileNotFoundException(
                        $"Falta um arquivo do layout original do .dat: '{entry.Name}'.",
                        localPath);
                }

                orderedFilePaths.Add(localPath);
            }
        }
        else
        {
            orderedFilePaths = files;
        }

        var entries = new List<DatArchiveEntry>();
        foreach (var fullPath in orderedFilePaths)
        {
            var relativePath = Path.GetRelativePath(inputDirectory, fullPath)
                .Replace(Path.DirectorySeparatorChar, '/')
                .Replace('\\', '/');
            if (relativePath.StartsWith(".metadata/", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            byte[] bytes;
            if (File.Exists(fullPath))
            {
                bytes = File.ReadAllBytes(fullPath);
            }
            else
            {
                bytes = Array.Empty<byte>();
            }

            var extension = Path.GetExtension(relativePath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new InvalidOperationException($"O arquivo '{relativePath}' não possui extensão válida.");
            }

            entries.Add(new DatArchiveEntry
            {
                Name = relativePath,
                Extension = extension.TrimStart('.'),
                Size = bytes.Length,
                Data = bytes
            });
        }

        var directoryName = Path.GetDirectoryName(outputDatPath);
        if (!string.IsNullOrWhiteSpace(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        var fileNameLength = entries.Max(entry => Encoding.ASCII.GetByteCount(entry.Name) + 1);
        var totalEntries = entries.Count;
        var offsetFileOffsets = 0x20u;
        var offsetFileExtensions = offsetFileOffsets + (uint)(4 * totalEntries);
        var offsetFileNames = offsetFileExtensions + (uint)(4 * totalEntries);
        var offsetFileSizes = Align(offsetFileNames + 4 + (uint)(fileNameLength * totalEntries), 4);
        var filesOffset = offsetFileSizes + (uint)(4 * totalEntries);

        // HashMap configuration
        var hasHashMap = manifest != null && manifest.Hashes != null && manifest.Hashes.Count > 0;
        var preHashShift = manifest?.PreHashShift ?? 0;
        var hashValues = manifest?.Hashes ?? new List<uint>();

        var offsetHashMap = 0u;
        var numBuckets = 0;
        var bucketRanks = Array.Empty<short>();
        var orderedHashes = new List<uint>();
        var orderedIndices = new List<ushort>();

        if (hasHashMap)
        {
            offsetHashMap = filesOffset;
            numBuckets = 1 << (31 - (int)preHashShift);
            var buckets = new List<(uint hash, ushort index)>[numBuckets];
            for (var i = 0; i < numBuckets; i++)
            {
                buckets[i] = new List<(uint hash, ushort index)>();
            }

            for (var i = 0; i < totalEntries; i++)
            {
                var hash = i < hashValues.Count ? hashValues[i] : 0u;
                var bucketIndex = (int)(hash >> (int)preHashShift);
                if (bucketIndex >= 0 && bucketIndex < numBuckets)
                {
                    buckets[bucketIndex].Add((hash, (ushort)i));
                }
            }

            bucketRanks = new short[numBuckets];
            var currentRank = 0;
            for (var i = 0; i < numBuckets; i++)
            {
                if (buckets[i].Count > 0)
                {
                    bucketRanks[i] = (short)currentRank;
                    currentRank += buckets[i].Count;
                    foreach (var item in buckets[i])
                    {
                        orderedHashes.Add(item.hash);
                        orderedIndices.Add(item.index);
                    }
                }
                else
                {
                    bucketRanks[i] = -1;
                }
            }

            var hmTotalSize = (uint)(0x10 + (numBuckets * 2) + (totalEntries * 4) + (totalEntries * 2));
            filesOffset = offsetHashMap + hmTotalSize;
        }

        var fileOffsets = new uint[totalEntries];
        var currentOffset = filesOffset;
        for (var i = 0; i < totalEntries; i++)
        {
            var extension = entries[i].Extension;
            var alignment = GetAlignment(extension);
            currentOffset = Align(currentOffset, alignment);
            fileOffsets[i] = currentOffset;
            if (entries[i].Data != null && entries[i].Data!.Length > 0)
            {
                currentOffset += (uint)entries[i].Data!.Length;
                currentOffset = Align(currentOffset, alignment);
            }
            else
            {
                fileOffsets[i] = 0u;
            }
        }

        var totalSize = Align(currentOffset, 0x1000);
        var buffer = new byte[totalSize];

        WriteAscii(buffer, 0x00, "DAT\0", 4);
        WriteUInt32(buffer, 0x04, (uint)totalEntries, isBigEndian);
        WriteUInt32(buffer, 0x08, offsetFileOffsets, isBigEndian);
        WriteUInt32(buffer, 0x0C, offsetFileExtensions, isBigEndian);
        WriteUInt32(buffer, 0x10, offsetFileNames, isBigEndian);
        WriteUInt32(buffer, 0x14, offsetFileSizes, isBigEndian);
        WriteUInt32(buffer, 0x18, offsetHashMap, isBigEndian);

        for (var i = 0; i < totalEntries; i++)
        {
            WriteUInt32(buffer, (int)(offsetFileOffsets + (i * 4)), fileOffsets[i], isBigEndian);
            WriteAscii(buffer, (int)(offsetFileExtensions + (i * 4)), entries[i].Extension.PadRight(4, '\0').Substring(0, 4), 4);
        }

        WriteUInt32(buffer, (int)offsetFileNames, (uint)fileNameLength, isBigEndian);
        for (var i = 0; i < totalEntries; i++)
        {
            var nameBytes = Encoding.ASCII.GetBytes(entries[i].Name + "\0");
            var targetOffset = (int)(offsetFileNames + 4 + (i * fileNameLength));
            Array.Copy(nameBytes, 0, buffer, targetOffset, Math.Min(nameBytes.Length, fileNameLength));
        }

        for (var i = 0; i < totalEntries; i++)
        {
            var size = entries[i].Data != null ? (uint)entries[i].Data!.Length : 0u;
            WriteUInt32(buffer, (int)(offsetFileSizes + (i * 4)), size, isBigEndian);
        }

        // Write Hash Map
        if (hasHashMap)
        {
            var hmStart = (int)offsetHashMap;
            var hmOBR = 0x10u;
            var hmOH = (uint)(hmOBR + numBuckets * 2);
            var hmOFI = (uint)(hmOH + totalEntries * 4);

            WriteUInt32(buffer, hmStart, preHashShift, isBigEndian);
            WriteUInt32(buffer, hmStart + 4, hmOBR, isBigEndian);
            WriteUInt32(buffer, hmStart + 8, hmOH, isBigEndian);
            WriteUInt32(buffer, hmStart + 12, hmOFI, isBigEndian);

            for (var i = 0; i < numBuckets; i++)
            {
                WriteInt16(buffer, hmStart + (int)hmOBR + (i * 2), bucketRanks[i], isBigEndian);
            }

            for (var i = 0; i < orderedHashes.Count; i++)
            {
                WriteUInt32(buffer, hmStart + (int)hmOH + (i * 4), orderedHashes[i], isBigEndian);
            }

            for (var i = 0; i < orderedIndices.Count; i++)
            {
                WriteUInt16(buffer, hmStart + (int)hmOFI + (i * 2), orderedIndices[i], isBigEndian);
            }
        }

        for (var i = 0; i < totalEntries; i++)
        {
            if (entries[i].Data == null || entries[i].Data!.Length == 0)
            {
                continue;
            }

            var dataOffset = (int)fileOffsets[i];
            Array.Copy(entries[i].Data!, 0, buffer, dataOffset, entries[i].Data!.Length);
        }

        File.WriteAllBytes(outputDatPath, buffer);
    }

    private static bool TryReadHeader(byte[] data, out DatHeader header, out bool isBigEndian, out string error)
    {
        header = new DatHeader();
        isBigEndian = false;
        error = string.Empty;

        if (data.Length < 0x20)
        {
            error = "O arquivo é muito pequeno para ser um .dat válido.";
            return false;
        }

        var magic = Encoding.ASCII.GetString(data, 0, 4);
        if (!magic.StartsWith("DAT", StringComparison.Ordinal))
        {
            error = "O arquivo não possui o identificador DAT no cabeçalho.";
            return false;
        }

        // Test little endian
        var leHeader = ReadHeader(data, false);
        var leValid = leHeader.OffsetFileOffsets < data.Length &&
                      leHeader.OffsetFileExtensions < data.Length &&
                      leHeader.OffsetFileNames < data.Length &&
                      leHeader.OffsetFileSizes < data.Length;

        // Test big endian
        var beHeader = ReadHeader(data, true);
        var beValid = beHeader.OffsetFileOffsets < data.Length &&
                      beHeader.OffsetFileExtensions < data.Length &&
                      beHeader.OffsetFileNames < data.Length &&
                      beHeader.OffsetFileSizes < data.Length;

        if (leValid && !beValid)
        {
            isBigEndian = false;
            header = leHeader;
            return true;
        }
        if (beValid && !leValid)
        {
            isBigEndian = true;
            header = beHeader;
            return true;
        }
        if (leValid && beValid)
        {
            isBigEndian = false;
            header = leHeader;
            return true;
        }

        error = "Não foi possível determinar a endianness correta ou os offsets no cabeçalho são inválidos.";
        return false;
    }

    private static DatHeader ReadHeader(byte[] data, bool isBigEndian)
    {
        var numFiles = (int)ReadUInt32(data, 0x04, isBigEndian);
        var offsetFileNames = ReadUInt32(data, 0x10, isBigEndian);
        var fileNameLength = 0u;
        if (numFiles > 0 && offsetFileNames < data.Length)
        {
            fileNameLength = ReadUInt32(data, (int)offsetFileNames, isBigEndian);
        }

        return new DatHeader
        {
            NumFiles = numFiles,
            OffsetFileOffsets = ReadUInt32(data, 0x08, isBigEndian),
            OffsetFileExtensions = ReadUInt32(data, 0x0C, isBigEndian),
            OffsetFileNames = offsetFileNames,
            OffsetFileSizes = ReadUInt32(data, 0x14, isBigEndian),
            OffsetHashMap = ReadUInt32(data, 0x18, isBigEndian),
            FileNameLength = fileNameLength
        };
    }

    private static uint ReadUInt32(byte[] data, int offset, bool isBigEndian)
    {
        if (isBigEndian)
        {
            return (uint)((data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3]);
        }

        return BitConverter.ToUInt32(data, offset);
    }

    private static ushort ReadUInt16(byte[] data, int offset, bool isBigEndian)
    {
        if (isBigEndian)
        {
            return (ushort)((data[offset] << 8) | data[offset + 1]);
        }
        return BitConverter.ToUInt16(data, offset);
    }

    private static string ReadAsciiCString(byte[] data, int offset, int length)
    {
        var bytes = new byte[length];
        Array.Copy(data, offset, bytes, 0, length);
        var end = Array.IndexOf(bytes, (byte)0x00);
        if (end < 0)
        {
            end = bytes.Length;
        }

        return Encoding.ASCII.GetString(bytes, 0, end).Trim();
    }

    private static void WriteAscii(byte[] buffer, int offset, string value, int length)
    {
        var bytes = Encoding.ASCII.GetBytes(value);
        Array.Copy(bytes, 0, buffer, offset, Math.Min(bytes.Length, length));
    }

    private static void WriteUInt32(byte[] buffer, int offset, uint value, bool isBigEndian)
    {
        if (isBigEndian)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
            return;
        }

        var little = BitConverter.GetBytes(value);
        Array.Copy(little, 0, buffer, offset, little.Length);
    }

    private static void WriteUInt16(byte[] buffer, int offset, ushort value, bool isBigEndian)
    {
        if (isBigEndian)
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)value;
            return;
        }
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, buffer, offset, bytes.Length);
    }

    private static void WriteInt16(byte[] buffer, int offset, short value, bool isBigEndian)
    {
        WriteUInt16(buffer, offset, (ushort)value, isBigEndian);
    }

    private static int Align(int value, int alignment)
    {
        return alignment == 0 ? value : ((value + alignment - 1) / alignment) * alignment;
    }

    private static uint Align(uint value, int alignment)
    {
        return (uint)Align((int)value, alignment);
    }

    private static int GetAlignment(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            return 0x10;
        }

        var key = extension.TrimStart('.').ToLowerInvariant();
        return Alignments.TryGetValue(key, out var alignment) ? alignment : 0x10;
    }
}

public sealed class DatArchiveManifest
{
    public bool IsBigEndian { get; set; }
    public string SourcePath { get; set; } = string.Empty;
    public List<DatArchiveEntry> Entries { get; set; } = new();
    public uint PreHashShift { get; set; }
    public List<uint> Hashes { get; set; } = new();
}

public sealed class DatArchiveEntry
{
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int Size { get; set; }
    public int Offset { get; set; }
    public byte[]? Data { get; set; }
}

public sealed class DatHeader
{
    public int NumFiles { get; set; }
    public uint OffsetFileOffsets { get; set; }
    public uint OffsetFileExtensions { get; set; }
    public uint OffsetFileNames { get; set; }
    public uint OffsetFileSizes { get; set; }
    public uint OffsetHashMap { get; set; }
    public uint FileNameLength { get; set; }
}
