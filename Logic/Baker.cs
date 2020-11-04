using System;
using Data.Serialization;

namespace Logic
{
    public class Baker : Person
    {
        public static string TYPE = "Baker";

        public int CakesBaked;

        public Baker(string dataName) : base(dataName)
        {
        }
        public Baker(SerializedData source) : base(source)
        {
        }

        public override string DataType => "Baker";

        protected override SerializedData.Field[] SaveFields()
        {
            return _saveFields(new SerializedData.Field("CakesBaked", CakesBaked.ToString()));
        }

        protected override void LoadField(SerializedData.Field field)
        {
            if (field.Name == "CakesBaked") CakesBaked = int.Parse(field.Value);
            else base.LoadField(field);
        }

        
        public string Bake()
        {
            var num = new Random().Next() % 12 + 4;
            CakesBaked += num;
            return FirstName + " baked " + num + " cakes";
        }
    }
}