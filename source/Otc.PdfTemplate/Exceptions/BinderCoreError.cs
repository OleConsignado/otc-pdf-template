using Otc.DomainBase.Exceptions;

namespace Otc.PdfTemplate.Exceptions
{
    public class BinderCoreError : CoreError
    {
        public BinderCoreError(string key, string message) : base(key, message)
        {
        }

        public static readonly BinderCoreError InvalidParametersNumber = new BinderCoreError("400.001", "Número de parâmetros de entrada de dados diferente do número de parâmetros do template");
        public static readonly BinderCoreError NotFoundParameters = new BinderCoreError("400.002", "Os parâmetros {0} não existem no template");
    }
}
