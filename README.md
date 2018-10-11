# Template to PDF Binding
[![Build Status](https://travis-ci.org/OleConsignado/otc-pdf-template.svg?branch=master)](https://travis-ci.org/OleConsignado/otc-pdf-template)

![PdfTemplate](https://github.com/OleConsignado/otc-pdf-template/blob/master/pdf.png)

## Description

This library is responsible to make binding between data and images to Pdf Template then buiding a new pdf file.

## How To Use

Create a binder

```var pdfConverter = new PdfConverter();```

Set all parameters and call Generate()
```
pdfConverter.Add("Nome", "Name of user")
	 .Add("CPF", "01234567890")
	 .Add("Identidade", "12457")
	 .Add("Endereco", "rua do nada")
	 .Add("N", "5")
	 .Add("Complemento", "nada")
	 .Add("Bairro", "bairro")
	 .Add("Cidade", "cidade")
	 .Add("UF", "KK")
	 .Add("CEP", "456789")
	 .PathFile(Path.Combine(Directory.GetCurrentDirectory(), "Template.pdf"))
	 .Generate(); 
```	 
At this time, the return is a byte array with the result. 

## Important

In order to deploy your application which uses Otc.Pdf.Template in Linux environments, you probably need to install `libgdiplus`.

Install libgdiplus on Debian based Linux:

```sudo apt-get install libgdiplus```

Dockerfile (for images based on Debian):

```RUN apt-get install -y libgdiplus```
