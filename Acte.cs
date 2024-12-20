using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("acte")]
public class Acte
{
    [XmlAttribute("id")] public string Id { get; set; }
    
    public Acte() { }

    public Acte(string id)
    {
        Id = id;
    }
}