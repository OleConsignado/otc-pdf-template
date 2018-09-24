using System.Drawing;
using iTextSharp.text.pdf;
using Otc.PdfTemplate.Abstractions;

namespace Otc.PdfTemplate
{
    public class BarcodeGenerator : IBarcodeGenerator
    {
        /// <summary>
        /// Generate a barcode based on the string passed to the method
        /// </summary>
        /// <param name="barcodeSource">string to convert to barcode</param>
        /// <returns>return a barcode image based in the parameter.</returns>
        public Image GenerateBarcode(string barcodeSource)
        {
            Barcode128 barcode128 = new Barcode128 { Code = barcodeSource };
            return barcode128.CreateDrawingImage(Color.Black, Color.White);
        }
    }
}
