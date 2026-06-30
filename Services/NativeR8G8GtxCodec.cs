using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StarFoxZeroLocalizationTool.Services
{
    internal static class NativeR8G8GtxCodec
    {
        private const uint SupportedFormat = 0x07;
        private const string SupportedFormatName = "GX2_SURFACE_FORMAT_TC_R8_G8_UNORM";
        private const int BitsPerPixel = 16;
        private const int BytesPerPixel = 2;
        private static readonly byte[] GtxMagic = { (byte)'G', (byte)'f', (byte)'x', (byte)'2' };
        private static readonly byte[] BlockMagic = { (byte)'B', (byte)'L', (byte)'K', (byte)'{' };
        private static readonly int[] BankSwapOrder = { 0, 1, 3, 2, 6, 7, 5, 4, 0, 0 };

        public static bool TryAnalyze(string gtxPath, out GtxAnalysisResult result)
        {
            result = default!;
            if (!TryReadMetadata(gtxPath, out var metadata, out _))
            {
                return false;
            }

            if (!IsSupported(metadata, out _))
            {
                return false;
            }

            var toolOutput = BuildToolOutput(gtxPath, metadata);
            result = new GtxAnalysisResult(
                true,
                SupportedFormatName,
                metadata.TileMode,
                $"{metadata.Swizzle}, 0x{metadata.Swizzle:x}",
                metadata.Swizzle,
                (metadata.Swizzle >> 8) & 0xFF,
                BuildComponentSelectorText(metadata.ComponentSelector),
                false,
                true,
                "Atencao: o formato R8_G8_UNORM usado em fonte/UI exige preservar tileMode e swizzle do GTX base na recriacao.",
                toolOutput);
            return true;
        }

        public static bool TryConvertGtxToDds(string inputGtxPath, string outputDdsPath, out GtxCommandResult result)
        {
            result = default!;
            if (!TryReadMetadata(inputGtxPath, out var metadata, out var error))
            {
                return false;
            }

            if (!IsSupported(metadata, out error))
            {
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(outputDdsPath) ?? AppContext.BaseDirectory);

            var deswizzled = DeswizzleLevel0(metadata);
            var header = BuildA8L8DdsHeader(metadata.Width, metadata.Height);

            using (var stream = File.Create(outputDdsPath))
            {
                stream.Write(header, 0, header.Length);
                stream.Write(deswizzled, 0, deswizzled.Length);
            }

            result = new GtxCommandResult(
                true,
                0,
                BuildToolOutput(inputGtxPath, metadata) + Environment.NewLine + Environment.NewLine + $"Finished converting: {outputDdsPath}",
                "native-r8g8",
                $"internal-native -o \"{outputDdsPath}\" \"{inputGtxPath}\"");
            return true;
        }

        public static bool TryConvertDdsToGtxPreservingOriginalContainer(
            string inputDdsPath,
            string outputGtxPath,
            string originalGtxPath,
            out GtxCommandResult result)
        {
            result = default!;
            if (!TryReadMetadata(originalGtxPath, out var metadata, out var error))
            {
                return false;
            }

            if (!IsSupported(metadata, out error))
            {
                return false;
            }

            var dds = RawR8G8DdsImage.Load(inputDdsPath);
            if (dds.Width != metadata.Width || dds.Height != metadata.Height)
            {
                throw new InvalidOperationException(
                    $"O DDS precisa ter exatamente {metadata.Width}x{metadata.Height} para este GTX.");
            }

            var swizzled = SwizzleLevel0(metadata, dds.PixelData);
            var outputBytes = new byte[metadata.OriginalBytes.Length];
            Buffer.BlockCopy(metadata.OriginalBytes, 0, outputBytes, 0, outputBytes.Length);
            Buffer.BlockCopy(swizzled, 0, outputBytes, metadata.ImageDataOffset, swizzled.Length);

            Directory.CreateDirectory(Path.GetDirectoryName(outputGtxPath) ?? AppContext.BaseDirectory);
            File.WriteAllBytes(outputGtxPath, outputBytes);

            result = new GtxCommandResult(
                true,
                0,
                BuildToolOutput(originalGtxPath, metadata)
                + Environment.NewLine + Environment.NewLine
                + "Original GTX container preserved by native R8_G8 data transplant."
                + Environment.NewLine
                + $"Finished converting: {outputGtxPath}",
                "native-r8g8",
                $"internal-native-preserve -o \"{outputGtxPath}\" \"{inputDdsPath}\"");
            return true;
        }

        private static bool TryReadMetadata(string gtxPath, out NativeGtxMetadata metadata, out string error)
        {
            metadata = default!;
            error = string.Empty;

            if (!File.Exists(gtxPath))
            {
                error = "O arquivo GTX informado nao foi encontrado.";
                return false;
            }

            var bytes = File.ReadAllBytes(gtxPath);
            if (bytes.Length < 0x20 || !HasMagic(bytes, 0, GtxMagic))
            {
                error = "O arquivo GTX informado possui cabecalho invalido.";
                return false;
            }

            var majorVersion = ReadUInt32BigEndian(bytes, 0x08);
            var minorVersion = ReadUInt32BigEndian(bytes, 0x0C);
            var headerSize = checked((int)ReadUInt32BigEndian(bytes, 0x04));
            var surfBlockType = majorVersion == 6 && minorVersion == 0 ? 0x0Au : 0x0Bu;
            var dataBlockType = majorVersion == 6 && minorVersion == 0 ? 0x0Bu : 0x0Cu;
            var mipBlockType = majorVersion == 6 && minorVersion == 0 ? 0x0Cu : 0x0Du;

            if (headerSize <= 0 || headerSize > bytes.Length)
            {
                error = "O arquivo GTX possui cabecalho GFD invalido.";
                return false;
            }

            var offset = headerSize;
            int? surfOffset = null;
            int? surfSize = null;
            int? imageDataOffset = null;
            int? imageDataSize = null;
            int? mipDataOffset = null;
            int? mipDataSize = null;

            while (offset + 0x20 <= bytes.Length)
            {
                if (!HasMagic(bytes, offset, BlockMagic))
                {
                    break;
                }

                var blockType = ReadUInt32BigEndian(bytes, offset + 0x10);
                var blockDataSize = checked((int)ReadUInt32BigEndian(bytes, offset + 0x14));
                var blockDataOffset = offset + 0x20;
                if (blockDataOffset + blockDataSize > bytes.Length)
                {
                    error = "O arquivo GTX contem um bloco truncado.";
                    return false;
                }

                if (blockType == surfBlockType)
                {
                    surfOffset = blockDataOffset;
                    surfSize = blockDataSize;
                }
                else if (blockType == dataBlockType)
                {
                    imageDataOffset = blockDataOffset;
                    imageDataSize = blockDataSize;
                }
                else if (blockType == mipBlockType)
                {
                    mipDataOffset = blockDataOffset;
                    mipDataSize = blockDataSize;
                }
                else if (blockType == 0x01)
                {
                    break;
                }

                offset = blockDataOffset + blockDataSize;
            }

            if (!surfOffset.HasValue || !surfSize.HasValue || !imageDataOffset.HasValue || !imageDataSize.HasValue)
            {
                error = "Nao foi possivel localizar os blocos GX2Surface e Image no GTX.";
                return false;
            }

            if (surfSize.Value < 0x88)
            {
                error = "O bloco GX2Surface do GTX esta incompleto.";
                return false;
            }

            var surfaceOffset = surfOffset.Value;
            var componentSelectorOffset = surfaceOffset + 0x84;
            var componentSelector = new[]
            {
                bytes[componentSelectorOffset],
                bytes[componentSelectorOffset + 1],
                bytes[componentSelectorOffset + 2],
                bytes[componentSelectorOffset + 3]
            };

            metadata = new NativeGtxMetadata(
                bytes,
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x00)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x04)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x08)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x0C)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x10)),
                ReadUInt32BigEndian(bytes, surfaceOffset + 0x14),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x18)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x1C)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x20)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x28)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x30)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x34)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x38)),
                checked((int)ReadUInt32BigEndian(bytes, surfaceOffset + 0x3C)),
                componentSelector,
                imageDataOffset.Value,
                imageDataSize.Value,
                mipDataOffset,
                mipDataSize);

            return true;
        }

        private static bool IsSupported(NativeGtxMetadata metadata, out string error)
        {
            if (metadata.Format != SupportedFormat)
            {
                error = $"Formato nao suportado nativamente: 0x{metadata.Format:x} ({metadata.FormatName}).";
                return false;
            }

            if (metadata.Aa != 0)
            {
                error = "O backend nativo atual nao suporta GTX com AA.";
                return false;
            }

            if (metadata.Depth != 1 || metadata.Dim != 1)
            {
                error = "O backend nativo atual so suporta texturas 2D simples.";
                return false;
            }

            if (metadata.NumMips != 1 || metadata.MipSize != 0 || metadata.MipDataSize.GetValueOrDefault() != 0)
            {
                error = "O backend nativo atual so suporta GTX R8_G8 sem mipmaps.";
                return false;
            }

            if (metadata.ImageSize <= 0 || metadata.Pitch <= 0)
            {
                error = "O GTX possui metadados invalidos de imageSize ou pitch.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private static byte[] DeswizzleLevel0(NativeGtxMetadata metadata)
        {
            var linearPadded = TransformSurface(metadata.Width, metadata.Height, metadata.TileMode, metadata.Swizzle, metadata.Pitch, metadata.ImageData, swizzle: false);
            var linearLength = checked(metadata.Width * metadata.Height * BytesPerPixel);
            var linear = new byte[linearLength];
            Buffer.BlockCopy(linearPadded, 0, linear, 0, linear.Length);
            return linear;
        }

        private static byte[] SwizzleLevel0(NativeGtxMetadata metadata, byte[] linearData)
        {
            var paddedLinear = new byte[metadata.ImageSize];
            Buffer.BlockCopy(linearData, 0, paddedLinear, 0, Math.Min(linearData.Length, paddedLinear.Length));
            return TransformSurface(metadata.Width, metadata.Height, metadata.TileMode, metadata.Swizzle, metadata.Pitch, paddedLinear, swizzle: true);
        }

        private static byte[] TransformSurface(int width, int height, int tileMode, int swizzleValue, int pitch, byte[] data, bool swizzle)
        {
            var result = new byte[data.Length];
            var bytesPerPixel = BytesPerPixel;
            var pipeSwizzle = (swizzleValue >> 8) & 1;
            var bankSwizzle = (swizzleValue >> 9) & 3;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    int targetOffset;
                    if (tileMode is 0 or 1)
                    {
                        targetOffset = ComputeSurfaceAddrFromCoordLinear(x, y, bytesPerPixel, pitch, height);
                    }
                    else if (tileMode is 2 or 3)
                    {
                        targetOffset = ComputeSurfaceAddrFromCoordMicroTiled(x, y, BitsPerPixel, pitch, height, tileMode, isDepth: false);
                    }
                    else
                    {
                        targetOffset = ComputeSurfaceAddrFromCoordMacroTiled(
                            x,
                            y,
                            BitsPerPixel,
                            pitch,
                            height,
                            tileMode,
                            isDepth: false,
                            pipeSwizzle,
                            bankSwizzle);
                    }

                    var linearOffset = ((y * width) + x) * bytesPerPixel;
                    if (linearOffset + bytesPerPixel > result.Length || targetOffset + bytesPerPixel > data.Length)
                    {
                        continue;
                    }

                    if (swizzle)
                    {
                        Buffer.BlockCopy(data, linearOffset, result, targetOffset, bytesPerPixel);
                    }
                    else
                    {
                        Buffer.BlockCopy(data, targetOffset, result, linearOffset, bytesPerPixel);
                    }
                }
            }

            return result;
        }

        private static int ComputeSurfaceAddrFromCoordLinear(int x, int y, int bytesPerPixel, int pitch, int height)
        {
            var sliceOffset = pitch * height;
            return (y * pitch + x + sliceOffset * 0) * bytesPerPixel;
        }

        private static int ComputeSurfaceAddrFromCoordMicroTiled(int x, int y, int bitsPerPixel, int pitch, int height, int tileMode, bool isDepth)
        {
            var microTileThickness = tileMode == 3 ? 4 : 1;
            var microTileBytes = (64 * microTileThickness * bitsPerPixel + 7) / 8;
            var microTilesPerRow = pitch >> 3;
            var microTileIndexX = x >> 3;
            var microTileIndexY = y >> 3;
            var microTileOffset = microTileBytes * (microTileIndexX + microTileIndexY * microTilesPerRow);
            var pixelIndex = ComputePixelIndexWithinMicroTile(x, y, 0, bitsPerPixel, tileMode, isDepth);
            var pixelOffset = (bitsPerPixel * pixelIndex) >> 3;
            return pixelOffset + microTileOffset;
        }

        private static int ComputeSurfaceAddrFromCoordMacroTiled(
            int x,
            int y,
            int bitsPerPixel,
            int pitch,
            int height,
            int tileMode,
            bool isDepth,
            int pipeSwizzle,
            int bankSwizzle)
        {
            var microTileThickness = ComputeSurfaceThickness(tileMode);
            var microTileBits = bitsPerPixel * (microTileThickness * 64);
            var microTileBytes = (microTileBits + 7) / 8;
            var pixelIndex = ComputePixelIndexWithinMicroTile(x, y, 0, bitsPerPixel, tileMode, isDepth);
            var elemOffset = (bitsPerPixel * pixelIndex + 7) / 8;

            var pipe = ComputePipeFromCoordWoRotation(x, y);
            var bank = ComputeBankFromCoordWoRotation(x, y);
            var swizzle = pipeSwizzle + 2 * bankSwizzle;
            var bankPipe = pipe + 2 * bank;
            var rotation = ComputeSurfaceRotationFromTileMode(tileMode);
            bankPipe ^= swizzle + 0 * rotation;
            bankPipe %= 8;
            pipe = bankPipe % 2;
            bank = bankPipe / 2;

            var sliceBytes = (height * pitch * microTileThickness * bitsPerPixel + 7) / 8;
            var sliceOffset = sliceBytes * 0;

            var macroTilePitch = 32;
            var macroTileHeight = 16;
            if (tileMode is 5 or 9)
            {
                macroTilePitch = 16;
                macroTileHeight = 32;
            }
            else if (tileMode is 6 or 10)
            {
                macroTilePitch = 8;
                macroTileHeight = 64;
            }

            var macroTilesPerRow = pitch / macroTilePitch;
            var macroTileBytes = (microTileThickness * bitsPerPixel * macroTileHeight * macroTilePitch + 7) / 8;
            var macroTileIndexX = x / macroTilePitch;
            var macroTileIndexY = y / macroTileHeight;
            var macroTileOffset = (macroTileIndexX + macroTilesPerRow * macroTileIndexY) * macroTileBytes;

            if (IsBankSwappedTileMode(tileMode))
            {
                var bankSwapWidth = ComputeSurfaceBankSwappedWidth(tileMode, bitsPerPixel, 1, pitch);
                var swapIndex = macroTilePitch * macroTileIndexX / bankSwapWidth;
                bank ^= BankSwapOrder[swapIndex & 3];
            }

            var totalOffset = elemOffset + ((macroTileOffset + sliceOffset) >> 3);
            return (bank << 9) | (pipe << 8) | (totalOffset & 0xFF) | ((totalOffset & ~0xFF) << 3);
        }

        private static int ComputeSurfaceThickness(int tileMode)
        {
            return tileMode switch
            {
                3 or 7 or 11 or 13 or 15 => 4,
                16 or 17 => 8,
                _ => 1
            };
        }

        private static int ComputePixelIndexWithinMicroTile(int x, int y, int z, int bitsPerPixel, int tileMode, bool isDepth)
        {
            var thickness = ComputeSurfaceThickness(tileMode);
            int pixelBit0;
            int pixelBit1;
            int pixelBit2;
            int pixelBit3;
            int pixelBit4;
            int pixelBit5;

            if (isDepth)
            {
                pixelBit0 = x & 1;
                pixelBit1 = y & 1;
                pixelBit2 = (x & 2) >> 1;
                pixelBit3 = (y & 2) >> 1;
                pixelBit4 = (x & 4) >> 2;
                pixelBit5 = (y & 4) >> 2;
            }
            else if (bitsPerPixel == 0x10)
            {
                pixelBit0 = x & 1;
                pixelBit1 = (x & 2) >> 1;
                pixelBit2 = (x & 4) >> 2;
                pixelBit3 = y & 1;
                pixelBit4 = (y & 2) >> 1;
                pixelBit5 = (y & 4) >> 2;
            }
            else
            {
                pixelBit0 = x & 1;
                pixelBit1 = (x & 2) >> 1;
                pixelBit2 = y & 1;
                pixelBit3 = (x & 4) >> 2;
                pixelBit4 = (y & 2) >> 1;
                pixelBit5 = (y & 4) >> 2;
            }

            var pixelBit6 = thickness > 1 ? z & 1 : 0;
            var pixelBit7 = thickness > 1 ? (z & 2) >> 1 : 0;
            var pixelBit8 = thickness == 8 ? (z & 4) >> 2 : 0;

            return (pixelBit8 << 8) | (pixelBit7 << 7) | (pixelBit6 << 6)
                 | (32 * pixelBit5) | (16 * pixelBit4) | (8 * pixelBit3)
                 | (4 * pixelBit2) | pixelBit0 | (2 * pixelBit1);
        }

        private static int ComputePipeFromCoordWoRotation(int x, int y)
        {
            return ((y >> 3) ^ (x >> 3)) & 1;
        }

        private static int ComputeBankFromCoordWoRotation(int x, int y)
        {
            return (((y >> 5) ^ (x >> 3)) & 1) | (2 * (((y >> 4) ^ (x >> 4)) & 1));
        }

        private static int ComputeSurfaceRotationFromTileMode(int tileMode)
        {
            return tileMode switch
            {
                4 or 5 or 6 or 7 or 8 or 9 or 10 or 11 => 2,
                12 or 13 or 14 or 15 => 1,
                _ => 0
            };
        }

        private static bool IsThickMacroTiled(int tileMode)
        {
            return tileMode is 7 or 11 or 13 or 15;
        }

        private static bool IsBankSwappedTileMode(int tileMode)
        {
            return tileMode is 8 or 9 or 10 or 11 or 14 or 15;
        }

        private static int ComputeMacroTileAspectRatio(int tileMode)
        {
            return tileMode switch
            {
                5 or 9 => 2,
                6 or 10 => 4,
                _ => 1
            };
        }

        private static int ComputeSurfaceBankSwappedWidth(int tileMode, int bitsPerPixel, int numSamples, int pitch)
        {
            if (!IsBankSwappedTileMode(tileMode))
            {
                return 0;
            }

            var bytesPerSample = 8 * bitsPerPixel;
            var samplesPerTile = bytesPerSample != 0 ? 2048 / bytesPerSample : 1;
            var slicesPerTile = Math.Max(1, numSamples / samplesPerTile);

            if (IsThickMacroTiled(tileMode))
            {
                numSamples = 4;
            }

            var bytesPerTileSlice = numSamples * bytesPerSample / slicesPerTile;
            var factor = ComputeMacroTileAspectRatio(tileMode);
            var swapTiles = Math.Max(1, 128 / bitsPerPixel);
            var swapWidth = swapTiles * 32;
            var heightBytes = numSamples * factor * bitsPerPixel * 2 / slicesPerTile;
            var swapMax = 0x4000 / heightBytes;
            var swapMin = 256 / bytesPerTileSlice;

            var bankSwapWidth = Math.Min(swapMax, Math.Max(swapMin, swapWidth));
            while (bankSwapWidth >= 2 * pitch)
            {
                bankSwapWidth >>= 1;
            }

            return bankSwapWidth;
        }

        private static byte[] BuildA8L8DdsHeader(int width, int height)
        {
            var header = new byte[128];
            WriteUInt32LittleEndian(header, 0x00, 0x20534444); // "DDS "
            WriteUInt32LittleEndian(header, 0x04, 124);
            WriteUInt32LittleEndian(header, 0x08, 0x0000100F);
            WriteUInt32LittleEndian(header, 0x0C, (uint)height);
            WriteUInt32LittleEndian(header, 0x10, (uint)width);
            WriteUInt32LittleEndian(header, 0x14, (uint)(width * BytesPerPixel));
            WriteUInt32LittleEndian(header, 0x1C, 1);
            WriteUInt32LittleEndian(header, 0x4C, 32);
            WriteUInt32LittleEndian(header, 0x50, 0x00020001);
            WriteUInt32LittleEndian(header, 0x58, BitsPerPixel);
            WriteUInt32LittleEndian(header, 0x5C, 0x000000FF);
            WriteUInt32LittleEndian(header, 0x60, 0);
            WriteUInt32LittleEndian(header, 0x64, 0);
            WriteUInt32LittleEndian(header, 0x68, 0x0000FF00);
            WriteUInt32LittleEndian(header, 0x6C, 0x00001000);
            return header;
        }

        private static string BuildToolOutput(string gtxPath, NativeGtxMetadata metadata)
        {
            var componentSelectorText = new[]
            {
                SelectorName(metadata.ComponentSelector[0]),
                SelectorName(metadata.ComponentSelector[1]),
                SelectorName(metadata.ComponentSelector[2]),
                SelectorName(metadata.ComponentSelector[3])
            };

            var builder = new StringBuilder();
            builder.AppendLine("GTX Native R8_G8 Backend");
            builder.AppendLine("(C) StarFox Zero Localization Tool");
            builder.AppendLine();
            builder.AppendLine($"Converting: {gtxPath}");
            builder.AppendLine();
            builder.AppendLine("// ----- GX2Surface Info ----- ");
            builder.AppendLine($"  dim             = {metadata.Dim}");
            builder.AppendLine($"  width           = {metadata.Width}");
            builder.AppendLine($"  height          = {metadata.Height}");
            builder.AppendLine($"  depth           = {metadata.Depth}");
            builder.AppendLine($"  numMips         = {metadata.NumMips}");
            builder.AppendLine($"  format          = {metadata.FormatName}");
            builder.AppendLine($"  aa              = {metadata.Aa}");
            builder.AppendLine($"  use             = {metadata.Use}");
            builder.AppendLine($"  imageSize       = {metadata.ImageSize}");
            builder.AppendLine($"  mipSize         = {metadata.MipSize}");
            builder.AppendLine($"  tileMode        = {metadata.TileMode}");
            builder.AppendLine($"  swizzle         = {metadata.Swizzle}, 0x{metadata.Swizzle:x}");
            builder.AppendLine($"  alignment       = {metadata.Alignment}");
            builder.AppendLine($"  pitch           = {metadata.Pitch}");
            builder.AppendLine();
            builder.AppendLine("  GX2 Component Selector:");
            builder.AppendLine($"    Red Channel:    {componentSelectorText[0]}");
            builder.AppendLine($"    Green Channel:  {componentSelectorText[1]}");
            builder.AppendLine($"    Blue Channel:   {componentSelectorText[2]}");
            builder.AppendLine($"    Alpha Channel:  {componentSelectorText[3]}");
            builder.AppendLine();
            builder.AppendLine($"  bits per pixel  = {BitsPerPixel}");
            builder.AppendLine($"  bytes per pixel = {BytesPerPixel}");
            builder.Append($"  realSize        = {metadata.Width * metadata.Height * BytesPerPixel}");
            return builder.ToString();
        }

        private static string BuildComponentSelectorText(byte[] selector)
        {
            return $"{SelectorName(selector[0])}, {SelectorName(selector[1])}, {SelectorName(selector[2])}, {SelectorName(selector[3])}";
        }

        private static string SelectorName(byte value)
        {
            return value switch
            {
                0 => "R",
                1 => "G",
                2 => "B",
                3 => "A",
                4 => "0",
                5 => "1",
                _ => value.ToString()
            };
        }

        private static bool HasMagic(byte[] bytes, int offset, byte[] magic)
        {
            if (offset + magic.Length > bytes.Length)
            {
                return false;
            }

            for (var index = 0; index < magic.Length; index++)
            {
                if (bytes[offset + index] != magic[index])
                {
                    return false;
                }
            }

            return true;
        }

        private static uint ReadUInt32BigEndian(byte[] bytes, int offset)
        {
            return ((uint)bytes[offset] << 24)
                 | ((uint)bytes[offset + 1] << 16)
                 | ((uint)bytes[offset + 2] << 8)
                 | bytes[offset + 3];
        }

        private static void WriteUInt32LittleEndian(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value & 0xFF);
            bytes[offset + 1] = (byte)((value >> 8) & 0xFF);
            bytes[offset + 2] = (byte)((value >> 16) & 0xFF);
            bytes[offset + 3] = (byte)((value >> 24) & 0xFF);
        }

        private readonly record struct NativeGtxMetadata(
            byte[] OriginalBytes,
            int Dim,
            int Width,
            int Height,
            int Depth,
            int NumMips,
            uint Format,
            int Aa,
            int Use,
            int ImageSize,
            int MipSize,
            int TileMode,
            int Swizzle,
            int Alignment,
            int Pitch,
            byte[] ComponentSelector,
            int ImageDataOffset,
            int ImageDataSize,
            int? MipDataOffset,
            int? MipDataSize)
        {
            public byte[] ImageData => OriginalBytes[ImageDataOffset..(ImageDataOffset + ImageDataSize)];

            public string FormatName => Format == SupportedFormat ? SupportedFormatName : $"0x{Format:x}";
        }
    }
}
