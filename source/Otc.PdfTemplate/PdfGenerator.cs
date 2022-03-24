using iTextSharp.text.pdf;
using Otc.PdfTemplate.Abstractions;
using Otc.PdfTemplate.Entities;
using Otc.PdfTemplate.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Otc.PdfTemplate
{
    public class PdfGenerator : IPdfGenerator
    {
        List<KeyValuePair<string, string>> _templateParameters = new List<KeyValuePair<string, string>>();
        List<ImageData> _images = new List<ImageData>();
        string _templatePath;
                
        private byte[] FillForm(List<KeyValuePair<string, string>> templateData)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var pdfStamper = new PdfStamper(new PdfReader(_templatePath), memoryStream, '7');
                var acroFields = pdfStamper.AcroFields;

                foreach (var parametro in templateData)
                    acroFields.SetField(parametro.Key, parametro.Value);

                if (_images.Any())
                {
                    foreach (ImageData imagen in _images)
                    {
                        iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(imagen.Image, iTextSharp.text.BaseColor.White);
                        pdfStamper.GetOverContent(1).AddImage(instance, instance.Width, 0, 0, instance.Height, imagen.HorizontalPosition, imagen.VerticalPosition - 10);
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
                throw PdfTemplateException.InvalidParametersNumber;

            if (_templatePath == string.Empty)
                throw PdfTemplateException.PathNotFound;

            string notExists = string.Empty;

            foreach (var key in templateData.Keys)
            {
                if (_templateParameters.All(x => x.Key != key))
                    notExists += string.Format("[{0}]", key);
            }

            if (!string.IsNullOrEmpty(notExists))
            {
                var message = string.Format(PdfTemplateException.ParametersNotFound.Message, notExists);

                throw PdfTemplateException.InvalidParametersNumber;
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

        public IPdfGenerator Add(string key, string value)
        {
            _templateParameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public IPdfGenerator AddRange(IEnumerable<KeyValuePair<string, string>> values)
        {
            _templateParameters.AddRange(values);
            return this;
        }

        public IPdfGenerator AddImage(Image image, float horizontalPosition, float verticalPosition)
        {
            _images.Add(new ImageData() { Image = image, HorizontalPosition = horizontalPosition, VerticalPosition = verticalPosition });
            return this;
        }

        public byte[] Generate()
        {
            if (_templateParameters == null)
                throw new ArgumentNullException(nameof(_templateParameters));

            if (_templatePath == null)
                throw new ArgumentNullException(nameof(_templatePath));

            var templateData = LoadTemplateData();

            CheckParameters(templateData);

            var byteTemplate = FillForm(_templateParameters);

            return byteTemplate;
        }

        public IPdfGenerator PathFile(string path)
        {
            _templatePath = path;
            return this;
        }
    }
}
