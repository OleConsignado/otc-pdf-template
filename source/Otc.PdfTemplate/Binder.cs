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
        List<KeyValuePair<string, string>> _templateParameters = new List<KeyValuePair<string, string>>();
        List<ImageData> _images = new List<ImageData>();
        string _templatePath;

        ///// <summary>
        ///// Make a binding of data to pdf template
        ///// </summary>
        ///// <param name="data">Data</param>
        ///// <param name="templatePath">Path to pdf template file</param>
        ///// <returns>byte array</returns>
        ///// <exception cref="ArgumentNullException"></exception>
        ///// <exception cref="BinderException"></exception>
        //public byte[] Bind(Dictionary<string, string> data, string templatePath)
        //{
        //    return Bind(data, templatePath, new List<ImageData>());
        //}

        ///// <summary>
        ///// Make a binding of data and images to pdf template
        ///// </summary>
        ///// <param name="data">Data</param>
        ///// <param name="templatePath">Path to pdf template file</param>
        ///// <param name="images">List of images to binding to pdf</param>
        ///// <returns>byte array</returns>
        ///// <exception cref="ArgumentNullException"></exception>
        ///// <exception cref="BinderException"></exception>
        //public byte[] Bind(Dictionary<string, string> data, string templatePath, List<ImageData> images)
        //{
        //    if (data == null)
        //        throw new ArgumentNullException(nameof(data));

        //    if (templatePath == null)
        //        throw new ArgumentNullException(nameof(templatePath));

        //    var templateData = LoadTemplateData(templatePath);

        //    CheckParameters(data, templateData);

        //    var byteTemplate = FillForm(data, images, templatePath);

        //    return byteTemplate;
        //}

        private byte[] FillForm(Dictionary<string, string> templateData)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var pdfStamper = new PdfStamper(new PdfReader(_templatePath), memoryStream);
                var acroFields = pdfStamper.AcroFields;

                foreach (var parametro in templateData)
                    acroFields.SetField(parametro.Key, parametro.Value);

                if (_images.Any())
                {
                    foreach (ImageData imagen in _images)
                    {
                        iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(imagen.Image, iTextSharp.text.BaseColor.White);
                        pdfStamper.GetOverContent(1).AddImage(instance, instance.Width, 0.0f, 0.0f, instance.Height, imagen.VerticalPosition, imagen.HorizontalPosition);
                    }
                }

                pdfStamper.FormFlattening = true;
                pdfStamper.Close();

                return memoryStream.ToArray();
            }
        }

        private void CheckParameters(Dictionary<string, string> templateData)
        {
            if (_templateParameters == null)
                throw new ArgumentNullException(nameof(_templateParameters));

            if (templateData.Count != _templateParameters.Count())
                throw new BinderException(BinderCoreError.InvalidParametersNumber);

            if (_templatePath == string.Empty)
                throw new BinderException(BinderCoreError.PathNotFound);

            string notExists = string.Empty;

            foreach (var key in templateData.Keys)
            {
                if (_templateParameters.All(x => x.Key != key))
                    notExists += string.Format("[{0}]", key);
            }

            if (!string.IsNullOrEmpty(notExists))
            {
                var message = string.Format(BinderCoreError.ParametersNotFound.Message, notExists);

                throw new BinderException(new BinderCoreError(BinderCoreError.InvalidParametersNumber.Key, message));
            }
        }

        private Dictionary<string, string> LoadTemplateData()
        {
            PdfReader pdfReader = null;

            try
            {
                pdfReader = new PdfReader(_templatePath);
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

        public IBinder Add(string key, string value)
        {
            _templateParameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public IBinder AddRange(IEnumerable<KeyValuePair<string, string>> values)
        {
            _templateParameters.AddRange(values);
            return this;
        }

        public IBinder AddImage(Image image, float horizontalPosition, float verticalPosition)
        {
            _images.Add(new ImageData() { Image = image, HorizontalPosition = horizontalPosition, VerticalPosition = verticalPosition });
            return this;
        }

        public Image GenerateBarCode(string barcodeSource)
        {
            Barcode128 barcode128 = new Barcode128 { Code = barcodeSource };
            return barcode128.CreateDrawingImage(Color.Black, Color.White);
        }

        public byte[] Generate()
        {
            if (_templateParameters == null)
                throw new ArgumentNullException(nameof(_templateParameters));

            if (_templatePath == null)
                throw new ArgumentNullException(nameof(_templatePath));

            var templateData = LoadTemplateData();

            CheckParameters(templateData);

            var byteTemplate = FillForm(templateData);

            return byteTemplate;
        }

        public IBinder PathFile(string path)
        {
            _templatePath = path;
            return this;
        }
    }
}
