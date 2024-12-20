using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("infirmiers")]
public class Infirmiers
{
    [XmlElement("infirmier")] public List<Infirmier> _Infirmiers { get; }

    public Infirmiers()
    {
        _Infirmiers = new List<Infirmier>();
    }
    
    public Infirmiers(List<Infirmier> infirmiers) => _Infirmiers = infirmiers;

    public void AddInfirmier(Infirmier infirmier)
    {
        if (infirmier == null)
        {
            throw new ArgumentNullException(nameof(infirmier), "Infirmier cannot be null");
        }
        
        _Infirmiers.Add(infirmier);
    }

    public override string ToString()
    {
        String description = "";
        foreach (var infirmier in _Infirmiers)
        {
            description += $"\t{infirmier}\n";
        }
        
        return description;
    }
}