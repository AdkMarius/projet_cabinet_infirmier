using System.Xml.Serialization;

namespace CabinetInfirmier;

[Serializable]
[XmlRoot("patients")]
public class Patients
{
    [XmlElement("patient")] public List<Patient> _Patients { get; }

    public Patients()
    {
        _Patients = new List<Patient>();
    }
    
    public Patients(List<Patient> patients) => _Patients = patients;

    public void AddPatient(Patient? patient)
    {
        if (patient == null)
        {
            throw new ArgumentNullException(nameof(patient), "Patient cannot be null");
        }
        else
        {
            _Patients.Add(patient);
        }
    }
    
    public override string ToString()
    {
        String description = "";
        foreach (var patient in _Patients)
        {
            description += $"\t{patient}\n";
        }
        
        return description;
    }
}