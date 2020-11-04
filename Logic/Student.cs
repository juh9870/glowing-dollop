using System;
using System.Text.RegularExpressions;
using Data.IO;
using Data.Serialization;

namespace Logic
{
    public class Student : Person
    {
        public static string TYPE = "Student";


        public ushort Grade;
        public string StudentId;

        public Student(string dataName) : base(dataName)
        {
        }
        public Student(SerializedData source) : base(source)
        {
        }

        public override string DataType => TYPE;

        protected override SerializedData.Field[] SaveFields()
        {
            return _saveFields(new SerializedData.Field("Grade", Grade.ToString()),
                new SerializedData.Field("StudentId", StudentId));
        }

        protected override void LoadField(SerializedData.Field field)
        {
            switch (field.Name)
            {
                case "Grade":
                    Grade = ushort.Parse(field.Value);
                    break;
                case "StudentId":
                    StudentId = field.Value;
                    break;
                default:
                    base.LoadField(field);
                    break;
            }
        }

        private static readonly Regex studentIdRegex = new Regex(@"^\d{4}/\w{6}$");
        public static bool CheckStudentId(string id)
        {
            return studentIdRegex.IsMatch(id);
        }

        protected override void Validate()
        {
            base.Validate();
            if (Grade == 0) throw Utils.GenerateMissingFieldException("Grade", "Student");
            if (StudentId == null) throw Utils.GenerateMissingFieldException("StudentId", "Student");
            if (!CheckStudentId(StudentId)) throw Utils.GenerateInvalidFieldValueException("StudentId", "Student");
        }

        public string Study()
        {
            return FirstName + " studied for " + new Random().Next() % 7 + " hours";
        }
    }
}