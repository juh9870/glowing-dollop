using System;
using Data.IO;
using Logic;

namespace Presentation
{
    public class PersonConsoleUtils : ConsoleUtils
    {
        private readonly BakerTable _bakerTable = new BakerTable(Charset.SymbolicCharset);
        private readonly EntrepreneurTable _entrepreneurTable = new EntrepreneurTable(Charset.SymbolicCharset);
        private readonly PersonTable _personTable = new PersonTable(Charset.SymbolicCharset);
        private readonly DataStorage _storage = new DataStorage();
        private readonly StudentTable _studentTable = new StudentTable(Charset.SymbolicCharset);

        private readonly DataProcessor _dataProcessor;
        private readonly DataReader _dataReader;
        private readonly DataWriter _dataWriter;

        public PersonConsoleUtils(DataProcessor dataProcessor, DataWriter dataWriter, DataReader dataReader)
        {
            _dataProcessor = dataProcessor;
            _dataReader = dataReader;
            _dataWriter = dataWriter;
        }

        public override void WriteControls()
        {
            _personTable.WriteListContent(_storage.ToArray());
            Console.WriteLine("Possible actions:");
            Console.WriteLine("1: show person");
            Console.WriteLine("2: add person");
            Console.WriteLine("3: remove person");
            Console.WriteLine("4: load from file");
            Console.WriteLine("5: save to file");
            Console.WriteLine("6: Special action");
        }

        public override bool ValidateInputChar(char ch)
        {
            switch (ch)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                    return true;
                default:
                    return false;
            }
        }

        public override void ProcessKeyInput(char ch)
        {
            switch (ch)
            {
                case '1':
                    Show();
                    break;
                case '2':
                    Add();
                    break;
                case '3':
                    Remove();
                    break;
                case '4':
                    _storage.LoadData(_dataReader, _dataProcessor);
                    _storage.ResolveConflicts();
                    Console.WriteLine("Data loaded.");
                    Pause();
                    break;
                case '5':
                    _storage.SaveData(_dataWriter, _dataProcessor);
                    Console.WriteLine("Data saved.");
                    Pause();
                    break;
                case '6':
                    SpecialAction();
                    Pause();
                    break;
            }

            WaitForInput();
        }

        private void Cancel(string reason = "")
        {
            Console.WriteLine(reason + "Operation canceled");
        }

        private void SpecialAction()
        {
            var id = ReadString("Input person id");
            var data = _storage.FindData(id);
            if (data == null)
            {
                Cancel("Person not found. ");
                return;
            }

            if (data is Student student)
            {
                Console.WriteLine("Possible actions:");
                Console.WriteLine("1: Study");
                Console.WriteLine("2: Skydive");
                if (ReadNumber("Input number", 1, 2) == 1)
                {
                    Console.WriteLine(student.Study());
                    return;
                }
            } else
            if (data is Baker baker)
            {
                Console.WriteLine("Possible actions:");
                Console.WriteLine("1: Bake");
                Console.WriteLine("2: Skydive");
                if (ReadNumber("Input number", 1, 2) == 1)
                {
                    Console.WriteLine(baker.Bake());
                    return;
                }
            } else
            if (data is Entrepreneur entrepreneur)
            {
                Console.WriteLine("Possible actions:");
                Console.WriteLine("1: Invest");
                Console.WriteLine("2: Skydive");
                if (ReadNumber("Input number", 1, 2) == 1)
                {
                    Console.WriteLine(entrepreneur.Invest());
                    return;
                }
            }

            Console.WriteLine(data.Dive());
        }

        private void Add()
        {
            var id = ReadString("Input entry name");

            if (_storage.FindData(id) != null)
            {
                Cancel("Person with this ID is already added. ");
                return;
            }

            Console.WriteLine("Input type:");
            Console.WriteLine("1: Student");
            Console.WriteLine("2: Baker");
            Console.WriteLine("3: Entrepreneur");
            var type = ReadNumber("Input type", 1, 3);

            var firstName = ReadString("First name");
            var lastName = ReadString("Last name");
            var birthDate = ReadString("Birth date");
            DateTime date;
            while (!DateTime.TryParse(birthDate, out date))
            {
                Console.WriteLine("Invalid date.");
                birthDate = ReadString("Birth date");
            }

            var canSkydive = ReadBool("Can skydive?");

            if (type == 1)
            {
                var grade = 0;
                while (grade < 1) grade = ReadNumber("Input grade", 0, 9);

                var studId = ReadString("Student ID (dddd/wwwwww)");
                while (!Student.CheckStudentId(studId))
                {
                    Console.WriteLine(
                        "Invalid student ID. Valid format: dddd/wwwwww where d is [0-9] and w is [a-zA-Z]");
                    studId = ReadString("Student ID");
                }

                var stud = new Student(id);
                stud.FirstName = firstName;
                stud.LastName = lastName;
                stud.BirthDate = date;
                stud.Grade = (ushort) grade;
                stud.StudentId = studId;
                stud.CanDive = canSkydive;
                _studentTable.WriteListContent(new[] {stud});

                var add = ReadBool("Confirm adding?");
                if (add) _storage.Add(stud);
            }

            if (type == 2)
            {
                var baked = ReadNumber("Cakes baked", 0);
                var baker = new Baker(id);
                baker.FirstName = firstName;
                baker.LastName = lastName;
                baker.BirthDate = date;
                baker.CakesBaked = baked;
                baker.CanDive = canSkydive;
                _bakerTable.WriteListContent(new[] {baker});

                var add = ReadBool("Confirm adding?");
                if (add) _storage.Add(baker);
            }

            if (type == 3)
            {
                var income = ReadNumber("Monthly income");
                var entrepreneur = new Entrepreneur(id);
                entrepreneur.FirstName = firstName;
                entrepreneur.LastName = lastName;
                entrepreneur.BirthDate = date;
                entrepreneur.MonthlyIncome = income;
                entrepreneur.CanDive = canSkydive;
                _entrepreneurTable.WriteListContent(new[] {entrepreneur});

                var add = ReadBool("Confirm adding?");
                if (add) _storage.Add(entrepreneur);
            }
        }

