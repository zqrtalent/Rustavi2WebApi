using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rustavi2WebApi.Models.Services
{
    public class NewsItemDetail
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public string StoryDetail { get; set; }
        public string VideoUrl { get; set; }
    }
}
