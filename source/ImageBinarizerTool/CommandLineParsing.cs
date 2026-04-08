using Daenet.ImageBinarizerTool.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Daenet.ImageBinarizerTool
{
    /// <summary>
    /// This class is use to Parse the arguments of the Application
    /// </summary>
    public class CommandLineParsing
    {
        #region Private members
        public readonly static List<string> HelpArguments = new List<string> { "-h", "--help" };
        private readonly List<string> inverseArguments = new List<string> { "-inv", "--inverse" };
        private readonly List<string> greyScaleArguments = new List<string> { "-gs", "--greyscale" };
        private readonly List<string> createCodeArguments = new List<string> { "-cc", "--createcode", "--create-code" };
        private readonly List<string> getContourArguments = new List<string> { "-gc", "--getcontour" };

        private List<string> command;        
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor to pass the arguments
        /// </summary>
        /// <param name="args">Input arguments</param>
        public CommandLineParsing(string[] args)
        {
            command = args.ToList();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Conducting parsing process that map the input argument to ImageBinarizerApp.Entities.BinarizerConfiguration object.
        /// </summary>
        /// <param name="config">assign output BinarizerConfiguration object to this variable.</param>
        /// <param name="errMsg">assign output error message to this variable.</param>
        /// <returns>True if no error and false if error exit or help argument called.</returns>
        public bool Parse(out BinarizerConfiguration config, out string errMsg)
        {
            config = new BinarizerConfiguration();

            CorrectArgsIfRequired();

            Dictionary<string, string> switchMappings = GetCommandLineMap();

            try
            {
                var builder = new ConfigurationBuilder().AddCommandLine(command.ToArray(), switchMappings);
                var configRoot = builder.Build();
                configRoot.Bind(config);

            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return false;
            }
            //Console.WriteLine(Configurations.Inverse);
            if (!ValidateArgs(config, out errMsg))
                return false;
            errMsg = null;
            return true;


        }
        #endregion

        #region Private methods
        /// <summary>
        /// Get Dictionary for mapping command line
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetCommandLineMap()
        {
            return new Dictionary<string, string>()
            {
                { "-ip", "inputImagePath"},
                { "--input-image", "inputImagePath"},
                { "-op", "outputImagePath" },
                { "--output-image", "outputImagePath" },
                { "-iw", "imageWidth" },
                { "-ih", "imageHeight"},
                { "-rt", "redThreshold" },
                { "-gt", "greenThreshold" },
                { "-bt", "blueThreshold"},
                { "-grt", "greyThreshold"},
                { "-h", "help"},
                { "-inv", "inverse"},
                { "-gs", "greyScale"},
                { "-cc", "createCode"},
                { "--create-code", "createCode"},
                { "-gc", "getContour"}
            };
        }

        /// <summary>
        /// Corect the arguments input that received type boolean.
        /// </summary>
        private void CorrectArgsIfRequired()
        {            
            //
            //Check help argument
            CheckAndCorrectArgument(HelpArguments, "-h");

            //
            //Check inverse argument
            CheckAndCorrectArgument(inverseArguments, "-inv");

            //
            //Check greyscale argument
            CheckAndCorrectArgument(greyScaleArguments, "-gs");

            //
            //Check createcode argument
            CheckAndCorrectArgument(createCodeArguments, "-cc");

            //
            //Check getcontour argument
            CheckAndCorrectArgument(getContourArguments, "-gc");

        }

        /// <summary>
        /// Check if boolean argument called.
        /// </summary>
        /// <param name="Arguments">List of the arguments for one condition</param>
        /// <param name="argCommand">The argument command to set the condition to true when user use one of the arguments in the parameter Arguments</param>
        private void CheckAndCorrectArgument(List<string> Arguments, string argCommand)
        {
            bool hasArg = false;
            foreach (var arg in Arguments)
            {
                while (command.Contains(arg))
                {
                    command.Remove(arg);
                    hasArg = true;
                }
            }
            if (hasArg)
            {
                command.Add(argCommand);
                command.Add("true");
            }
        }        

        /// <summary>
        /// Check validation of arguments. The method take ImageBinarizerApp.Entities.BinarizerConfiguration object 
        /// as input and check if user input arguments are correct.
        /// </summary>
        /// <param name="Configuration">Configuration for binarization</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns></returns>
        private bool ValidateArgs(BinarizerConfiguration Configuration, out string errMsg)
        {
            //
            //Check if help is call
            if (Configuration.Help)
            {
                PrintHelp();
                errMsg = null;
                return false;
            }

            //
            //Check if input file is valid
            if (!(File.Exists(Configuration.InputImagePath)))
            {
                errMsg = "Input file doesn't exist.";
                return false;
            }

            //
            //Check to set output path when code create is required
            if (Configuration.CreateCode)
            {                
                if (Configuration.OutputImagePath == null)
                    Configuration.OutputImagePath = ".\\LogoPrinter.cs";
            }

            if (Path.GetDirectoryName(Configuration.OutputImagePath) != String.Empty)
            {
                //
                //Check if output dir is valid
                if (!(Directory.Exists(Path.GetDirectoryName(Configuration.OutputImagePath))))
                {
                    errMsg = "Output Directory doesn't exist.";
                    return false;
                }                
            }            

            //
            //Check if width or height input is valid
            if (Configuration.ImageHeight < 0 || Configuration.ImageWidth < 0)
            {
                errMsg = "Height and Width should be larger than 0";
                return false;
            }

            //
            //Check if red threshold is valid
            if ((Configuration.RedThreshold < -1 || Configuration.RedThreshold > 255))
            {
                errMsg = "Red Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if green threshold is valid
            if ((Configuration.GreenThreshold < -1 || Configuration.GreenThreshold > 255))
            {
                errMsg = "Green Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if blue threshold is valid
            if ((Configuration.BlueThreshold < -1 || Configuration.BlueThreshold > 255))
            {
                errMsg = "Blue Threshold should be in between 0 and 255.";
                return false;
            }

            //
            //Check if grey threshold is valid
            if ((Configuration.GreyThreshold < -1 || Configuration.GreyThreshold > 255))
            {
                errMsg = "Grey Threshold should be in between 0 and 255.";
                return false;
            }
            errMsg = null;
            return true;
        }

        /// <summary>
        /// Print help to console
        /// </summary>
        private void PrintHelp()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"\nImageBinarizer [{version.Major}.{version.Minor}.{version.Build}] - .NET Global Tool for image binarization.");
            Console.WriteLine("Copyright \u00a9 daenet GmbH, All rights reserved.\n");

            Console.WriteLine("Usage: imgbin [options]\n");
            Console.WriteLine("Options:");
            Console.WriteLine("  -ip,  --input-image       Input image file path (required)          [string]");
            Console.WriteLine("  -op,  --output-image      Output file path                          [string]");
            Console.WriteLine("  -iw,  --imageWidth        Custom output width (0 = original)        [int, default: 0]");
            Console.WriteLine("  -ih,  --imageHeight       Custom output height (0 = original)       [int, default: 0]");
            Console.WriteLine("  -rt,  --redThreshold      Red channel threshold (-1 = auto)         [int, 0-255, default: -1]");
            Console.WriteLine("  -gt,  --greenThreshold    Green channel threshold (-1 = auto)       [int, 0-255, default: -1]");
            Console.WriteLine("  -bt,  --blueThreshold     Blue channel threshold (-1 = auto)        [int, 0-255, default: -1]");
            Console.WriteLine("  -grt, --greyThreshold     Grey scale threshold (-1 = auto)          [int, 0-255, default: -1]");
            Console.WriteLine("  -inv, --inverse           Inverse binarized image contrast           [flag]");
            Console.WriteLine("  -gs,  --greyscale         Use grey scale threshold mode              [flag]");
            Console.WriteLine("  -cc,  --create-code       Generate .cs code file for logo printing   [flag]");
            Console.WriteLine("  -gc,  --getcontour        Extract contour from image                 [flag]");
            Console.WriteLine("  -h,   --help              Show this help message                     [flag]");

            Console.WriteLine("\nExamples:");
            Console.WriteLine("  imgbin --input-image c:\\a.png --output-image d:\\out.txt");
            Console.WriteLine("  imgbin --input-image c:\\a.png --output-image d:\\out.txt -iw 32 -ih 32");
            Console.WriteLine("  imgbin --input-image c:\\a.png --output-image d:\\out.txt -rt 100 -gt 100 -bt 100");
            Console.WriteLine("  imgbin --input-image c:\\a.png --output-image d:\\out.txt -inv");
            Console.WriteLine("  imgbin --input-image c:\\a.png --output-image d:\\out.txt -iw 32 -ih 32 -grt 100 -gs");
            Console.WriteLine("  imgbin --input-image c:\\a.png --output-image d:\\out.txt -inv -gc");
            Console.WriteLine("  imgbin --input-image c:\\a.png --create-code");
            Console.WriteLine("  imgbin --input-image c:\\a.png -iw 150 --create-code");
        }
        #endregion
    }


}
