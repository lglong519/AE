using ESRI.ArcGIS.Display;

namespace s4Map
{
    class Lib
    {
        public static IRgbColor GetRGB(int r, int g, int b)
        {
            IRgbColor rgb = new RgbColorClass();
            rgb.Red = r;
            rgb.Green = g;
            rgb.Blue = b;
            return rgb;
        }
    }
}
