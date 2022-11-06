using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TodoList
{
    static public class ConsoleHelper
    {
        public static bool EescapePressed { get; private set; } = false;
        public static bool HidePassword { get; set; } = false;
        public static string? ReadLine(string[]? strings = null)
        {
            string buffer = string.Empty;
            string displayString;
            var curPos = Console.GetCursorPosition();
            int pos = 0, curstr = 0, lastLength = 0;
            EescapePressed = false;

            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Escape)
            {
                if (info.Key == ConsoleKey.Backspace && pos > 0)
                {
                    buffer = buffer.Remove((pos--) - 1, 1);
                }
                else if (info.Key == ConsoleKey.Home) { pos = 0; }
                else if (info.Key == ConsoleKey.End) { pos = buffer.Length; }
                else if (info.Key == ConsoleKey.LeftArrow && pos > 0) { pos--; }
                else if (info.Key == ConsoleKey.RightArrow && pos < buffer.Length) { pos++; }
                else if (info.Key == ConsoleKey.DownArrow && strings != null)
                {
                    if (curstr == 0) { curstr = strings.Length - 1; }
                    else             { curstr--;                    }
                    buffer = strings[curstr];
                }
                else if (info.Key == ConsoleKey.UpArrow && strings != null)
                {
                    if (curstr == strings.Length - 1)   {   curstr = 0; }
                    else                                {   curstr++;   }
                    buffer = strings[curstr];
                }
                else
                {
                    if (Regex.IsMatch(info.KeyChar.ToString(), @"[@№;:\(\)\[\]<>\\"",\.\-!#\$%&'\*\+/=\?\^`\{\}\|~\w]+", RegexOptions.IgnoreCase))
                    {
                        buffer = buffer.Insert(pos++, info.KeyChar.ToString());
                    }
                }

                int lenght = lastLength - buffer.Length;

                if (HidePassword) { displayString = new string('*', buffer.Length); }
                else { displayString = buffer;  }

                displayString += new string(' ', lenght > 0 ? lenght : 0);

                Console.SetCursorPosition(curPos.Left, curPos.Top);
                Console.Write(displayString);
                lastLength = buffer.Length;

                if (buffer.Length < pos)   { pos = buffer.Length;    }

                Console.SetCursorPosition(curPos.Left + pos, curPos.Top);

                info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Enter) { return buffer.ToString()??string.Empty; }
            }
            EescapePressed = true;
            return string.Empty;
        }
    }
}