        private void Show()
        {
            var id = ReadString("Input id");
            var data = _storage.FindData(id);
            if (data == null)
            {
                Cancel("Person not found. ");
                return;
            }

            if (data.DataType == Student.TYPE)
                _studentTable.WriteListContent(new[] {data as Student});
            else if (data.DataType == Baker.TYPE)
                _bakerTable.WriteListContent(new[] {data as Baker});
            else if (data.DataType == Entrepreneur.TYPE)
                _entrepreneurTable.WriteListContent(new[] {data as Entrepreneur});
            Pause();
        }

        private void Remove()
        {
            var id = ReadString("Input id of person to remove");
            var data = _storage.FindData(id);
            if (data == null)
            {
                Cancel("Person not found. ");
                return;
            }

            if (data.DataType == Student.TYPE)
                _studentTable.WriteListContent(new[] {data as Student});
            else if (data.DataType == Baker.TYPE)
                _bakerTable.WriteListContent(new[] {data as Baker});
            else if (data.DataType == Entrepreneur.TYPE)
                _entrepreneurTable.WriteListContent(new[] {data as Entrepreneur});

            var remove = ReadBool("Remove this person? ");
            if (remove) _storage.Remove(id);
        }


        private class PersonTable : TableOutput<Person>
        {
            public PersonTable(Charset charset) : base(new[] {12, 16, 10, 10, 10, 11}, charset)
            {
            }

            protected override string[] GetHeader()
            {
                return new[]
                {
                    "type",
                    "id",
                    "first name",
                    "last name",
                    "birth date",
                    "can skydive"
                };
            }

            protected override string[] GetFields(Person source)
            {
                return new[]
                {
                    source.DataType,
                    source.DataName,
                    source.FirstName,
                    source.LastName,
                    source.BirthDate.ToShortDateString(),
                    source.CanDive.ToString()
                };
            }
        }

        private class StudentTable : TableOutput<Student>
        {
            public StudentTable(Charset charset) : base(new[] {16, 10, 10, 10, 5, 11, 11}, charset)
            {
            }

            protected override string[] GetHeader()
            {
                return new[]
                {
                    "id",
                    "first name",
                    "last name",
                    "birth date",
                    "grade",
                    "student ID",
                    "can skydive"
                };
            }

            protected override string[] GetFields(Student source)
            {
                return new[]
                {
                    source.DataName,
                    source.FirstName,
                    source.LastName,
                    source.BirthDate.ToShortDateString(),
                    source.Grade.ToString(),
                    source.StudentId,
                    source.CanDive.ToString()
                };
            }
        }

        private class BakerTable : TableOutput<Baker>
        {
            public BakerTable(Charset charset) : base(new[] {16, 10, 10, 10, 11, 11}, charset)
            {
            }

            protected override string[] GetHeader()
            {
                return new[]
                {
                    "id",
                    "first name",
                    "last name",
                    "birth date",
                    "cakes baked",
                    "can skydive"
                };
            }

            protected override string[] GetFields(Baker source)
            {
                return new[]
                {
                    source.DataName,
                    source.FirstName,
                    source.LastName,
                    source.BirthDate.ToShortDateString(),
                    source.CakesBaked.ToString(),
                    source.CanDive.ToString()
                };
            }
        }

        private class EntrepreneurTable : TableOutput<Entrepreneur>
        {
            public EntrepreneurTable(Charset charset) : base(new[] {16, 10, 10, 10, 12, 11}, charset)
            {
            }

            protected override string[] GetHeader()
            {
                return new[]
                {
                    "id",
                    "first name",
                    "last name",
                    "birth date",
                    "income",
                    "can skydive"
                };
            }

            protected override string[] GetFields(Entrepreneur source)
            {
                return new[]
                {
                    source.DataName,
                    source.FirstName,
                    source.LastName,
                    source.BirthDate.ToShortDateString(),
                    source.MonthlyIncome > 999_999_999 ? "999,999,999+" :
                    source.MonthlyIncome <= 9_999_999 ? source.MonthlyIncome.ToString("N2") :
                    source.MonthlyIncome.ToString("N0"),
                    source.CanDive.ToString()
                };
            }
        }
    }
}