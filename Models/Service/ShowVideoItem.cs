using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rustavi2WebApi.Models.Services
{
    public class ShowVideoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string VideoPageUrl { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
