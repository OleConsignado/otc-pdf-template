namespace Otc.PdfTemplate.Abstractions
{
    public class ImageData
    {
        public  string ImageAttributes { get; set; }
        public System.Drawing.Image Image { get; set; }
        public bool BarCode { get; set; }
        public float HorizontalPosition { get; set; }
        public float VerticalPosition { get; set; }
    }
}
