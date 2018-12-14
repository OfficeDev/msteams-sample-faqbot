using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FaqMdParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new ParserOptions();
            Parser.Default.ParseArguments<ParserOptions>(args)
                .WithParsed(o =>
                {
                    options = o;
                });

            if (string.IsNullOrWhiteSpace(options.MdFilePath)
                || string.IsNullOrWhiteSpace(options.OutputFileName)
                || string.IsNullOrWhiteSpace(options.OutputDir))
            {
                Console.WriteLine("file, output, and output file name are required.");
                return;
            }

            if (!File.Exists(options.MdFilePath))
            {
                Console.WriteLine($"Could not find file {options.MdFilePath}");
                return;
            }

            if (Path.GetExtension(options.MdFilePath) != ".md")
            {
                Console.WriteLine($"Invalid file extension {options.MdFilePath}. Can only parse .md file");
                return;
            }

            Console.WriteLine("Starting md file parsing");
            string outputFilePath = $"{options.OutputDir}\\{options.OutputFileName}.tsv";

            // lines for tsv file
            var tsvPairLines = new List<string>();
            parseMdFileToTabLines(options.MdFilePath, tsvPairLines, options.AppendContentHeader);

            if (tsvPairLines.Count == 0)
            {
                Console.WriteLine($"No tab pairs lines to output. Skipping file write. Check file source.");
                return;
            }

            if (!Directory.Exists(options.OutputDir))
            {
                Console.WriteLine("Output directory not found. Creating output directory.");
                Directory.CreateDirectory(options.OutputDir);
            }

            // append existing or create new file
            if (options.Overwrite)
            {
                Console.WriteLine($"Creating new file {outputFilePath}");
                File.WriteAllLines(outputFilePath, tsvPairLines);
            }
            else
            {
                File.AppendAllLines(outputFilePath, tsvPairLines);
            }

            Console.WriteLine($"Done. File can be found at: {outputFilePath}");
            return;

        }

        /// <summary>
        /// Parses an .md file into tab separated key value pairs based on header markers (#, ##)
        /// </summary>
        /// <param name="filePath">Path of .md file to parse</param>
        /// <param name="tabPairLines"></param>
        /// <param name="log"></param>
        private static void parseMdFileToTabLines(
            string filePath,
            IList<string> tabPairLines,
            bool addHeaderToContent)
        {
            Console.WriteLine($"Parsing {filePath}");
            using (StreamReader reader = File.OpenText(filePath))
            {
                string header = "";
                var contentSb = new StringBuilder();
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var lineTrimmed = line.Trim();

                    // check if line starts with a h1 or h2 header hash
                    // length check makes sure the header has a value so that a question key can be parsed
                    if (lineTrimmed.StartsWith("#", StringComparison.OrdinalIgnoreCase)
                        || lineTrimmed.StartsWith("##", StringComparison.OrdinalIgnoreCase))
                    {
                        // add the previous found pair to new lines
                        addTabPairToLines(header, contentSb, addHeaderToContent, tabPairLines);
                        header = lineTrimmed.TrimStart(' ', '#'); ;
                        contentSb.Clear();
                    }
                    else
                    {
                        // add line in between headers to string builder
                        contentSb.Append(@"<br/>" + lineTrimmed);
                    }
                }
                // add last found pair as new tsv line
                addTabPairToLines(header, contentSb, addHeaderToContent, tabPairLines);
            }
        }

        /// <summary>
        /// adds a header content pair as a new tab separated line
        /// </summary>
        /// <param name="header">header value</param>
        /// <param name="content">content below header</param>
        /// <param name="addHeaderToContent">prepend bolded header value to content</param>
        /// <param name="tablines">master tab lines list</param>
        private static void addTabPairToLines(
            string header,
            StringBuilder contentSb,
            bool addHeaderToContent,
            IList<string> tablines)
        {
            if (!string.IsNullOrWhiteSpace(header)
                  && contentSb.Length > 0)
            {
                if (addHeaderToContent)
                {
                    // Adds a bolded header (markdown) to content
                    contentSb.Insert(0, $@"**{header}**");
                }
                tablines.Add($"{header}\t{contentSb.ToString()}");
            }
        }
    }
    
}
