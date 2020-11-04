using Data.IO;
using Data.Serialization;

namespace Logic
{
    public static class Processor
    {
        public static Person DeserializePerson(SerializedData data)
        {
            if(data.Type==Student.TYPE)return new Student(data);
            if(data.Type==Entrepreneur.TYPE)return new Entrepreneur(data);
            if(data.Type==Baker.TYPE)return new Baker(data);
            throw Utils.GenerateUnknownTypeException(data.Type);
        }

        public static Person[] DeserializePeople(SerializedData[] data)
        {
            var ret = new Person[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                ret[i] = DeserializePerson(data[i]);
            }

            return ret;
        }
    }
}