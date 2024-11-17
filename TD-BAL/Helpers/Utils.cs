using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_BAL.Helpers
{
    public static class Utils
    {

        public static string FormatToRupiah(double? amount)
        {
            if (amount.HasValue)
            {
                return "Rp " + amount.Value.ToString("#,##0", CultureInfo.InvariantCulture);
            }

            return "Rp 0";
        }
    }
}
