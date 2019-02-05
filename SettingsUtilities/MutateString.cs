using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsUtilities
{
    public static class MutateString
    {
        /// <summary>
        /// Converts a string array of json objects into a string which can be imported into postgress as a jsonb array.
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string ConvertToJsonbString(string[] stringArray)
        {
            var sb = new StringBuilder();
            sb.Append("array[");

            int count = stringArray.Length;
            for (int i = 0; i < count; i++)
            {
                sb.Append($"'{stringArray[i]}'");
                if (i != (count - 1)) sb.Append(",");
            }
            sb.Append("]::json[]");

            return sb.ToString();
        }
    }
}
