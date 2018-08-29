using iTextSharp.text.pdf;
using Otc.PdfTemplate.Abstractions;
using Otc.PdfTemplate.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Otc.PdfTemplate
{
    public class Binder : IBinder
    {
        /// <summary>
        /// Make a binding of data to pdf template
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="templatePath">Path to pdf template file</param>
        /// <returns>byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BinderException"></exception>
        public byte[] Bind(Dictionary<string, string> data, string templatePath)
        {
            return Bind(data, templatePath, new List<ImageData>());
        }

        /// <summary>
        /// Make a binding of data and images to pdf template
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="templatePath">Path to pdf template file</param>
        /// <param name="images">List of images to binding to pdf</param>
        /// <returns>byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BinderException"></exception>
        public byte[] Bind(Dictionary<string, string> data, string templatePath, List<ImageData> images)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (templatePath == null)
                throw new ArgumentNullException(nameof(templatePath));

            var templateData = LoadTemplateData(templatePath);

            CheckParameters(data, templateData);

            var byteTemplate = FillForm(data, images, templatePath);

            return byteTemplate;
        }

        private static byte[] FillForm(Dictionary<string, string> templateData, ICollection<ImageData> images, string templatePath)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var pdfStamper = new PdfStamper(new PdfReader(templatePath), memoryStream);
                var acroFields = pdfStamper.AcroFields;

                foreach (var parametro in templateData)
                    acroFields.SetField(parametro.Key, parametro.Value);

                if (images.Any())
                {
                    foreach (ImageData imagen in images)
                    {
                        if (imagen.BarCode)
                        {
                            Barcode128 barcode128 = new Barcode128 { Code = imagen.ImageAttributes };
                            iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(barcode128.CreateDrawingImage(Color.Black, Color.White), iTextSharp.text.BaseColor.White);
                            pdfStamper.GetOverContent(1).AddImage(instance, Convert.ToInt32(instance.Width * 0.98), 0.0f, 0.0f, instance.Height, 25f, 445f);
                        }
                        else
                        {
                            iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(imagen.Image, iTextSharp.text.BaseColor.White);
                            pdfStamper.GetOverContent(1).AddImage(instance, instance.Width, 0.0f, 0.0f, instance.Height, imagen.VerticalPosition, imagen.HorizontalPosition);
                        }
                    }
                }

                pdfStamper.FormFlattening = true;
                pdfStamper.Close();

                return memoryStream.ToArray();
            }
        }

        private void CheckParameters(Dictionary<string, string> data, Dictionary<string, string> templateData)
        {
            if (templateData == null) 
                throw new ArgumentNullException(nameof(templateData));

            if(data.Count != templateData.Count())
                throw new BinderException(BinderCoreError.InvalidParametersNumber);

            string notExists = string.Empty;

            foreach (var key in data.Keys)
            {
                if(templateData.All(x => x.Key != key))
                    notExists += string.Format("[{0}]", key);
            }

            if (!string.IsNullOrEmpty(notExists))
            {
                var message = string.Format(BinderCoreError.NotFoundParameters.Message, notExists);

                throw new BinderException(new BinderCoreError(BinderCoreError.InvalidParametersNumber.Key, message));
            }
        }

        private Dictionary<string, string> LoadTemplateData(string path)
        {
            PdfReader pdfReader = null;

            try
            {
                pdfReader = new PdfReader(path);
                var acroFields = pdfReader.AcroFields;
                var parametros = new Dictionary<string, string>();

                foreach (var item in acroFields.Fields.Keys)
                    parametros.Add(item.ToString(), string.Empty);
                
                return parametros;
            }
            finally
            {
                pdfReader?.Close();
            }
        }
    }
}
