using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("adresse")]
public class Adresse
{
    private string? _numero;
    private string _codePostal;
    private string? _etage;
    
    /**
    * _numero can be null
    * but if defined, must be a positive number
    */
    [XmlElement("numero")]
    public string? Numero
    {
        get => _numero;
        set
        {
            if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, "^[0-9]+$"))
            {
                throw new ArgumentException("Numero must be a positive number.", nameof(value));
            }
            _numero = value;
        }
    }
    
    [XmlElement("rue")] 
    public string Rue { get; set; }
    
    /**
     * Proprety CodePostal related to _codePostal field
     * return _codePostal in case of getter
     * check if the zip code match the regular expression
     * if yes, assign value to _codePostal
     * if not, throw argument exception
     */
    [XmlElement("codePostal")]
    public string? CodePostal
    {
        get => _codePostal;
        set
        {
            if (value != null)
            {
                Regex zipCodeRegex = new Regex("[0-9]{5}");
                Match m = zipCodeRegex.Match(value);
                _codePostal = m.Success 
                    ? value 
                    : throw new ArgumentException("The zip code must match the pattern.", nameof(value));
            }
        }
    }
    
    [XmlElement("ville")] 
    public string Ville { get; set; }
    
    /**
    * _etage can be null
    * but if defined, must be a positive number
    */
    [XmlElement("etage")]
    public string? Etage
    {
        get => _etage;
        set
        {
            if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, "^[0-9]+$"))
            {
                throw new ArgumentException("Etage must be a positive number.", nameof(value));
            }
            _etage = value;
        }
    }

    public Adresse()
    {
    }
    
    public Adresse(string rue, string ville, string codePostal, string? etage, string? numero)
    {
        Rue = rue;
        Ville = ville;
        CodePostal = codePostal;
        Etage = etage;
        Numero = numero;
    }
    
    public Adresse(string rue, string ville, string codePostal) : this(rue, ville, codePostal, null, null)
    {
        
    }
    
    public Adresse(string rue, string ville, string codePostal, string numero) : this(rue, ville, codePostal, null, numero)
    {
        
    }

    public override string ToString()
    {
        string description = "";
        description += Numero + " " ?? "";
        description += Rue + ", ";
        description += (Etage != null) ? "Etage " + Etage + ", " : "";
        description += CodePostal + ", ";
        description += Ville;
        return description;
    }
}