using System;
using System.Collections.Generic;
using System.Text;
using Logic;

namespace Presentation
{
    public abstract class ConsoleUtils
    {
        public abstract void WriteControls();
        public abstract void ProcessKeyInput(char ch);
        public abstract bool ValidateInputChar(char ch);

        public void WaitForInput()
        {
            Console.WriteLine();
            WriteControls();

            Console.WriteLine();
            var ch = '\u0000';
            do
            {
                ClearLine();
                var key = Console.ReadKey();
                ch = key.KeyChar;
            } while (!ValidateInputChar(ch));
            Console.WriteLine();

            ProcessKeyInput(ch);
        }

        public static bool ReadBool(string prompt)
        {
            ClearLine();
            Console.WriteLine(prompt);
            Console.Write("Press ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("y");
            Console.ResetColor();
            Console.Write(" for yes or ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("n");
            Console.ResetColor();
            Console.Write(" for no: ");

            var pos = Console.CursorLeft;
            do
            {
                if (Console.CursorLeft != pos)
                {
                    Console.CursorLeft = pos;
                    Console.Write(" ");
                    Console.CursorLeft = pos;
                }

                var key = Console.ReadKey();
                var ch = key.KeyChar;

                switch (ch)
                {
                    case 'y':
                    case 'Y':
                    case '\r':
                        return true;
                    case 'n':
                    case 'N':
                        return false;
                }
            } while (true);
        }

        public static int ReadNumber(string prompt, int min=int.MinValue,int max=int.MaxValue)
        {
            ClearLine();
            int num;
            Console.Write(prompt + ": ");
            var text = Console.ReadLine();
            bool parsed = false;
            while (!(parsed=int.TryParse(text, out num)) || num<min || num>max)
            {
                Console.CursorTop--;
                ClearLine();
                if(!parsed)
                {
                    Console.Write("Invalid number. " + prompt + ": ");
                }
                else
                {
                    Console.Write("Number is out of allowed bounds. " + prompt + ": ");
                }
                text = Console.ReadLine();
            }

            return num;
        }

        public static void Pause()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            ClearLine();
        }

        public static string ReadString(string prompt)
        {
            ClearLine();
            Console.Write(prompt + ": ");
            var text = Console.ReadLine();
            while (text == null || (text = text.Trim()).Length == 0)
            {
                Console.CursorTop--;
                ClearLine();
                Console.Write("Input string empty. " + prompt + ": ");
                text = Console.ReadLine();
            }

            return text;
        }

        private static void ClearLine()
        {
            var curLine = Console.CursorTop;
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, curLine);
        }

        protected string FormatString(string name, int length, bool right = false)
        {
            if (name.Length > length)
            {
                name = name.Substring(0, length - 4);
                name += "...";
            }
            else
            {
                name = right ? name.PadLeft(length) : name.PadRight(length);
            }

            return name;
        }
    }
}