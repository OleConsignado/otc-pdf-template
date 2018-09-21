using System.Drawing;
using iTextSharp.text.pdf;
using Otc.PdfTemplate.Abstractions;

namespace Otc.PdfTemplate
{
    public class BarcodeGenerator : IBarcodeGenerator
    {
        public Image GenerateBarCode(string barcodeSource)
        {
            Barcode128 barcode128 = new Barcode128 { Code = barcodeSource };
            return barcode128.CreateDrawingImage(Color.Black, Color.White);
        }
    }
}
