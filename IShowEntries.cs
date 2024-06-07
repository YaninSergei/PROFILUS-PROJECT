using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PROFILUS.Entry;
using static PROFILUS.ConsoleShowData;
using PROFILUS;

namespace PROFILUS
{
    interface IShowEntries
    {
        void ShowEntries(List<Entry> prientries);
    }

}
