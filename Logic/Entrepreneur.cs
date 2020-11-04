using System;
using Data.Serialization;

namespace Logic
{
    public class Entrepreneur : Person
    {
        public static string TYPE = "Entrepreneur";
        public double MonthlyIncome;

        public Entrepreneur(string dataName) : base(dataName)
        {
        }

        public Entrepreneur(SerializedData source) : base(source)
        {
        }

        public override string DataType => TYPE;

        protected override SerializedData.Field[] SaveFields()
        {
            return _saveFields(new SerializedData.Field("MonthlyIncome", MonthlyIncome.ToString()));
        }

        protected override void LoadField(SerializedData.Field field)
        {
            if (field.Name == "MonthlyIncome") MonthlyIncome = double.Parse(field.Value);
            else base.LoadField(field);
        }

        public string Invest()
        {
            var rnd = new Random();
            var num = Math.Round(((rnd.NextDouble() + rnd.NextDouble()) / 2 * 500000 - 250000)*100)/100;
            MonthlyIncome += num;
            return FirstName + " made investment and his monthly income " + (num > 0
                ? "increased"
                : "decreased") + " by " + num.ToString("N2");
        }
    }
}