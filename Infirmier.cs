using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("infirmier")]
public class Infirmier
{
    private static int _nombreInfirmier;
    
    [XmlElement("nom")]
    public string Nom { get; set; }
    
    [XmlElement("prenom")]
    public string Prenom { get; set; }
    
    [XmlElement("photo")]
    public string Photo { get; set; }
    
    [XmlAttribute("id")]
    public string Identifier { get; set; }
    
    public Infirmier() { }

    public Infirmier(string nom, string prenom)
    {
        _nombreInfirmier++;
        Nom = nom;
        Prenom = prenom;
        Photo = prenom.ToLower() + ".png";
        Identifier = Convert.ToString(_nombreInfirmier).PadLeft(3, '0');
    }

    public override string ToString()
    {
        return $"{Nom} {Prenom} {Photo} {Identifier}";
    }
}