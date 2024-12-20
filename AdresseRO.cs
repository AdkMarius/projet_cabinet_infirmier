using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("adresse")]
public class AdresseRO
{
    private readonly string? _numero;
    private readonly string _codePostal;
    private readonly string? _etage;
    private readonly string _rue;
    private readonly string _ville;
    
    /**
    * _numero can be null
    * but if defined, must be a positive number
    */
    [XmlElement("numero")]
    public string? Numero => _numero;
    
    [XmlElement("rue")] 
    public string Rue => _rue;
    
    /**
     * Proprety CodePostal related to _codePostal field
     * return _codePostal in case of getter
     * check if the zip code match the regular expression
     * if yes, assign value to _codePostal
     * if not, throw argument exception
     */
    [XmlElement("codePostal")]
    public string? CodePostal => _codePostal;
    
    [XmlElement("ville")] 
    public string Ville => _ville;
    
    /**
    * _etage can be null
    * but if defined, must be a positive number
    */
    [XmlElement("etage")]
    public string? Etage => _etage;

    public AdresseRO()
    {
    }
    
    public AdresseRO(string rue, string ville, string codePostal, string? etage, string? numero)
    {
        if (!string.IsNullOrEmpty(numero) && !Regex.IsMatch(numero, "^[0-9]+$"))
        {
            throw new ArgumentException("Numero must be a positive number.", nameof(numero));
        }
        
        if (codePostal != null)
        {
            Regex zipCodeRegex = new Regex("[0-9]{5}");
            Match m = zipCodeRegex.Match(codePostal);
            _codePostal = m.Success 
                ? codePostal 
                : throw new ArgumentException("The zip code must match the pattern.", nameof(codePostal));
        }
        
        if (!string.IsNullOrEmpty(etage) && !Regex.IsMatch(etage, "^[0-9]+$"))
        {
            throw new ArgumentException("Etage must be a positive number.", nameof(etage));
        }
        
        _rue = rue;
        _ville = ville;
        _codePostal = codePostal;
        _etage = etage;
        _numero = numero;
    }
    
    public AdresseRO(string rue, string ville, string codePostal) : this(rue, ville, codePostal, null, null)
    {
        
    }
    
    public AdresseRO(string rue, string ville, string codePostal, string numero) : this(rue, ville, codePostal, null, numero)
    {
        
    }
    
    // méthode pour valiser les données après désérialisation
    public void Valider()
    {
        if (!string.IsNullOrEmpty(Numero) && !Regex.IsMatch(Numero, "^[0-9]+$"))
        {
            throw new InvalidOperationException("Numero must be a positive number.");
        }

        if (!Regex.IsMatch(CodePostal, "^[0-9]{5}$"))
        {
            throw new InvalidOperationException("CodePostal must be a 5-digit number.");
        }

        if (!string.IsNullOrEmpty(Etage) && !Regex.IsMatch(Etage, "^[0-9]+$"))
        {
            throw new InvalidOperationException("Etage must be a positive number.");
        }
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