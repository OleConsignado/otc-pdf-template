using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Otc.PdfTemplate.Abstractions;
using Xunit;

namespace Otc.PdfTemplate.Tests
{
    public class PdfTemplateBind
    {
        private readonly ServiceProvider serviceProvider;

        private IBinder binder;

        public PdfTemplateBind()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IBinder, Binder>();

            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void Make_Data_Merge_With_Template()
        {
            binder = serviceProvider.GetService<IBinder>();

            Assert.True(binder.Add("Nome", "Zé Ruela da Silva")
                         .Add("CPF", "01234567890")
                         .Add("Identidade", "12457")
                         .Add("Endereço", "rua do nada")
                         .Add("N", "5")
                         .Add("Complemento", "nada")
                         .Add("Bairro", "Tabajara")
                         .Add("Cidade", "Dazueira")
                         .Add("UF", "KK")
                         .Add("CEP", "456789")
                         .PathFile(string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), "Template.pdf"))
                         .Generate() != null);
        }

        [Fact]
        public void Make_Data_And_Images_Merge_With_Template()
        {
            binder = serviceProvider.GetService<IBinder>();

            var dictionary = BuildDictionaryForImage();
            var templatePath = string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), "TemplateBoleto.pdf");

            
            
            Assert.True(binder.AddRange(dictionary)
                            .AddImage(binder.GenerateBarCode("03399000000000000009762852800000733268360101"), 50, 465)
                            .PathFile(templatePath)
                            .Generate() != null);
        }

        #region private

        private Dictionary<string, string> BuildDictionary()
        {
            Dictionary<string, string> formModel = new Dictionary<string, string>
            {
                {"Nome", "Zé Ruela da Silva"},
                {"CPF", "01234567890"},
                {"Identidade", "12457"},
                {"Endereço", "rua do nada"},
                {"N", "5"},
                {"Complemento", "nada"},
                {"Bairro", "Tabajara"},
                {"Cidade", "Dazueira"},
                {"UF", "KK"},
                {"CEP", "456789"},
            };

            return formModel;
        }

        private Dictionary<string, string> BuildDictionaryForImage()
        {
            Dictionary<string, string> contractPaymentModel = new Dictionary<string, string>
            {
                { "N_BANCO", "347" },
                { "PREFIXO", "033-7" },
                { "LINHA_DIGITAVEL", "03399.76284 52800.000730 40529.901015 7 0000000" },
                { "PAGADOR", "ZÉ RUELA DA SILVA   010.695.984-02\nRUA VAI QUE COLA, 1726,   StO Cristo\nTabajara - RN \nCEP: 59615270   " },
                { "NOSSO_NUMERO", "000.000.000-42" },
                { "NUMERO_DOCUMENTO", "000.000.000-42" },
                { "DT_VENCIMENTO", "17/08/2018" },
                { "VLR_DOCUMENTO", "35.656,64" },
                { "BENEFICIARIO", "00.000-042" },
                { "AGENCIA_BENEFICIARIA", "181" },
                { "N_BANCO2", "347" },
                { "PREFIXO2", "033-7" },
                { "LINHA_DIGITAVEL2", "03399.76284152800.000730140529.9010151710000000" },
                { "AGENCIA_RECEBEDORA", "Sei nao" },
                { "DT_VENCIMENTO2", "17/08/2018" },
                { "BENEFICIARIO2", "00.000-042" },
                { "AGENCIA_BENEFICIARIA2", "\n181" },
                { "DT_DOCUMENTO", "Yes" },
                { "NUMERO_DOCUMENTO2", "000.000.000-42" },
                { "NOSSO_NUMERO2", "000.000.000-42" },
                { "DT_PROCES", "Yes" },
                { "VALOR", "Yes" },
                { "VLR_DOCUMENTO2", "35.656,64" },
                { "INSTRUCOES", "Yes" },
                { "PAGADOR2", "ZÉ RUELA DA SILVA   000.000.000-42\nRUA VAI QUE COLA, 1726,   StO Cristo\nTabajara - RN \nCEP: 59615270   " }
            };

            return contractPaymentModel;
        }

        #endregion
    }
}