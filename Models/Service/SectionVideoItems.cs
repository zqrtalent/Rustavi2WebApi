using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rustavi2WebApi.Models.Services
{
    public class SectionVideoItems
    {
        public string Section { get; set; }
        public IEnumerable<ShowVideoItem> VideoItems { get; set; }
    }
}
