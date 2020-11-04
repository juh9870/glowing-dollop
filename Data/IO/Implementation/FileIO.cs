using System.IO;
using System.Reflection;

namespace Data.IO.Implementation
{
    public static class FileIoSettings
    {
        public static readonly string Path = System.IO.Path.Join(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),"data.txt");
    }
    
    public class FileDataReader : DataReader
    {
        public override string ReadData()
        {
            return File.ReadAllText(FileIoSettings.Path);
        }
    }
    public class FileDataWriter : DataWriter
    {
        public override void WriteData(string data)
        {
            File.WriteAllText(FileIoSettings.Path,data);
        }
    }
}