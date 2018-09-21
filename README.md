# Template to PDF Binding
[![Build Status](https://travis-ci.org/OleConsignado/otc-pdf-template.svg?branch=master)](https://travis-ci.org/OleConsignado/otc-pdf-template)

![PdfTemplate](https://github.com/OleConsignado/otc-pdf-template/blob/master/pdf.png)

## Description

This library is responsible to make binding between data and images to Pdf Template then buiding a new pdf file.

#How To Use

Create a binder

```var binder = Binder();```

Set all parameters and call Generate()
```
binder.Add("Nome", "Name of user")
	 .Add("CPF", "01234567890")
	 .Add("Identidade", "12457")
	 .Add("Endereço", "rua do nada")
	 .Add("N", "5")
	 .Add("Complemento", "nada")
	 .Add("Bairro", "bairro")
	 .Add("Cidade", "cidade")
	 .Add("UF", "KK")
	 .Add("CEP", "456789")
	 .PathFile(string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), "Template.pdf"))
	 .Generate(); 
```	 
At this time, the return is a byte array with the result. 

