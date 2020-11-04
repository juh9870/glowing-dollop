using System;
using System.Text;

namespace Presentation
{
    public abstract class TableOutput<T>
    {
        protected readonly int _columns;
        protected readonly int[] _colWidths;
        protected readonly Charset _chars;

        public TableOutput(int[] columnsWidths, Charset charset)
        {
            _chars = charset;
            _colWidths = columnsWidths;
            _columns = columnsWidths.Length;
        }

        private string FormatString(string name, int length, bool right = false)
        {
            if (name.Length > length)
            {
                name = name.Substring(0, length - 3);
                name += "...";
            }
            else
            {
                name = right ? name.PadLeft(length) : name.PadRight(length);
            }

            return name;
        }

        private string SplitterLine(char[] line)
        {
            var builder = new StringBuilder();
            builder.Append(line[0]);
            for (var i = 0; i < _colWidths.Length; i++)
            {
                if (i != 0) builder.Append(line[1]);
                builder.Append(new string(line[3], _colWidths[i]));
            }
            builder.Append(line[2]);
            return builder.ToString();
        }

        private string TextLine(string[] data)
        {
            var builder = new StringBuilder();
            builder.Append(_chars.LeftVertical);
            for (var i = 0; i < _colWidths.Length; i++)
            {
                if (i != 0) builder.Append(_chars.CentralVertical);
                builder.Append(FormatString(data[i], _colWidths[i]));
            }

            builder.Append(_chars.RightVertical);

            return builder.ToString();
        }

        protected abstract string[] GetHeader();
        protected abstract string[] GetFields(T source);

        public void WriteListContent(T[] list)
        {
            var text = new StringBuilder();
            text.AppendLine(SplitterLine(_chars.TopRow()));
            text.AppendLine(TextLine(GetHeader()));
            text.AppendLine(SplitterLine(_chars.MiddleRow()));

            for (var i = 0; i < list.Length; i++)
            {
                text.AppendLine(TextLine(GetFields(list[i])));
            }

            text.AppendLine(SplitterLine(_chars.BottomRow()));

            Console.WriteLine(text.ToString());
        }
    }

    public struct Charset
    {
        public Charset(char topHorizontal, char centralHorizontal, char bottomHorizontal,
            char leftVertical, char centralVertical, char rightVertical,
            char topLeftCorner, char topIntersection, char topRightCorner,
            char leftIntersection, char centralIntersection, char rightIntersection,
            char bottomLeftCorner, char bottomIntersection, char bottomRightCorner)
        {
            TopHorizontal = topHorizontal;
            CentralHorizontal = centralHorizontal;
            BottomHorizontal = bottomHorizontal;
            LeftVertical = leftVertical;
            CentralVertical = centralVertical;
            RightVertical = rightVertical;
            TopLeftCorner = topLeftCorner;
            TopIntersection = topIntersection;
            TopRightCorner = topRightCorner;
            LeftIntersection = leftIntersection;
            CentralIntersection = centralIntersection;
            RightIntersection = rightIntersection;
            BottomLeftCorner = bottomLeftCorner;
            BottomIntersection = bottomIntersection;
            BottomRightCorner = bottomRightCorner;
        }

        public Charset(char horizontal, char vertical, char topLeftCorner, char topIntersection, char topRightCorner,
            char leftIntersection, char centralIntersection, char rightIntersection, char bottomLeftCorner,
            char bottomIntersection, char bottomRightCorner) :
            this(horizontal, horizontal, horizontal,
                vertical, vertical, vertical,
                topLeftCorner, topIntersection, topRightCorner,
                leftIntersection, centralIntersection, rightIntersection,
                bottomLeftCorner, bottomIntersection, bottomRightCorner)
        {
        }

        public Charset(char horizontal, char vertical, char intersection) : this(horizontal, vertical,
            intersection, intersection, intersection,
            intersection, intersection, intersection,
            intersection, intersection, intersection)
        {
        }

        public Charset(char topHorizontal, char centralHorizontal, char bottomHorizontal,
            char leftVertical, char centralVertical, char rightVertical, char intersection) :
            this(topHorizontal, centralHorizontal, bottomHorizontal,
                leftVertical, centralVertical, rightVertical,
                intersection, intersection, intersection,
                intersection, intersection, intersection,
                intersection, intersection, intersection)
        {
        }

        public char[] TopRow()
        {
            return TableArray()[0];
        }

        public char[] MiddleRow()
        {
            return TableArray()[1];
        }

        public char[] BottomRow()
        {
            return TableArray()[2];
        }

        public char[][] TableArray()
        {
            return new[]
            {
                new[] {TopLeftCorner, TopIntersection, TopRightCorner, TopHorizontal},
                new[] {LeftIntersection, CentralIntersection, RightIntersection, CentralHorizontal},
                new[] {BottomLeftCorner, BottomIntersection, BottomRightCorner, BottomHorizontal}
            };
        }

        public char TopHorizontal { get; }
        public char CentralHorizontal { get; }
        public char BottomHorizontal { get; }
        public char LeftVertical { get; }
        public char CentralVertical { get; }
        public char RightVertical { get; }
        public char TopLeftCorner { get; }
        public char TopIntersection { get; }
        public char TopRightCorner { get; }
        public char LeftIntersection { get; }
        public char CentralIntersection { get; }
        public char RightIntersection { get; }
        public char BottomLeftCorner { get; }
        public char BottomIntersection { get; }
        public char BottomRightCorner { get; }
    
        public static Charset SymbolicCharset = new Charset('─', '│',
            '┌', '┬', '┐',
            '├', '┼', '┤',
            '└', '┴', '┘');

        public static Charset BorderlessSymbolicCharset = new Charset(' ', '─', ' ',
            ' ', '│', ' ',
            ' ', '│', ' ',
            '─', '┼', '─',
            ' ', '│', ' ');
        
        public static Charset AsciiCharset = new Charset('-', '|', '+');

        public static Charset BorderlessAsciiCharset = new Charset(' ', '-', ' ',
            ' ', '|', ' ',
            ' ', '|', ' ',
            '-', '+', '-',
            ' ', '|', ' ');
    }
}