using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("visite")]
public class Visite
{
    
     [XmlIgnore] public DateOnly DateVisite { get; set; }
     
     [XmlAttribute("date")]
     public string Date
     {
         get => DateVisite.ToString("yyyy-MM-dd");
         set => DateVisite = DateOnly.Parse(value);
     }
    
    private string? _intervenant;

    [XmlAttribute("intervenant")]
    public String Intervenant
    {
        get => _intervenant;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _intervenant = value;
                return;
            }

            if (!ValiderIntervenant(value))
            {
                throw new ArgumentException("Intervenant must match one nurse's identifier", nameof(value));
            }

            _intervenant = value;
        }
    }
    
    [XmlElement("acte")] public List<Acte> Actes { get; set; }

    public static Infirmiers Infirmiers { get; set; }

    public Visite()
    {
        Actes = new List<Acte>();
    }

    public Visite(DateOnly date, string intervenant, List<Acte> actes)
    {
        DateVisite = date;
        Intervenant = intervenant;
        Actes = actes;
    }
    
    public Visite(DateOnly date, List<Acte> actes) : this(date, null, actes)
    {
       
    }

    public bool ValiderIntervenant(string intervenant)
    {
        foreach (Infirmier infirmier in Infirmiers._Infirmiers)
        {
            if (intervenant.Equals(infirmier.Identifier))
                return true;
        }
        
        return false;
    }
}