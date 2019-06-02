using System;
using System.Collections.Generic;
using System.Linq;
using rustavi2WebApi.Settings;

namespace rustavi2WebApi.Extensions
{
    public static class FindAndReplaceExtensions
    {
        public static string ReplaceAllTheOccurences(this List<FindAndReplace> dict, string replace)
        {
            if(string.IsNullOrEmpty(replace) || !(dict?.Any() ?? false))
                return replace;

            foreach(var pair in dict)
            {
                replace = replace.Replace(pair.Find, pair.Replace);
            }
            return replace;
        }
    }
}