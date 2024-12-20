using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

public static class XMLUtils
{
    public static async Task ValidateXmlFileAsync(string schemaNamespace, string xsdFilePath, string xmlFilePath)
    {
        var settings = new XmlReaderSettings();
        settings.ValidationType = ValidationType.Schema;
        
        Console.WriteLine("Nombre de schemas utilisés dans la validation : " + settings.Schemas.Count);

        settings.ValidationEventHandler += ValidationCallBack;
        var readItems = XmlReader.Create(xmlFilePath, settings);
        while (readItems.Read()) {}
    }

    private static void ValidationCallBack(object? sender, ValidationEventArgs e)
    {
        if (e.Severity.Equals(XmlSeverityType.Warning))
        {
            Console.Write("WARNING: ");
            Console.WriteLine(e.Message);
        } 
        else if (e.Severity.Equals(XmlSeverityType.Error))
        {
            Console.Write("ERROR: ");
            Console.WriteLine(e.Message);
        }
    }

    public static void XslTransform(string xmlFilePath, string xsltFilePath, string htmlFilePath, string xsltParams, string namespaceUri, string xsltParamsValue)
    {
        XPathDocument xpathDoc = new XPathDocument(xmlFilePath);
        
        // create the XslCompiledTransform and load the style sheet
        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load(xsltFilePath);
        
        // create the parameters
        XsltArgumentList xsltArgs = new XsltArgumentList();
        xsltArgs.AddParam(xsltParams, namespaceUri, xsltParamsValue);
        
        // create the writer to write the output
        XmlTextWriter htmlWriter = new XmlTextWriter(htmlFilePath, Encoding.UTF8);
        xslt.Transform(xpathDoc, xsltArgs, htmlWriter);
    }
}