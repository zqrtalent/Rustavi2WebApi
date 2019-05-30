using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace rustavi2WebApi.Models.Services
{
    public enum ItemVideoType
    {
        NewsVideo,
        ShowVideo
    }

    public class ItemVideoDetails
    {
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemVideoType VideoType {get; set;}
        
        public string VideoUrl { get; set; }
    }
}
