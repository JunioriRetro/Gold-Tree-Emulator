using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTree
{
    public class CustomCulture
    {
        public static CultureInfo GetCustomCultureInfo()
        {
            CultureInfo customCulture = CultureInfo.InvariantCulture;   
            customCulture.NumberFormat.NumberDecimalSeparator = ".";    

            return customCulture;
        }
    }
}
