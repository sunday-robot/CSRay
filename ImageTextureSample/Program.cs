// See https://aka.ms/new-console-template for more information
using CsRay;
using System.Drawing;

try
{
    var image = Image.FromFile("sample.png");
    var w = image.Width;
    var h = image.Height;
    var imgconv = new ImageConverter();
    var data = (byte[])imgconv.ConvertTo(image, typeof(byte[]));
    Console.WriteLine($"{w}x{h},data size = {data.Length}");

    for (var y = 0; y < h; y++)
    {
        for (var x = 0; x < w; x++)
        {
            var idx = y * w + x * 3;
            Console.Write($"({data[idx]:x},{data[idx + 1]:x},{data[idx + 2]:x}), ");
        }
        Console.WriteLine();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
