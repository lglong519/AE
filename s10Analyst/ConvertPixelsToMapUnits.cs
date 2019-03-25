using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace s10Analyst
{
    public partial class Form1
    {
        public double ConvertPixelsToMapUnits(IActiveView activeView, double pixelUnits)
        {
            tagRECT rect = activeView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
            int pixelExtent = rect.right - rect.left;
            double realWorldDisplayExtent = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return sizeOfOnePixel*pixelUnits;
        }
    }
}
