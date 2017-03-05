using System;
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
        const private string commands = "[@|\\](brief|param|returns)";
        public DoxygenParser(string raw_string)
        {
            List<string> local_list;
            local_list.Add(raw_string);
            m_dictionary.Add("raw", local_list);
        }

        /// <summary>
        /// This will parse the string into the specific doxygen blocks
        /// </summary>
        /// <returns>The dictionary of all of the string contents parsed</returns>
        public Dictionary<string, IList<string>> Parse()
        {
            return m_dictionary;
        }
    }
}
