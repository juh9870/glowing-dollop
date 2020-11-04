using System;
using Data.Serialization;

namespace Data.IO
{
    public abstract class DataProcessor
    {
        public abstract string ProcessWrite(SerializedData[] data);
        public abstract SerializedData[] ProcessRead(string data);
    }
}