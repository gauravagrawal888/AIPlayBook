using JetBrains.Annotations;

namespace Common
{
    [PublicAPI]
    public static class Utils
    {
        public static void WriteLineDarkGray(string text)
        {
            WriteLine(text, ConsoleColor.DarkGray);
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            ConsoleColor orgColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
            }
            finally
            {
                Console.ForegroundColor = orgColor;
            }
        }

        public static void Separator()
        {
            Console.WriteLine();
            WriteLine("".PadLeft(Console.WindowWidth, '-'), ConsoleColor.Gray);
            Console.WriteLine();
        }
    }
}
