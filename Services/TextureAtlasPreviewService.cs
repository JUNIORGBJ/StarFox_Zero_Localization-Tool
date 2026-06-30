using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StarFoxZeroLocalizationTool.Services
{
    internal static class TextureAtlasPreviewService
    {
        private static readonly object CacheLock = new();
        private static readonly Dictionary<string, Bitmap> AtlasCache = new(StringComparer.OrdinalIgnoreCase);

        public static bool TryLoadAtlasBitmap(
            string? mcdPath,
            string textureIdHex,
            out Bitmap? atlasBitmap,
            out TextureAtlasInfo? atlasInfo,
            out string error)
        {
            atlasBitmap = null;
            atlasInfo = null;
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(mcdPath) || !File.Exists(mcdPath))
            {
                error = "O arquivo MCD atual nao foi localizado para procurar a textura correspondente.";
                return false;
            }

            if (!TryResolveSiblingTextureContainerPaths(mcdPath, out var wtaPath, out var wtpPath, out error))
            {
                return false;
            }

            if (!uint.TryParse(textureIdHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var textureId))
            {
                error = $"TextureID invalido: {textureIdHex}.";
                return false;
            }

            var cacheKey = BuildCacheKey(wtaPath, wtpPath, textureIdHex);
            lock (CacheLock)
            {
                if (AtlasCache.TryGetValue(cacheKey, out var cachedBitmap))
                {
                    atlasBitmap = (Bitmap)cachedBitmap.Clone();
                    atlasInfo = new TextureAtlasInfo(textureIdHex.ToUpperInvariant(), cacheKey, wtaPath, wtpPath);
                    return true;
                }
            }

            if (!TryExtractWiiUGtxBytes(wtaPath, wtpPath, textureId, out var gtxBytes, out _, out _, out _, out error))
            {
                return false;
            }

            if (!TryConvertGtxBytesToBitmap(gtxBytes, cacheKey, out var decodedBitmap, out error))
            {
                return false;
            }

            if (decodedBitmap == null)
            {
                error = "A conversao da atlas terminou sem gerar imagem.";
                return false;
            }

            lock (CacheLock)
            {
                AtlasCache[cacheKey] = (Bitmap)decodedBitmap.Clone();
            }

            atlasBitmap = decodedBitmap;
            atlasInfo = new TextureAtlasInfo(textureIdHex.ToUpperInvariant(), cacheKey, wtaPath, wtpPath);
            return true;
        }

        public static bool TryExportTextureToDds(string? mcdPath, string textureIdHex, string outputDdsPath, out string error)
        {
            error = string.Empty;
            if (!TryResolveTextureEntry(mcdPath, textureIdHex, out var entry, out error))
            {
                return false;
            }

            var tempDirectory = Path.Combine(Path.GetTempPath(), "StarFoxZeroLocalizationTool", "TextureAtlasPreview");
            Directory.CreateDirectory(tempDirectory);

            var safeKey = MakeSafeFileName(entry.CacheKey);
            var originalGtxPath = Path.Combine(tempDirectory, safeKey + "_export.gtx");
            File.WriteAllBytes(originalGtxPath, entry.GtxBytes);

            if (!NativeR8G8GtxCodec.TryConvertGtxToDds(originalGtxPath, outputDdsPath, out _))
            {
                error = "Nao foi possivel exportar a textura selecionada para DDS.";
                return false;
            }

            return true;
        }

        public static bool TryImportTextureFromDds(string? mcdPath, string textureIdHex, string inputDdsPath, out string error)
        {
            error = string.Empty;
            if (!File.Exists(inputDdsPath))
            {
                error = "O arquivo DDS informado nao foi encontrado.";
                return false;
            }

            if (!TryResolveTextureEntry(mcdPath, textureIdHex, out var entry, out error))
            {
                return false;
            }

            if (entry.NumMips > 1 || entry.MipDataLength > 0)
            {
                error = "A reimportacao por duplo clique suporta apenas texturas R8_G8 sem mipmaps.";
                return false;
            }

            var tempDirectory = Path.Combine(Path.GetTempPath(), "StarFoxZeroLocalizationTool", "TextureAtlasPreview");
            Directory.CreateDirectory(tempDirectory);

            var safeKey = MakeSafeFileName(entry.CacheKey);
            var originalGtxPath = Path.Combine(tempDirectory, safeKey + "_import_original.gtx");
            var importedGtxPath = Path.Combine(tempDirectory, safeKey + "_import_result.gtx");
            File.WriteAllBytes(originalGtxPath, entry.GtxBytes);

            if (!NativeR8G8GtxCodec.TryConvertDdsToGtxPreservingOriginalContainer(inputDdsPath, importedGtxPath, originalGtxPath, out _))
            {
                error = "Nao foi possivel converter o DDS editado de volta para GTX.";
                return false;
            }

            var importedGtxBytes = File.ReadAllBytes(importedGtxPath);
            if (!TryGetImageDataBlock(importedGtxBytes, out var importedImageData, out error))
            {
                return false;
            }

            if (importedImageData.Length != entry.DataLength)
            {
                error = "O tamanho da textura convertida nao corresponde ao espaco original dentro do WTP.";
                return false;
            }

            var wtpBytes = File.ReadAllBytes(entry.WtpPath);
            if (entry.DataOffset < 0 || entry.DataOffset + entry.DataLength > wtpBytes.Length)
            {
                error = "Nao foi possivel localizar a area da textura original dentro do WTP.";
                return false;
            }

            Buffer.BlockCopy(importedImageData, 0, wtpBytes, entry.DataOffset, importedImageData.Length);
            File.WriteAllBytes(entry.WtpPath, wtpBytes);
            RemoveCachedAtlas(entry.CacheKey);
            return true;
        }

        private static bool TryResolveSiblingTextureContainerPaths(
            string mcdPath,
            out string wtaPath,
            out string wtpPath,
            out string error)
        {
            var directory = Path.GetDirectoryName(mcdPath) ?? string.Empty;
            var baseName = Path.GetFileNameWithoutExtension(mcdPath);
            wtaPath = Path.Combine(directory, baseName + ".wta");
            wtpPath = Path.Combine(directory, baseName + ".wtp");

            if (File.Exists(wtaPath) && File.Exists(wtpPath))
            {
                error = string.Empty;
                return true;
            }

            error = $"Nao encontrei os arquivos '{baseName}.wta' e '{baseName}.wtp' ao lado do MCD.";
            return false;
        }

        private static bool TryResolveTextureEntry(
            string? mcdPath,
            string textureIdHex,
            out TextureContainerEntryInfo entry,
            out string error)
        {
            entry = default!;
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(mcdPath) || !File.Exists(mcdPath))
            {
                error = "O arquivo MCD atual nao foi localizado para procurar a textura correspondente.";
                return false;
            }

            if (!TryResolveSiblingTextureContainerPaths(mcdPath, out var wtaPath, out var wtpPath, out error))
            {
                return false;
            }

            if (!uint.TryParse(textureIdHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var textureId))
            {
                error = $"TextureID invalido: {textureIdHex}.";
                return false;
            }

            var cacheKey = BuildCacheKey(wtaPath, wtpPath, textureIdHex);
            if (!TryExtractWiiUGtxBytes(
                wtaPath,
                wtpPath,
                textureId,
                out var gtxBytes,
                out var dataOffset,
                out var dataLength,
                out var numMips,
                out error))
            {
                return false;
            }

            entry = new TextureContainerEntryInfo(
                textureIdHex.ToUpperInvariant(),
                cacheKey,
                wtaPath,
                wtpPath,
                gtxBytes,
                dataOffset,
                dataLength,
                numMips,
                0);
            return true;
        }

        private static bool TryExtractWiiUGtxBytes(
            string wtaPath,
            string wtpPath,
            uint textureId,
            out byte[] gtxBytes,
            out int dataOffset,
            out int dataLength,
            out int numMips,
            out string error)
        {
            gtxBytes = Array.Empty<byte>();
            dataOffset = 0;
            dataLength = 0;
            numMips = 0;
            error = string.Empty;

            var wtaBytes = File.ReadAllBytes(wtaPath);
            var wtpBytes = File.ReadAllBytes(wtpPath);

            if (wtaBytes.Length < 0x24 || wtaBytes[0] != 0x00 || wtaBytes[1] != (byte)'B' || wtaBytes[2] != (byte)'T' || wtaBytes[3] != (byte)'W')
            {
                error = "O arquivo WTA informado nao esta no formato Wii U esperado para esta visualizacao.";
                return false;
            }

            var numTextures = checked((int)ReadUInt32BE(wtaBytes, 0x08));
            var offsetTextureOffsets = checked((int)ReadUInt32BE(wtaBytes, 0x0C));
            var offsetTextureSizes = checked((int)ReadUInt32BE(wtaBytes, 0x10));
            var offsetTextureIdx = checked((int)ReadUInt32BE(wtaBytes, 0x18));
            var offsetTextureInfos = checked((int)ReadUInt32BE(wtaBytes, 0x1C));
            var offsetMipmapOffsets = checked((int)ReadUInt32BE(wtaBytes, 0x20));

            if (numTextures <= 0)
            {
                error = "O WTA nao contem texturas.";
                return false;
            }

            var textureOffsets = ReadUInt32ArrayBE(wtaBytes, offsetTextureOffsets, numTextures);
            var textureSizes = ReadUInt32ArrayBE(wtaBytes, offsetTextureSizes, numTextures);
            var textureIndexes = offsetTextureIdx > 0
                ? ReadUInt32ArrayBE(wtaBytes, offsetTextureIdx, numTextures)
                : Array.Empty<uint>();
            var mipmapOffsets = offsetMipmapOffsets > 0
                ? ReadUInt32ArrayBE(wtaBytes, offsetMipmapOffsets, numTextures)
                : Array.Empty<uint>();

            var selectedIndex = Array.FindIndex(textureIndexes, value => value == textureId);
            if (selectedIndex < 0)
            {
                if (textureIndexes.Length == 1)
                {
                    selectedIndex = 0;
                }
                else
                {
                    error = $"Nao foi encontrada uma textura com TextureID 0x{textureId:X8} dentro do WTA.";
                    return false;
                }
            }

            var textureInfoOffset = offsetTextureInfos + (selectedIndex * 0xC0);
            if (textureInfoOffset + 0x9C > wtaBytes.Length)
            {
                error = "Os metadados GX2Surface da textura estao truncados no WTA.";
                return false;
            }

            var gx2Surface = new byte[0x9C];
            Buffer.BlockCopy(wtaBytes, textureInfoOffset, gx2Surface, 0, gx2Surface.Length);

            numMips = checked((int)ReadUInt32BE(gx2Surface, 0x10));
            dataLength = checked((int)ReadUInt32BE(gx2Surface, 0x20));
            var mipmapLength = checked((int)ReadUInt32BE(gx2Surface, 0x28));
            dataOffset = checked((int)textureOffsets[selectedIndex]);
            var mipOffset = mipmapOffsets.Length > selectedIndex ? checked((int)mipmapOffsets[selectedIndex]) : 0;

            if (dataOffset < 0 || dataOffset + dataLength > wtpBytes.Length)
            {
                error = "A textura principal apontada pelo WTA/WTP esta truncada.";
                return false;
            }

            using var stream = new MemoryStream();
            WriteGfdHeader(stream);
            WriteBlock(stream, 0x0B, gx2Surface);
            WriteBlock(stream, 0x0C, wtpBytes.AsSpan(dataOffset, dataLength));

            if (numMips > 1 && mipmapLength > 0)
            {
                if (mipOffset <= 0 || mipOffset + mipmapLength > wtpBytes.Length)
                {
                    error = "Os mipmaps apontados pelo WTA/WTP estao truncados.";
                    return false;
                }

                WriteBlock(stream, 0x0D, wtpBytes.AsSpan(mipOffset, mipmapLength));
            }

            WriteBlock(stream, 0x01, ReadOnlySpan<byte>.Empty);
            gtxBytes = stream.ToArray();
            return true;
        }

        private static bool TryGetImageDataBlock(byte[] gtxBytes, out byte[] imageData, out string error)
        {
            imageData = Array.Empty<byte>();
            error = string.Empty;

            if (gtxBytes.Length < 0x20 || gtxBytes[0] != (byte)'G' || gtxBytes[1] != (byte)'f' || gtxBytes[2] != (byte)'x' || gtxBytes[3] != (byte)'2')
            {
                error = "O GTX gerado para reimportacao esta invalido.";
                return false;
            }

            var majorVersion = ReadUInt32BE(gtxBytes, 0x08);
            var minorVersion = ReadUInt32BE(gtxBytes, 0x0C);
            var headerSize = checked((int)ReadUInt32BE(gtxBytes, 0x04));
            var imageBlockType = majorVersion == 6 && minorVersion == 0 ? 0x0Bu : 0x0Cu;

            var offset = headerSize;
            while (offset + 0x20 <= gtxBytes.Length)
            {
                if (!(gtxBytes[offset] == (byte)'B' && gtxBytes[offset + 1] == (byte)'L' && gtxBytes[offset + 2] == (byte)'K' && gtxBytes[offset + 3] == (byte)'{'))
                {
                    break;
                }

                var blockType = ReadUInt32BE(gtxBytes, offset + 0x10);
                var blockSize = checked((int)ReadUInt32BE(gtxBytes, offset + 0x14));
                var blockDataOffset = offset + 0x20;
                if (blockDataOffset + blockSize > gtxBytes.Length)
                {
                    error = "O GTX gerado contem um bloco truncado.";
                    return false;
                }

                if (blockType == imageBlockType)
                {
                    imageData = new byte[blockSize];
                    Buffer.BlockCopy(gtxBytes, blockDataOffset, imageData, 0, blockSize);
                    return true;
                }

                if (blockType == 0x01)
                {
                    break;
                }

                offset = blockDataOffset + blockSize;
            }

            error = "Nao foi possivel localizar o bloco de imagem no GTX gerado.";
            return false;
        }

        private static bool TryConvertGtxBytesToBitmap(byte[] gtxBytes, string cacheKey, out Bitmap? bitmap, out string error)
        {
            bitmap = null;
            error = string.Empty;

            var tempDirectory = Path.Combine(Path.GetTempPath(), "StarFoxZeroLocalizationTool", "TextureAtlasPreview");
            Directory.CreateDirectory(tempDirectory);

            var safeKey = MakeSafeFileName(cacheKey);
            var gtxPath = Path.Combine(tempDirectory, safeKey + ".gtx");
            var ddsPath = Path.Combine(tempDirectory, safeKey + ".dds");

            File.WriteAllBytes(gtxPath, gtxBytes);

            if (!NativeR8G8GtxCodec.TryConvertGtxToDds(gtxPath, ddsPath, out _))
            {
                error = "Nao foi possivel converter a textura GTX extraida para um bitmap visualizavel.";
                return false;
            }

            var dds = RawR8G8DdsImage.Load(ddsPath);
            bitmap = dds.CreateCompositeBitmap();
            return true;
        }

        private static string MakeSafeFileName(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = SHA256.HashData(bytes);
            var hashText = Convert.ToHexString(hash, 0, 16);
            return "atlas_" + hashText;
        }

        private static string BuildCacheKey(string wtaPath, string wtpPath, string textureIdHex)
        {
            return $"{wtaPath}|{wtpPath}|{textureIdHex.ToUpperInvariant()}";
        }

        private static void RemoveCachedAtlas(string cacheKey)
        {
            lock (CacheLock)
            {
                if (AtlasCache.TryGetValue(cacheKey, out var cachedBitmap))
                {
                    AtlasCache.Remove(cacheKey);
                    cachedBitmap.Dispose();
                }
            }
        }

        private static void WriteGfdHeader(Stream stream)
        {
            stream.Write(new byte[]
            {
                0x47, 0x66, 0x78, 0x32,
                0x00, 0x00, 0x00, 0x20,
                0x00, 0x00, 0x00, 0x07,
                0x00, 0x00, 0x00, 0x01,
                0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00
            });
        }

        private static void WriteBlock(Stream stream, uint blockType, ReadOnlySpan<byte> data)
        {
            stream.Write(new byte[]
            {
                0x42, 0x4C, 0x4B, 0x7B,
                0x00, 0x00, 0x00, 0x20,
                0x00, 0x00, 0x00, 0x01,
                0x00, 0x00, 0x00, 0x00
            });
            WriteUInt32BE(stream, blockType);
            WriteUInt32BE(stream, checked((uint)data.Length));
            WriteUInt32BE(stream, 0);
            WriteUInt32BE(stream, 0);
            if (!data.IsEmpty)
            {
                stream.Write(data);
            }
        }

        private static void WriteUInt32BE(Stream stream, uint value)
        {
            stream.WriteByte((byte)((value >> 24) & 0xFF));
            stream.WriteByte((byte)((value >> 16) & 0xFF));
            stream.WriteByte((byte)((value >> 8) & 0xFF));
            stream.WriteByte((byte)(value & 0xFF));
        }

        private static uint[] ReadUInt32ArrayBE(byte[] data, int offset, int count)
        {
            var result = new uint[count];
            for (var index = 0; index < count; index++)
            {
                result[index] = ReadUInt32BE(data, offset + (index * 4));
            }

            return result;
        }

        private static uint ReadUInt32BE(byte[] data, int offset)
        {
            return ((uint)data[offset] << 24)
                 | ((uint)data[offset + 1] << 16)
                 | ((uint)data[offset + 2] << 8)
                 | data[offset + 3];
        }

        internal sealed record TextureAtlasInfo(
            string TextureId,
            string CacheKey,
            string WtaPath,
            string WtpPath);

        private sealed record TextureContainerEntryInfo(
            string TextureId,
            string CacheKey,
            string WtaPath,
            string WtpPath,
            byte[] GtxBytes,
            int DataOffset,
            int DataLength,
            int NumMips,
            int MipDataLength);
    }
}
