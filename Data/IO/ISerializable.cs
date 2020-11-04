using Data.Serialization;

namespace Data.IO
{
    public interface ISerializable
    {
        string DataName { get; set; }
        public SerializedData Serialize();
        public object Deserialize(SerializedData source);
    }
}