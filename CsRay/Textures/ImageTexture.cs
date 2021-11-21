namespace CsRay
{
#if false
    public sealed class ImageTexture : Texture
    {
        const int _bytesPerPixel = 3;

        readonly int _width;
        readonly int _height;
        readonly int _bytesPerScanline;
        readonly byte[] _data;


        public ImageTexture(string filePath)
        {
#if false
            var componentsPerPixel = _bytesPerPixel;

            _data = stbi_load(
               filePath, out _width, out _height, out componentsPerPixel, _bytesPerPixel);
#endif
            if (_data == null)
            {
                //std::cerr << "ERROR: Could not load texture image file '" << filename << "'.\n";
                _width = _height = 0;
            }

            _bytesPerScanline = _bytesPerPixel * _width;
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
            var pixelIndex = j * _bytesPerScanline + i * _bytesPerPixel;

            return new Rgb(colorScale * _data[pixelIndex], colorScale * _data[pixelIndex + 1], colorScale * _data[pixelIndex + 2]);
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
#endif
}
