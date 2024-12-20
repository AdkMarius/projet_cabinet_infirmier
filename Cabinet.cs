using System.Xml;
using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("cabinet", Namespace = "http://univ-grenoble-alpes/fr/l3miage/medical")]
public class Cabinet
{
    private static string _projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
    private static string _filePath = Path.Combine(_projectDirectory, "data/xml/cabinet.xml");


    [XmlElement("nom")] public string Nom {init; get;}
    
    [XmlElement("adresse")] public Adresse Adresse {get; set;}
    
    [XmlElement("infirmiers")] public Infirmiers Infirmiers {get; set;}
    
    [XmlElement("patients")] public Patients Patients {get; set;}
    
    private static XmlDocument _doc = new XmlDocument();
    private static XmlNode _rootNode;
    private static XmlNamespaceManager _nsmgr;
    
    public Cabinet()
    {
    }

    public Cabinet(string nom, Adresse adresse, Infirmiers infirmiers, Patients patients)
    {
        Nom = nom;
        Adresse = adresse;
        Infirmiers = infirmiers;
        Patients = patients;
    }

    // méthode utilisant le parser XmlReader pour récupérer des informations à la volée
    public static void AnalyseGlobale(string filepath)
    {
        var settings = new XmlReaderSettings();
        using (var reader = XmlReader.Create(filepath, settings))
        {
            reader.MoveToContent();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Document:
                        Console.WriteLine("Entering in the document {0}", filepath);
                        break;
                    case XmlNodeType.Element:
                        Console.WriteLine("Entering in the element {0}, number of attributes {1}", 
                            reader.Name, reader.AttributeCount);
                        break;
                    case XmlNodeType.EndElement:
                        Console.WriteLine("Exiting in the element {0}", reader.Name);
                        break;
                    case XmlNodeType.Text:
                        Console.WriteLine("Entering in the text : {0}", reader.Value);
                        break;
                    case XmlNodeType.Attribute:
                        Console.WriteLine("Entering in the attribute {0}, content : {1}", reader.Name, reader.Value);
                        break;
                }
            }
        }
    }

    // méthode utilisée pour récupérer la valeur d'un élément spécifiaue en utilisant le parser XmlReader
    public static List<string> GetElementValues(string filepath, string elementName)
    {
        var settings = new XmlReaderSettings();
        XmlReader reader = XmlReader.Create(filepath, settings);
        List<string> elementValues = new List<string>();
        reader.MoveToContent();
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == elementName)
            {
                elementValues.Add(reader.Value);
            }
        }
        
        return elementValues;
    }

    // méthode utilisée pour compter le nombre d'acte dans le fichier xml
    public static int CountActes(string filepath)
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        XmlReader reader = XmlReader.Create(filepath, settings);
        int nbActes = 0;
        reader.MoveToContent();
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "cb:acte")
                nbActes++;
        }
        
        return nbActes;
    }

    // méthode permettant de charger un document xml
    private static void LoadXMLFile(string filepath, string nsPrefix, string nsURI)
    {
        _doc.Load(filepath);
        
        _rootNode = _doc.DocumentElement ?? throw new ArgumentNullException(nameof(_doc.DocumentElement));
        
        _nsmgr = new XmlNamespaceManager(_doc.NameTable);
        _nsmgr.AddNamespace(nsPrefix, nsURI);
    }

    // méthode utilisée pour retourner une NodeList en appliquant un chemin XPath via DOM
    public static XmlNodeList? GetElements(string filepath, string nsPrefix, string nsURI, string xPathExpression)
    {
        LoadXMLFile(filepath, nsPrefix, nsURI);
        return _rootNode.SelectNodes(xPathExpression, _nsmgr);
    }

    public static bool CheckNurseElementsNumber()
    {
        XmlNodeList? elements = GetElements(
            "./data/xml/cabinet.xml", 
            "cb", 
            "http://univ-grenoble-alpes/fr/l3miage/medical", 
            "//cb:infirmier"
        );

        return (elements != null && elements.Count == 3);
    }

    // méthode pour vérifier le nombre d'infirmier dans le fichier xml
    // ajuster la valeur dans le return selon le nombre d'élément infirmier
    // 3 dans notre cas à l'état initial
    public static bool CheckPatientElementsNumber()
    {
        XmlNodeList? elements = GetElements(
            "./data/xml/cabinet.xml", 
            "cb", 
            "http://univ-grenoble-alpes/fr/l3miage/medical", 
            "//cb:patient"
        );

        return (elements != null && elements.Count == 3);
    }
    
    // méthode permettant de tester la présence d'une adresse dans cabinet
    public static bool CheckCabinetHasAdress()
    {
        XmlNodeList? elements = GetElements(
            "./data/xml/cabinet.xml", 
            "cb", 
            "http://univ-grenoble-alpes/fr/l3miage/medical", 
            "./cb:adresse"
        );

        return elements != null && elements.Count != 0;
    }
    
    // méthode utilisée pour vérifier que chaque patient a une adresse
    public static bool CheckEachPatientHasAdress()
    {
        XmlNodeList? elements = GetElements(
            "./data/xml/cabinet.xml", 
            "cb", 
            "http://univ-grenoble-alpes/fr/l3miage/medical", 
            "//cb:patient/cb:adresse"
        );

        return elements != null && elements.Count == 4;
    }

    public override string ToString()
    {
        String description = "Présentation du Cabinet\n";
        description += "Nom : " + Nom + "\n";
        description += "Adresse : " + Adresse + "\n";
        description += "Liste des infirmiers : \n";
        description += Infirmiers;
        description += "Liste des patients : \n";
        description += Patients;
        
        return description;
    }
    
    // méthode utilisée pour valider un numéro de sécurité sociale
    // par rapport à la date de naissance et le sexe 
    public static bool ValidateSocialSecurityNumber(String numero, DateOnly DateNaissance, Sexe Sexe)
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
    
    // méthode utilisée pour vérifier la validité de toutes les numéros de sécurité sociale
    public static bool ValidateAllSocialSecurityNumber()
    {
        bool isValidated = false;
        
        XmlNodeList? patients = GetElements(
            "./data/xml/cabinet.xml",
            "cb",
            "http://univ-grenoble-alpes/fr/l3miage/medical",
            "//cb:patient"
        );

        if (patients != null)
        {
            foreach (XmlNode patient in patients)
            {
                string numero = patient.SelectSingleNode("cb:numero", _nsmgr).InnerText;
                string sexe = patient.SelectSingleNode("cb:sexe", _nsmgr).InnerText;
                string dateNaissance = patient.SelectSingleNode("cb:naissance", _nsmgr).InnerText;
                
                int year = int.Parse(dateNaissance.Substring(0, 4));
                int month = int.Parse(dateNaissance.Substring(5, 2));
                int day = int.Parse(dateNaissance.Substring(8, 2));
                
                DateOnly naissance = new DateOnly(year, month, day);
                Sexe Sexe = Sexe.M;
                if (sexe.Equals("F"))
                    Sexe = Sexe.F;
                
                isValidated = ValidateSocialSecurityNumber(numero, naissance, Sexe);

                if (isValidated == false)
                {
                    break;
                }
            }
        }
        
        return isValidated;
    }

    /*
     * méthode pour ajouter un infirmier dans le document xml par DOM
     */
    public static void AddInfirmierByDOM(string nom, string prenom)
    {
        LoadXMLFile("data/xml/cabinet.xml", "", "http://univ-grenoble-alpes/fr/l3miage/medical");
        
        XmlNode? infirmiersNode = ((XmlElement)_rootNode).GetElementsByTagName("infirmiers").Item(0);
        if (infirmiersNode != null)
        {
            int maxId = 0;
            foreach (XmlNode? element in infirmiersNode)
            {
                XmlAttribute? attribute = element.Attributes["id"];
                string? identifier = attribute?.Value;
                if (identifier != null)
                {
                    int idInf = int.Parse(identifier);
                    if (idInf > maxId)
                        maxId = idInf;
                }
            }
            
            string id = Convert.ToString(++maxId).PadLeft(3, '0');
            string namespaceURI = _rootNode.NamespaceURI;
            XmlElement infirmierElement = _doc.CreateElement("infirmier", namespaceURI);
        
            XmlElement nomElement = _doc.CreateElement("nom", namespaceURI);
            nomElement.InnerText = nom;
        
            XmlElement prenomElement = _doc.CreateElement("prenom", namespaceURI);
            prenomElement.InnerText = prenom;
        
            string photo = prenom.ToLower() + ".png";
            XmlElement photoElement = _doc.CreateElement("photo", namespaceURI);
            photoElement.InnerText = photo;

            infirmierElement.SetAttribute("id", id);
            infirmierElement.AppendChild(nomElement);
            infirmierElement.AppendChild(prenomElement);
            infirmierElement.AppendChild(photoElement);
        
            infirmiersNode.AppendChild(infirmierElement);
            _doc.Save(_filePath);
        }
    }

    /*
     * méthode pour ajouter un patient dans le fichier xml
     */
    public static void AddPatientByDOM(string nom, string prenom, Sexe sexe, DateOnly naissance, Adresse adresse,
        string numeroSS)
    {
        LoadXMLFile("data/xml/cabinet.xml", "", "http://univ-grenoble-alpes/fr/l3miage/medical");

        XmlNode? patientsNode = ((XmlElement)_rootNode).GetElementsByTagName("patients").Item(0);
        if (patientsNode != null)
        {
            string namespaceURI = _rootNode.NamespaceURI;
            XmlElement patientElement = _doc.CreateElement("patient", namespaceURI);

            XmlElement nomElement = _doc.CreateElement("nom", namespaceURI);
            nomElement.InnerText = nom;

            XmlElement prenomElement = _doc.CreateElement("prenom", namespaceURI);
            prenomElement.InnerText = prenom;

            XmlElement sexeElement = _doc.CreateElement("sexe", namespaceURI);
            sexeElement.InnerText = sexe.ToString();
            
            XmlElement naissanceElement = _doc.CreateElement("naissance", namespaceURI);
            naissanceElement.InnerText = naissance.ToString("yyyy-MM-dd");
            
            XmlElement numeroSecuElement = _doc.CreateElement("numero", namespaceURI);
            numeroSecuElement.InnerText = numeroSS;
            
            XmlElement adresseElement = _doc.CreateElement("adresse", namespaceURI);

            XmlElement adrNumeroElement = null;
            string? adresseNumero = adresse.Numero;
            if (adresseNumero != null)
            {
                adrNumeroElement = _doc.CreateElement("numero", namespaceURI);
                adrNumeroElement.InnerText = adresseNumero;
            }
            
            XmlElement rueElement = _doc.CreateElement("rue", namespaceURI);
            rueElement.InnerText = adresse.Rue;
            
            XmlElement codePostalElement = _doc.CreateElement("codePostal", namespaceURI);
            codePostalElement.InnerText = adresse.CodePostal;
            
            XmlElement villeElement = _doc.CreateElement("ville", namespaceURI);
            villeElement.InnerText = adresse.Ville;

            XmlElement etageElement = null;
            string? etage = adresse.Etage;
            if (etage != null)
            {
                etageElement = _doc.CreateElement("etage", namespaceURI);
                etageElement.InnerText = etage;
            }

            if (adrNumeroElement != null)
                adresseElement.AppendChild(adrNumeroElement);
            
            adresseElement.AppendChild(rueElement);
            adresseElement.AppendChild(codePostalElement);
            adresseElement.AppendChild(villeElement);
            
            if (etageElement != null)
                adresseElement.AppendChild(etageElement);

            patientElement.AppendChild(nomElement);
            patientElement.AppendChild(prenomElement);
            patientElement.AppendChild(sexeElement);
            patientElement.AppendChild(naissanceElement);
            patientElement.AppendChild(numeroSecuElement);
            patientElement.AppendChild(adresseElement);

            patientsNode.AppendChild(patientElement);
            _doc.Save(_filePath);
        }
    }

    /*
     * méthode pour ajouter des visites à un patient
     * param visites : une liste de visite créée à partir de la classe Visite
     * param numeroSS : numero de sécurité sociale du patient
     */
    public static void InsertVisiteInPatient(List<Visite> visites, string numeroSS)
    {
        string filename = "data/xml/cabinet.xml";
        LoadXMLFile(filename, "", "http://univ-grenoble-alpes/fr/l3miage/medical");
        
        XmlNode? patientNode = GetElements(
            filename, 
            "cb", 
            "http://univ-grenoble-alpes/fr/l3miage/medical",
            $"//cb:patient[cb:numero={numeroSS}]")
            .Item(0);
        
        if (patientNode != null)
        {
            string namespaceURI = _rootNode.NamespaceURI;
            XmlNode? adresseElement = ((XmlElement)patientNode).GetElementsByTagName("adresse").Item(0);

            foreach (Visite visite in visites)
            {
                XmlElement visiteElement = _doc.CreateElement("visite", namespaceURI);
                
                visiteElement.SetAttribute("date", visite.Date);
                visiteElement.SetAttribute("intervenant", visite.Intervenant);

                foreach (Acte acte in visite.Actes)
                {
                    XmlElement acteElement = _doc.CreateElement("acte", namespaceURI);
                    acteElement.SetAttribute("id", acte.Id);
                    
                    visiteElement.AppendChild(acteElement);
                }
                
                patientNode.InsertBefore(visiteElement, adresseElement);
            }
            
            _doc.Save(_filePath);
        }
    }

}