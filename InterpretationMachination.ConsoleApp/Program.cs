using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using InterpretationMachination.PascalInterpreter;

namespace InterpretationMachination.ConsoleApp
{
    internal static class Program
    {
        [ExcludeFromCodeCoverage]
        private static void Main(string[] args)
        {
            var c = new SimplePascalInterpreter();

            // A file was specified, run it.
            if (args.Length > 0)
            {
                var filePath = args[0];

                Console.WriteLine($"Attempting to run '{filePath}'...");

                // Move the current dir to the specified file's dir, so files can be called relative to the file (readfile).
                Directory.SetCurrentDirectory(Path.GetDirectoryName(filePath));

                if (File.Exists(Path.GetFileName(filePath)))
                {
                    c.Interpret(File.ReadAllText(Path.GetFileName(filePath)));
                }
                else
                {
                    throw new InvalidEnumArgumentException($"Filepath '{filePath}' does not contain a valid file.");
                }
            }
            else
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                Console.WriteLine("Enter a sum! Ex. calc>6+3");
                Console.Write("calc>");

                var text = Console.ReadLine();

                c.Interpret(text);
            }

            Console.WriteLine("Press key to exit...");
            Console.ReadKey();
        }
    }
}