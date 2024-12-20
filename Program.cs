using System;
using System.Xml.Serialization;

namespace CabinetInfirmier;

public class Program
{
    private static string _projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
    
    public static void Main(string[] args)
    {
        //SerialiserCabinet();
        //Cabinet.AddInfirmierByDOM("Némard", "Jean");
        //Cabinet.AddPatientByDOM("Niskotch", "Nicole", Sexe.F, new DateOnly(1978, 11, 07), new Adresse("Rue des Abattoirs", "Saint Egrève", "38756"), "278113875612393");
        
        Acte acte1 = new Acte("101");
        Acte acte2 = new Acte("102");
        Acte acte3 = new Acte("103");
        
        List<Acte> acteList1 = new List<Acte>();
        acteList1.Add(acte1);
        acteList1.Add(acte2);
        
        List<Acte> acteList2 = new List<Acte>();
        acteList2.Add(acte3);

        Infirmiers infirmiers = CreerInfirmiers();
        Visite.Infirmiers = infirmiers;
        
        Visite visite1 = new Visite(new DateOnly(1989, 08, 15), "002", acteList1);
        Visite visite2 = new Visite(new DateOnly(1990, 08, 15), "003", acteList2);
        
        List<Visite> visiteList = new List<Visite>();
        visiteList.Add(visite1);
        visiteList.Add(visite2);
        
        //Cabinet.InsertVisiteInPatient(visiteList, "278113875612393");

        Console.WriteLine(Cabinet.ValidateAllSocialSecurityNumber());
    }

    public static void SerialiserAdresse()
    {
        string filePath = Path.Combine(_projectDirectory, "data/xml/adresse.xml");       

        XMLManager<Adresse> adresseXmlManager = new XMLManager<Adresse>();
        Adresse adresse = adresseXmlManager.Load(filePath);
        Console.WriteLine(adresse);
        
        Adresse adr = new Adresse("Rue des Alpes", "Nice", "38450", "5", "18");
        adresseXmlManager.Save(filePath, adr);
        
        adresse = adresseXmlManager.Load(filePath);
        Console.WriteLine(adresse);
    }

    public static void SerialiserInfirmier()
    {
        string filePath = Path.Combine(_projectDirectory, "data/xml/infirmier.xml");
        XMLManager<Infirmier> infirmierXmlManager = new XMLManager<Infirmier>();
        Infirmier infirmier = infirmierXmlManager.Load(filePath);
        Console.WriteLine(infirmier);

        infirmier = new Infirmier("Mounier", "Stephan");
        infirmierXmlManager.Save(filePath, infirmier);
        Console.WriteLine(infirmier);
    }

    public static async Task ValidateXmlFile()
    {
        await XMLUtils.ValidateXmlFileAsync(
            "http://univ-grenoble-alpes/fr/l3miage/medical",
            "./data/xml/cabinet.xml",
            "./data/xsd/cabinet.xsd"
        );
    }

    public static void SerialiserCabinet()
    {
        string filePath = Path.Combine(_projectDirectory, "data/xml/cabinet.xml");

        string nom = "Soins à Grenoble";

        Adresse adresse = new Adresse("Rue de la Chimie", "Grenoble", "38041", "60");
        
        // création des infirmiers
        Infirmiers infirmiers = CreerInfirmiers();

        Visite.Infirmiers = infirmiers;

        // creation des patients
        
        // patient Orouge Elvire
        Adresse oAdress = new Adresse("Rond-Point de la Croix de Vie", "La Tronche", "38700");
        Acte oActe = new Acte("101");
        List<Acte> oActes = new List<Acte>();
        oActes.Add(oActe);
        Visite oVisite = new Visite(new DateOnly(2015, 12, 08), oActes);
        List<Visite> oVisites = new List<Visite>();
        oVisites.Add(oVisite);
        Patient orouge = new Patient("Orouge", "Elvire", Sexe.F, new DateOnly(1982, 03, 08), "282036902305274", oAdress,
            oVisites);
        
        // patient Pien Oscare
        Adresse pAdress = new Adresse("Casimir Brenier", "Grenoble", "38000");
        Acte pActe1 = new Acte("101");
        Acte pActe2 = new Acte("102");
        List<Acte> pActes = new List<Acte>();
        pActes.Add(pActe1);
        pActes.Add(pActe2);
        Visite pVisite = new Visite(new DateOnly(2015, 12, 08), "001", pActes);
        List<Visite> pVisites = new List<Visite>();
        pVisites.Add(pVisite);
        Patient pien = new Patient("Pien", "Oscare", Sexe.F, new DateOnly(1975, 03, 25), "275037306569241", pAdress,
            pVisites);
        
        // patient Kapoëla Xavier
        Adresse xAdress = new Adresse("Rue des Martyrs", "Grenoble", "38042", "25");
        Acte xActe = new Acte("101");
        List<Acte> xActes = new List<Acte>();
        xActes.Add(xActe);
        Visite xVisite = new Visite(new DateOnly(2015, 12, 08), "002", xActes);
        List<Visite> xVisites = new List<Visite>();
        xVisites.Add(xVisite);
        Patient xavier = new Patient("Kapoëla", "Xavier", Sexe.M, new DateOnly(2011, 08, 02), "111083306303513", xAdress, xVisites);
        
        Patients patients = new Patients();
        patients.AddPatient(orouge);
        patients.AddPatient(pien);
        patients.AddPatient(xavier);
        
        // creation du cabinet
        Cabinet cabinet = new Cabinet(nom, adresse, infirmiers, patients);
        
        // désérialisation du cabinet
        XMLManager<Cabinet> cabinetXmlManager = new XMLManager<Cabinet>();
        Cabinet cabinetLoaded = cabinetXmlManager.Load(filePath);
        
        Console.WriteLine(cabinetLoaded);
        
        // sérialisation du cabinet
        cabinetXmlManager.Save(filePath, cabinet);
        Cabinet cab = cabinetXmlManager.Load(filePath);
        Console.WriteLine(cab);
    }

    private static Infirmiers CreerInfirmiers()
    {
        Infirmier sarah = new Infirmier("Fréchie", "Sarah");
        Infirmier sophie = new Infirmier("Stické", "Sophie");
        Infirmier paul = new Infirmier("Ochon", "Paul");

        Infirmiers infirmiers = new Infirmiers();
        infirmiers.AddInfirmier(sarah);
        infirmiers.AddInfirmier(sophie);
        infirmiers.AddInfirmier(paul);
        
        return infirmiers;
    }
}