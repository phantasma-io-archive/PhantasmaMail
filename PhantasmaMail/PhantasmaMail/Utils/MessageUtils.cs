using System;
using System.Collections.Generic;
using System.Text;

namespace PhantasmaMail.Utils
{
    public static class MessageUtils
    {
        public static string CalculateDays(DateTime d)
        {
            // 1.
            // Get time span elapsed since the date.
            var s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            var dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            var secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31) return null;

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60) return "just now";
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120) return "1 minute ago";
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                    return $"{Math.Floor((double) secDiff / 60)} minutes ago";
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200) return "1 hour ago";
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                    return $"{Math.Floor((double) secDiff / 3600)} hours ago";
            }

            // 6.
            // Handle previous days.
            if (dayDiff == 1) return "yesterday";
            if (dayDiff < 7)
                return $"{dayDiff} days ago";
            if (dayDiff < 31)
                return $"{Math.Ceiling((double) dayDiff / 7)} weeks ago";
            return null;
        }

        public static bool ValidateBoxName(string boxName)
        {
            int index = 0;
            var boxNameBytes = Encoding.ASCII.GetBytes(boxName);
            while (index < boxNameBytes.Length)
            {
                var c = boxNameBytes[index];
                index++;

                if (c >= 97 && c <= 122) continue; // lowercase allowed
                if (c == 95) continue; // underscore allowed
                if (c >= 48 && c <= 57) continue; // numbers allowed

                return false;
            }

            return true;
        }

        public static bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;
            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }

    }
}
