using Daenet.ImageBinarizerTool.Entities;
using Daenet.Binarizer;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Daenet.ImageBinarizerTool
{
    /// <summary>
    /// Program Class of Console App
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point for Program
        /// </summary>
        /// <param name="args">Argument of main method</param>
        static void Main(string[] args)
        {

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"\nWelcome to ImageBinarizer [{version.Major}.{version.Minor}.{version.Build}]");
            Console.WriteLine("Copyright \u00a9 daenet GmbH, All rights reserved.\n");

            var clr = Console.ForegroundColor;

            if (args.Length == 0)
            {
                LogoPrinter logo = new LogoPrinter();
                logo.Print();

                Console.WriteLine("\nUse \"imgbin -h\" or \"imgbin --help\" for usage information.\n");
                return;
            }

            BinarizerConfiguration configuration;

            if (!(TryParseConfiguration(args, out configuration, out string errMsg)))
            {
                errMsg = errMsg == null ? null : "\nError: " + errMsg;
                PrintMessage(errMsg, ConsoleColor.Red, true);
                return;
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nImage Binarization in progress...");
            Console.ForegroundColor = clr;

            try
            {
                ImageBinarizer img = new ImageBinarizer(configuration);
                img.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine("Image Binarization failed.\n");
                PrintMessage($"\nError: {e.Message}", ConsoleColor.Red, true);
                return;
            }

            if (!configuration.CreateCode)
            {
                PrintMessage($"\nImage Binarization completed. Your Binarized Image is saved at:\n\t{Path.GetFullPath(configuration.OutputImagePath)}", ConsoleColor.Green);
                return;
            }

            PrintMessage($"\nCode file created. Your file is saved at:\n\t{Path.GetFullPath(configuration.OutputImagePath)}", ConsoleColor.Green);
        }

        /// <summary>
        /// Print message 
        /// </summary>
        /// <param name="msg">String of message</param>
        /// <param name="isError">Check if this is an error message</param>
        private static void PrintMessage(string msg = null, ConsoleColor clr = ConsoleColor.White, bool isError = false)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                if (isError)
                {
                    Console.ForegroundColor = clr;
                    Console.Write(msg + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nUse \"imgbin -h\" or \"imgbin --help\" for usage information.\n");
                }
                else
                {
                    Console.ForegroundColor = clr;
                    Console.Write(msg + "\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary>
        /// Check validation of arguments
        /// </summary>
        /// <param name="args">Input arguments</param>
        /// <param name="configurationData">Configurations provided</param>
        /// <returns></returns>
        private static bool TryParseConfiguration(string[] args, out BinarizerConfiguration configurationData, out string errMsg)
        {
            var parsingObject = new CommandLineParsing(args);

            //
            // Check if Parsed data is correct
            return parsingObject.Parse(out configurationData, out errMsg);
        }

    }
}

// --input-image D:\DAENET\image\daenet.png --output-image D:\DAENET\image\out.txt -iw 800 -ih 225 -rt 100 -gt 100 -bt 100