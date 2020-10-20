using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using iText.Forms;
using iText.Kernel.Pdf;
using iText.Pdfa;
using Otc.PdfTemplate.Abstractions;
using Otc.PdfTemplate.Exceptions;

namespace Otc.PdfTemplate
{
    public class PdfAGererator : IPdfGenerator
    {
        List<KeyValuePair<string, string>> _templateParameters =
            new List<KeyValuePair<string, string>>();
        string _templatePath;

        public IPdfGenerator Add(string key, string value)
        {
            _templateParameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public IPdfGenerator AddImage(Image image, float horizontalPosition, float verticalPosition)
        {
            throw new System.NotImplementedException();
        }

        public IPdfGenerator AddRange(IEnumerable<KeyValuePair<string, string>> values)
        {
            _templateParameters.AddRange(values);
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

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfDocument pdfDoc = new PdfADocument(
                    new PdfReader(this._templatePath),
                    new PdfWriter(memoryStream));

                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                foreach (var parametro in _templateParameters)
                    form.GetField(parametro.Key).SetValue(parametro.Value);

                form.FlattenFields();
                pdfDoc.Close();

                return memoryStream.ToArray();
            }
        }

        public IPdfGenerator PathFile(string path)
        {
            _templatePath = path;
            return this;
        }

        private Dictionary<string, string> LoadTemplateData()
        {
            PdfReader pdfReader = null;

            try
            {
                var pdfDoc = new PdfDocument(new PdfReader(_templatePath));
                var form = PdfAcroForm.GetAcroForm(pdfDoc, true);

                var acroFields = form.GetFormFields();
                var parametros = new Dictionary<string, string>();

                foreach (var item in acroFields.Keys)
                    parametros.Add(item.ToString(), string.Empty);

                return parametros;
            }
            finally
            {
                pdfReader?.Close();
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

            foreach (var param in _templateParameters)
            {
                if (templateData.All(x => x.Key != param.Key))
                    notExists += string.Format("[{0}]", param.Key);
            }

            if (!string.IsNullOrEmpty(notExists))
            {
                var message = string.Format(PdfTemplateException.ParametersNotFound.Message, notExists);

                throw new PdfTemplateException(message);
            }
        }
    }
}