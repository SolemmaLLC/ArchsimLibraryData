using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchsimLib
{
    public static class Logger
    {
        public static StringBuilder log = new StringBuilder();

        public static void WriteLine(string s)
        {
            log.AppendLine(s);
        }

    }
}
