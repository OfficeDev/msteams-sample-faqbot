using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace FaqMdParser
{
    class ParserOptions
    {
        [Option('f', "file", Required = true, HelpText = "File path of md file to parse")]
        public string MdFilePath { get; set; }

        [Option('p', "output", Required = true, HelpText = "Output directory of md file to parse")]
        public string OutputDir { get; set; }

        [Option('n', "name", Required = true, HelpText = "Output file name (name only - ext will be .tsv)")]
        public string OutputFileName { get; set; }

        [Option('o', "overwrite", HelpText = "Overwrites the output file, if exists. Default behavior is to append to end of output file.")]
        public bool Overwrite { get; set; }

        [Option('h', "content-header", HelpText = "Appends bolded headers to the content strings per tab pair")]
        public bool AppendContentHeader { get; set; }

    }
}
