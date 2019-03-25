using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Display;

namespace s10Analyst
{
    public partial class Form1
    {
        public ISymbol GetSimpleFillSymbol(int fillColor,int lineColor)
        {
            ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            simpleFillSymbol.Color = GetRGB(fillColor, 0, 0);
            //simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
            simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;

            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Width = 5;
            //simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDashDotDot;
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDash;
            simpleLineSymbol.Color = GetRGB(0, lineColor,0);
            ISymbol symbol = simpleLineSymbol as ISymbol;
            symbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            simpleFillSymbol.Outline = symbol as ILineSymbol;
            return simpleFillSymbol as ISymbol ;
        }
    }
}
