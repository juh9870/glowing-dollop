using System;
using System.Text.RegularExpressions;

namespace Data.IO
{
    public static class Utils
    {
        public static DateTime ParseDate(string date)
        {
            var match = new Regex(@"(\d{2})\.(\d{2})\.(\d{4})").Match(date);
            return new DateTime(int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[1].Value));
        }

        public static InvalidSerializedDataException GenerateUnknownFieldException(string fieldName, string type)
        {
            return new InvalidSerializedDataException($"Unknown field {fieldName} in {type} object");
        }
        public static InvalidSerializedDataException GenerateMissingFieldException(string fieldName, string type)
        {
            return new InvalidSerializedDataException($"Missing field {fieldName} in {type} object");
        }
        public static InvalidSerializedDataException GenerateInvalidFieldValueException(string fieldName, string type)
        {
            return new InvalidSerializedDataException($"Invalid field value for {fieldName} in {type} object");
        }
        public static InvalidSerializedDataException GenerateUnknownTypeException(string type)
        {
            return new InvalidSerializedDataException($"Unknown object type {type}");
        }
        
        public class InvalidSerializedDataException : Exception
        {
            public InvalidSerializedDataException(string? message) : base(message)
            {
            }
        }
    }
}