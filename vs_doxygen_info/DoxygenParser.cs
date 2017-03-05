using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_doxygen_info
{
    /// <summary>
    /// This will parse a string into a mapping of known doxygen commands and their contents.
    /// </summary>
    class DoxygenParser
    {
        private Dictionary<string, IList<string>> m_dictionary;

        /// <summary>
        /// These are the known commands we'll handle
        /// </summary>
        private const string commands = "[@|\\\\](brief|param|returns)";

        public DoxygenParser(string raw_string)
        {
            List<string> local_list = new List<string>();
            local_list.Add(raw_string);

            m_dictionary = new Dictionary<string, IList<string>>();
            m_dictionary.Add("raw", local_list);

            // The signature is always the first line.
            char[] delimiters = { '\n' };
            string [] signature = raw_string.Split(delimiters);
            local_list = new List<string>();
            local_list.Add(signature[0]);
            m_dictionary.Add("signature", local_list);
        }

        /// <summary>
        /// This will parse the string into the specific doxygen blocks
        /// </summary>
        /// <returns>The dictionary of all of the string contents parsed</returns>
        /// 
        /// From http://regexstorm.net/tester?p=%5b%5c%5c%40%5d%28brief%7cparam%7creturns%29%28.*%3f%29%28%3f%3d%28%5b%5c%5c%40%5d%28brief%7cparam%7creturns%29%29%7c%28%5cn%5cn%29%7c%24%29&i=%5cbrief+hello+this+is+my+function%0d%0athis+is+more+about+my+function%0d%0aand+some+more+text%0d%0a%0d%0a%0d%0a%0d%0a%5cparam+first+my+first+param%0d%0a%5cparam+second+this+is+the+second+param&o=s
        /// wow that is a crazy url.  
        /// 
        /// Here is the magic regex.
        /// [\\@](brief|param|returns)(.*?)(?=([\\@](brief|param|returns))|(\n\n)|$)
        /// First match a doxygen command such as "\brief" 
        /// then .*? is an ungreedy match of all characters, note this needs to be done in single line mode to grab newlines.
        /// Then do a look ahead to see if we found another doxygen command, a blank line, or the end of the string.
        public Dictionary<string, IList<string>> Parse()
        {
            string pattern = commands + "(.*?)(?=" + commands + "|(\\n\\n)|$)";
            Regex command_regex = new Regex(pattern, RegexOptions.Singleline);

            IList<string> raw;
            m_dictionary.TryGetValue("raw", out raw);
            Match m = command_regex.Match(raw[0]);
            while(m.Success)
            {
                IList<string> contents;
                string command = m.Groups[1].Value;
                m_dictionary.TryGetValue(command, out contents);
                if (contents == null)
                {
                    contents = new List<string>();
                }
                else
                {
                    m_dictionary.Remove(command);
                }
                contents.Add(m.Groups[2].Value);
                m_dictionary.Add(command, contents);

                m = m.NextMatch();
            }
            return m_dictionary;
        }
    }
}
