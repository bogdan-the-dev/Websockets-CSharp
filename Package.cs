using System.Runtime.Serialization.Formatters.Binary;

namespace WebSockets;

[Serializable]
public struct Package
{
    public string TargetIp;
    public int Value;

    public Package(string targetIp, int value)
    {
        TargetIp = targetIp;
        Value = value;
    }
    
    public byte[] ObjectToByteArray()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, this);

        return ms.ToArray();
    }

    public  void ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        Package obj = (Package) binForm.Deserialize(memStream);

        this = obj;
    }
};

