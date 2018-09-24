using System.Drawing;

namespace Otc.PdfTemplate.Abstractions
{
    public interface IBarcodeGenerator
    {
        /// <summary>
        /// Generate a barcode based on the string passed to the method
        /// </summary>
        /// <param name="barcodeSource">string to convert to barcode</param>
        /// <returns>return a barcode image based in the parameter.</returns>
        Image GenerateBarcode(string barcodeSource);
    }
}
