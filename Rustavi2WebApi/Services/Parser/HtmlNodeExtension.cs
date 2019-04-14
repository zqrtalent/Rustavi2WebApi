using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace rustavi2WebApi.Services.Parser
{
    internal static class HtmlNodeExtension
    {
        public static string BackgroundImageUrl(this HtmlNode node)
        {
            var style = node?.Attributes?.SingleOrDefault(x => x.Name == "style")?.Value ?? string.Empty;
            if(style.StartsWith("background-image:url("))
            {
                return style.Substring("background-image:url(".Length).TrimEnd(')');
            }
            return string.Empty;
        }

        public static string HrefAttribute(this HtmlNode node)
        {
            return node?.Attributes?.SingleOrDefault(x => x.Name == "href")?.Value ?? string.Empty; 
        }
    }
}
