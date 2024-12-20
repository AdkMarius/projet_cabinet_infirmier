using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("infirmier")]
public class InfirmierRO
{
    private static int _nombreInfirmier;

    private readonly string _nom;
    private readonly string _prenom;
    private readonly string _photo;
    private readonly string _identifier;
    
    [XmlElement("nom")]
    public string Nom => _nom;
    
    [XmlElement("prenom")]
    public string Prenom => _prenom;
    
    [XmlElement("photo")]
    public string Photo => _photo;
    
    [XmlAttribute("id")]
    public string Identifier => _identifier;
    
    public InfirmierRO() { }

    public InfirmierRO(string nom, string prenom)
    {
        _nombreInfirmier++;
        _nom = nom;
        _prenom = prenom;
        _photo = prenom.ToLower() + ".png";
        _identifier = Convert.ToString(_nombreInfirmier).PadLeft(3, '0');
    }

    public override string ToString()
    {
        return $"{Nom} {Prenom} {Photo} {Identifier}";
    }
}