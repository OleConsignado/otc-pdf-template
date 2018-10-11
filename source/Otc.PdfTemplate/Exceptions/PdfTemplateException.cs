using System;

namespace Otc.PdfTemplate.Exceptions
{
    public class PdfTemplateException : Exception
    {
        public PdfTemplateException(string message) : base(message)
        {
        }

        public static readonly PdfTemplateException InvalidParametersNumber = new PdfTemplateException("Número de parâmetros de entrada de dados diferente do número de parâmetros do template");
        public static readonly PdfTemplateException ParametersNotFound = new PdfTemplateException("Os parâmetros {0} não existem no template");
        public static readonly PdfTemplateException PathNotFound = new PdfTemplateException("O caminho do arquivo de template não foi informado.");
    }
}
