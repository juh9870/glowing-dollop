using System;

namespace Data.Serialization
{
    public class SerializedData
    {
        public SerializedData(int length)
        {
            Fields = new Field[length];
        }
        
        public Field[] Fields;
        public string Type;
        public string Name;

        public struct Field
        {

            public Field(string name, string value)
            {
                Name = name;
                Value = value;
            }
            
            public string Name;
            public string Value;
        }
    }
}