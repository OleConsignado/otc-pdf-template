using System;

namespace Otc.PdfTemplate.Exceptions
{
    public class PdfTemplateError : Exception
    {
        public PdfTemplateError(string message) : base(message)
        {
        }

        public static readonly PdfTemplateError InvalidParametersNumber = new PdfTemplateError("Número de parâmetros de entrada de dados diferente do número de parâmetros do template");
        public static readonly PdfTemplateError ParametersNotFound = new PdfTemplateError("Os parâmetros {0} não existem no template");
        public static readonly PdfTemplateError PathNotFound = new PdfTemplateError("O caminho do arquivo de template não foi informado.");
    }
}
