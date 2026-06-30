using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StarFoxZeroLocalizationTool.Services
{
    internal sealed class RawR8G8DdsImage
    {
        private const int DdsHeaderSize = 128;

        private RawR8G8DdsImage(byte[] header, byte[] pixelData, int width, int height)
        {
            Header = header;
            PixelData = pixelData;
            Width = width;
            Height = height;
        }

        public byte[] Header { get; }

        public byte[] PixelData { get; }

        public int Width { get; }

        public int Height { get; }

        public static RawR8G8DdsImage Load(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            if (bytes.Length < DdsHeaderSize)
            {
                throw new InvalidOperationException("O arquivo DDS e invalido ou esta incompleto.");
            }

            if (!(bytes[0] == (byte)'D' && bytes[1] == (byte)'D' && bytes[2] == (byte)'S' && bytes[3] == (byte)' '))
            {
                throw new InvalidOperationException("O arquivo informado nao e um DDS valido.");
            }

            var height = BitConverter.ToInt32(bytes, 12);
            var width = BitConverter.ToInt32(bytes, 16);
            if (width <= 0 || height <= 0)
            {
                throw new InvalidOperationException("Nao foi possivel determinar as dimensoes do DDS.");
            }

            var expectedPixelDataLength = checked(width * height * 2);
            if (bytes.Length < DdsHeaderSize + expectedPixelDataLength)
            {
                throw new InvalidOperationException("O DDS nao contem dados suficientes para uma imagem R8_G8.");
            }

            var header = new byte[DdsHeaderSize];
            Buffer.BlockCopy(bytes, 0, header, 0, header.Length);

            var pixelData = new byte[expectedPixelDataLength];
            Buffer.BlockCopy(bytes, DdsHeaderSize, pixelData, 0, pixelData.Length);

            return new RawR8G8DdsImage(header, pixelData, width, height);
        }

        public void Save(string filePath)
        {
            using var stream = File.Create(filePath);
            stream.Write(Header, 0, Header.Length);
            stream.Write(PixelData, 0, PixelData.Length);
        }

        public Bitmap CreateChannelBitmap(int channelIndex)
        {
            ValidateChannelIndex(channelIndex);

            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var channel = GetChannelValue(x, y, channelIndex);
                    var color = Color.FromArgb(255, channel, channel, channel);
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }

        public Bitmap CreateCompositeBitmap()
        {
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var fill = GetChannelValue(x, y, 0);
                    var outline = GetChannelValue(x, y, 1);
                    var red = Math.Min(255, fill + outline / 4);
                    var green = Math.Min(255, fill + outline / 4);
                    var blue = Math.Min(255, outline + fill / 8);
                    bitmap.SetPixel(x, y, Color.FromArgb(255, red, green, blue));
                }
            }

            return bitmap;
        }

        public void ImportChannelFromImage(string imagePath, int channelIndex)
        {
            ValidateChannelIndex(channelIndex);

            using var image = new Bitmap(imagePath);
            if (image.Width != Width || image.Height != Height)
            {
                throw new InvalidOperationException(
                    $"A imagem importada precisa ter exatamente {Width}x{Height} pixels.");
            }

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var color = image.GetPixel(x, y);
                    var value = ExtractMonochromeValue(color);
                    SetChannelValue(x, y, channelIndex, value);
                }
            }
        }

        public void ExportChannelToImage(string imagePath, int channelIndex)
        {
            using var bitmap = CreateChannelBitmap(channelIndex);
            bitmap.Save(imagePath);
        }

        private byte GetChannelValue(int x, int y, int channelIndex)
        {
            var offset = ((y * Width) + x) * 2 + channelIndex;
            return PixelData[offset];
        }

        private void SetChannelValue(int x, int y, int channelIndex, byte value)
        {
            var offset = ((y * Width) + x) * 2 + channelIndex;
            PixelData[offset] = value;
        }

        private static byte ExtractMonochromeValue(Color color)
        {
            if (color.A < 255)
            {
                return color.A;
            }

            return (byte)((color.R + color.G + color.B) / 3);
        }

        private static void ValidateChannelIndex(int channelIndex)
        {
            if (channelIndex is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(channelIndex));
            }
        }
    }
}
