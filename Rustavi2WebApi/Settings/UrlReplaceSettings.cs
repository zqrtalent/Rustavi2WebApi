using System;
using System.Collections.Generic;

namespace rustavi2WebApi.Settings
{
    public class FindAndReplace
    {
        public string Find {get; set;}
        public string Replace {get; set;}
    }

    public class UrlReplaceSettings
    {
        public UrlReplaceSettings()
        {
        }

        public List<FindAndReplace> ImageUrlHostReplace { get; set; }
        public List<FindAndReplace> HlsUrlHostReplace {get; set;}
    }
}