using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squeak
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: SqueakSpeakRunner <filename.ssp> [-compile] [-debug]");
                return;
            }

            // Setup logging
            var logLevel = args.Contains("-debug") ? LogLevel.Debug : LogLevel.Information;
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(logLevel)
                    .AddConsole();
            });
            var logger = loggerFactory.CreateLogger<SqueakSpeakInterpreterVisitor>();

            try
            {
                string filePath = Path.GetFullPath(args[0]);
                string code = File.ReadAllText(filePath);
                string dir = Path.GetDirectoryName(filePath) ?? "";

                AntlrInputStream input = new AntlrInputStream(code);
                SqueakSpeakLexer lexer = new SqueakSpeakLexer(input);
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                SqueakSpeakParser parser = new SqueakSpeakParser(tokens);

                // Parse
                var tree = parser.program();

                if (args.Contains("-compile"))
                {
                    // Compile to assembly
                    var generator = new SqueakSpeakAssembly();
                    generator.Visit(tree);
                    generator.WriteAssemblyToFile("output.asm");

                    Console.WriteLine($"Assembly code generated:");
                }
                else
                {
                    // Interpret the program
                    var sharedMemory = new Dictionary<string, object>();
                    var interpreter = new SqueakSpeakInterpreterVisitor(sharedMemory, dir, logger);

                    interpreter.Visit(tree);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
