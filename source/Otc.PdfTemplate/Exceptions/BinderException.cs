using Otc.DomainBase.Exceptions;

namespace Otc.PdfTemplate.Exceptions
{
    public class BinderException : CoreException<BinderCoreError>
    {
        public BinderException() : base()
        {
        }

        public BinderException(params BinderCoreError[] errors)
        {
            AddError(errors);
        }
    }
}
