using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTree
{
    public class CustomCultureInfo
    {
        static CultureInfo CustomCulture;

        public static void SetupCustomCultureInfo()
        {
            CustomCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            CustomCulture.NumberFormat.NumberDecimalSeparator = ".";    
        }

        public static CultureInfo GetCustomCultureInfo()
        {
            return CustomCulture;
        }
    }
}
