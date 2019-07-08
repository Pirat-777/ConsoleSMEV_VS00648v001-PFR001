using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSMEV_VS00648v001_PFR001
{
    class CurrentTime
    {
        public string Get()
        {
            return DateTime.Now.ToString("yyMMddHHmmssfff_");
        }
    }
}
