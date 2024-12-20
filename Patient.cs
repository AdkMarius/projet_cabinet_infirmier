using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("patient")]
public class Patient
{
    [XmlElement("nom")] public String Nom { get; set; }
    
    [XmlElement("prenom")] public String Prenom { get; set; }
    
    [XmlElement("sexe")] public Sexe Sexe { get; set; }
    
    [XmlIgnore] public DateOnly DateNaissance { get; set; }
    
    [XmlElement("naissance")]
    public string Naissance
    {
        get => DateNaissance.ToString("yyyy-MM-dd");
        set => DateNaissance = DateOnly.Parse(value);
    }
    
    private string _numeroSecuriteSociale;

    [XmlElement("numero")]
    public String NumeroSecuriteSociale
    {
        get => _numeroSecuriteSociale;
        set
        {
            if (ValiderNumeroSecuriteSociale(value))
                _numeroSecuriteSociale = value;
            else
                throw new ArgumentException("Numero Securite Sociale is not valid.");
        }
    }

    private List<Visite>? _visites;

    [XmlElement("visite")]
    public List<Visite> Visites
    {
        get => _visites;
        set => _visites = value;
    }
    
    [XmlElement("adresse")]
    public Adresse Adresse { get; set; }

    public Patient() { }

    public Patient(String nom, string prenom, Sexe sexe, DateOnly naissance, string numeroSecuriteSociale,
        Adresse adresse, List<Visite> visites)
    {
        Nom = nom;
        Prenom = prenom;
        Sexe = sexe;
        DateNaissance = naissance;
        NumeroSecuriteSociale = numeroSecuriteSociale;
        Adresse = adresse;
        Visites = visites;
    }

    public bool ValiderNumeroSecuriteSociale(String numero)
    {
        if (numero.Length != 15 || !long.TryParse(numero, out _))
            return false;

        int sexe = int.Parse(numero.Substring(0, 1));
        int anneeNaissance = int.Parse(numero.Substring(1, 2));
        int moisNaissance = int.Parse(numero.Substring(3, 2));
        int cle = int.Parse(numero.Substring(13, 2));

        if ((sexe != 1 && sexe != 2) || (sexe == 1 && Sexe != Sexe.M) || (sexe == 2 && Sexe != Sexe.F))
            return false;

        if (anneeNaissance != DateNaissance.Year % 100 || moisNaissance != DateNaissance.Month)
            return false;
        
        return true;
    }
    
    public override string ToString()
    {
        return $"{Nom} {Prenom} {Naissance} {Adresse} {NumeroSecuriteSociale}";
    }
}