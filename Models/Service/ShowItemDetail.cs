using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rustavi2WebApi.Models.Services
{
    public class ShowItemDetail
    {
        public string Id { get; set; }
        public string Desc { get; set; }
        public ShowVideoItem MainVideo { get; set; }
        public IEnumerable<SectionVideoItems> SectionVideoItems { get; set; }
    }
}
