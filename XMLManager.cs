using System.Xml.Serialization;

namespace CabinetInfirmier;

public class XMLManager<T>
{
    // unmarchalling operation
    public T Load(string path)
    {
        T instance;

        using (TextReader reader = new StreamReader(path))
        {
            var xml = new XmlSerializer(typeof(T));
            
            instance = (T)xml.Deserialize(reader);
        }
        
        return instance;
    }
    
    // marchalling operation
    public void Save(string path, T obj)
    {
        using (TextWriter writer = new StreamWriter(path))
        {
            var xml = new XmlSerializer(typeof(T));
            xml.Serialize(writer, obj);
        }
    }
    
    // marchalling operation with namespace
    public void Save(string path, T obj, XmlSerializerNamespaces ns)
    {
        using (TextWriter writer = new StreamWriter(path))
        {
            var xml = new XmlSerializer(typeof(T));
            xml.Serialize(writer, obj, ns);
        }
    }
}