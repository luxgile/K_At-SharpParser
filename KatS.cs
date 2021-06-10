using System;
using System.Collections.Generic;
using System.IO;

namespace KatS
{
    public static class KatS
    {
        private static bool error;

        static int Main(string[] args)
        {
            //Expr expression = new Binary()
            //{
            //    left = new Unary()
            //    {
            //        op = new MinusToken(),
            //        right = new Literal() { value = 57 },
            //    },
            //    op = new StarToken(),
            //    right = new Grouping()
            //    {
            //        expresion = new Binary()
            //        {
            //            left = new Literal() { value = 2 },
            //            op = new PlusToken(),
            //            right = new Literal() { value = 15 },
            //        }
            //    },
            //};

            //Console.WriteLine(new AstPrinter().Print(expression));
            //Console.Read();

            switch (args.Length)
            {
                case 0:
                return RunPrompt();

                case 1:
                return RunFile(args[0]);

                default:
                Console.WriteLine("Usage: k@ [script]");
                return 0;
            }
        }

        private static int RunFile(string file)
        {
            string text = File.ReadAllText(Path.GetFullPath(file));
            Run(text);

            if (error)
                return 65;

            return 0;
        }

        private static int RunPrompt()
        {
            while (true)
            {
                Console.Write("\n>");
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;
                Run(line);
                error = false;
            }

            return 0;
        }

        public static Exception ThrowError(string msg, Token token) => ThrowError(msg, token.Lexeme, token.Line);
        public static Exception ThrowError(string msg, string file, int line)
        {
            Console.WriteLine($"Error found at {file} at line {line}: {msg}");
            error = true;
            return new Exception(msg);
        }

        private static void Run(string source)
        {
            Scanner scanner = new(source);
            List<Token> tokens = scanner.ScanTokens();

            ScannerGrouper grouper = new(tokens);
            tokens = grouper.GroupTokens();

            tokens.ForEach(x => Console.Write(x.Lexeme));
            Console.Write("\n");

            Parser parser = new(tokens.ToArray());
            Expr expression = parser.Parse();

            if (error) return;

            Console.WriteLine(new AstPrinter().Print(expression));
        }
    }
}
