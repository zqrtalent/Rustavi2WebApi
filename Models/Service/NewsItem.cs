using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rustavi2WebApi.Models.Services
{
    public class NewsItem
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
    }
}