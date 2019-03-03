using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rustavi2WebApi.Models.Services
{
    public class ShowItemDetail
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string PageUrl { get; set; }
        public ShowVideoItem MainVideo { get; set; }
        public IDictionary<string, IEnumerable<ShowVideoItem>> VideoItemsBySection { get; set; }
    }
}
