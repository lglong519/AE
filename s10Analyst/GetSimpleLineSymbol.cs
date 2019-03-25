using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Display;

namespace s10Analyst
{
    public partial class Form1
    {
        public ISymbol GetSimpleLineSymbol(int width=10)
        {
            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Color = GetRGB(255, 0, 0);
            simpleLineSymbol.Width = width;
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDashDotDot;
            ISymbol symbol=simpleLineSymbol as ISymbol;
            symbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
            return symbol;
        }
    }
}
