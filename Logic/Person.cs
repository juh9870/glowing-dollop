using System;
using Data.IO;
using Data.Serialization;

namespace Logic
{
    public abstract class Person : ISerializable, ISkydiver
    {
        public string FirstName;
        public string LastName;
        public DateTime BirthDate;

        public abstract string DataType { get; }
        
        public Person(string dataName)
        {
            DataName = dataName;
        }
        public Person(SerializedData source)
        {
            DataName = source.Name;
            Deserialize(source);
        }
        public SerializedData Serialize()
        {
            return new SerializedData(5)
            {
                Fields = SaveFields(),
                Type = DataType,
                Name = DataName
            };
        }
        public object Deserialize(SerializedData source)
        {
            foreach (var field in source.Fields)
            {
                LoadField(field);
            }
            Validate();
            return this;
        }

        protected virtual SerializedData.Field[] SaveFields()
        {
            return _saveFields();
        }
        protected SerializedData.Field[] _saveFields(params SerializedData.Field[] fields)
        {
            var baseFields = new []
            {
                new SerializedData.Field("FirstName",FirstName), 
                new SerializedData.Field("LastName",LastName),
                new SerializedData.Field("BirthDate",BirthDate.ToString("dd.MM.yyyy")), 
                new SerializedData.Field("CanDive",CanDive.ToString()), 
            };
            if (fields.Length <= 0) return baseFields;
            var result = new SerializedData.Field[baseFields.Length + fields.Length];
            baseFields.CopyTo(result, 0);
            fields.CopyTo(result, baseFields.Length);
            return result;
        }

        protected virtual void LoadField(SerializedData.Field field)
        {
            switch (field.Name)
            {
                case "FirstName":
                    FirstName = field.Value;
                    break;
                case "LastName":
                    LastName = field.Value;
                    break;
                case "BirthDate":
                    BirthDate = Utils.ParseDate(field.Value);
                    break;
                case "CanDive":
                    CanDive = bool.Parse(field.Value);
                    break;
                default:
                    throw Utils.GenerateUnknownFieldException(field.Name, "Student");
            }
        }
        
        protected virtual void Validate()
        {
            if (FirstName == null)
            {
                throw Utils.GenerateMissingFieldException("FirstName", "Student");
            }
            if (LastName == null)
            {
                throw Utils.GenerateMissingFieldException("LastName", "Student");
            }
        }
        
        public string Dive()
        {
            if (!CanDive)
            {
                return FirstName + " can't skydive";
            }

            return FirstName + " skydived and enjoyed it";
        }
        public bool CanDive { get; set; }
        public string DataName { get; set; }
    }
}