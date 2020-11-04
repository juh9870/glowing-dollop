using System.Text;
using System.Text.RegularExpressions;
using Data.Serialization;

namespace Data.IO.Implementation
{
    public class PseudoJsonDataProcessor : DataProcessor
    {
        public override string ProcessWrite(SerializedData[] data)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < data.Length; i++)
            {
                if (i != 0)
                {
                    builder.AppendLine();
                }

                var serializedData = data[i];
                builder.Append(serializedData.Type);
                builder.Append(" ");
                builder.AppendLine(serializedData.Name);
                builder.Append("{");
                for (var j = 0; j < serializedData.Fields.Length; j++)
                {
                    var field = serializedData.Fields[j];
                    builder.Append($"\"{field.Name}\": \"{field.Value}\"");
                    if (j != serializedData.Fields.Length - 1)
                    {
                        builder.Append(",");
                        builder.AppendLine();
                    }
                }

                builder.AppendLine("};");
            }

            return builder.ToString();
        }
        
        private static readonly Regex GlobalMatcher = new Regex(@"(\w+)\s+(\w+)\s*{\s*((?:""\w+""\s*:\s*""(?:\\""|[^""])*"",?\s*)+)\s*};");
        private static readonly Regex FieldsMatcher = new Regex(@"(?:""(\w+)""\s*:\s*""((?:\\""|[^""])*)"",?\s*)");
        

        public override SerializedData[] ProcessRead(string data)
        {
            var entries = GlobalMatcher.Matches(data);
            var retData = new SerializedData[entries.Count];
            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                var type = entry.Groups[1].Value;
                var name = entry.Groups[2].Value;
                var fields = FieldsMatcher.Matches(entry.Groups[3].Value);
                var serialized = retData[i] = new SerializedData(fields.Count)
                {
                    Type = type,
                    Name = name
                };

                for (var j = 0; j < fields.Count; j++)
                {
                    var fieldMatch = fields[j];
                    serialized.Fields[j]=new SerializedData.Field(fieldMatch.Groups[1].Value,fieldMatch.Groups[2].Value);
                }
            }

            return retData;
        }
    }
}