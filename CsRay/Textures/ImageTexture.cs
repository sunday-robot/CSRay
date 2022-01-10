namespace CsRay.Textures
{
    public sealed class ImageTexture : Texture
    {
        readonly int _width;
        readonly int _height;
        readonly byte[] _data;

        public ImageTexture(string filePath)
        {
            (_data, _width, _height) = Bmp.Load(filePath);
        }

        public override Rgb Value(double u, double v, Vec3 p)
        {
            // If we have no texture data, then return solid cyan as a debugging aid.
            if (_data == null)
                return new Rgb(0, 1, 1);

            // Clamp input texture coordinates to [0,1] x [1,0]
            u = Clamp(u, 0.0, 1.0);
            v = 1.0 - Clamp(v, 0.0, 1.0);  // Flip V to image coordinates

            var i = (int)(u * _width);
            var j = (int)(v * _height);

            // Clamp integer mapping, since actual coordinates should be less than 1.0
            if (i >= _width) i = _width - 1;
            if (j >= _height) j = _height - 1;

            var colorScale = 1.0 / 255.0;
            var pixelIndex = (j * _width + i) * 3;

            return new Rgb(colorScale * _data[pixelIndex + 2], colorScale * _data[pixelIndex + 1], colorScale * _data[pixelIndex]);
        }

        static double Clamp(double value, double min, double max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
