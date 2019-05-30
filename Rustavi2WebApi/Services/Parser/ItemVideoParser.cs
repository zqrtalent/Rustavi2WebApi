using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services.Parser
{
    using HtmlAgilityPack;
    using System.Text.RegularExpressions;

    internal class ItemVideoParser : IHtmlParser<ItemVideoDetails>
    {
        public Regex _regSourceLookup = new Regex("src\\s*\\:\\s*\\\"([^\\\"]+)\\\"", RegexOptions.Compiled|RegexOptions.Multiline);
        public Regex _regTypeLookup = new Regex("type\\s*\\:\\s*\\\"([^\\\"]+)\\\"", RegexOptions.Compiled|RegexOptions.Multiline);

        public async Task<ItemVideoDetails> Parse(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            await Task.CompletedTask;

            ItemVideoDetails result = null; 
            var scriptNodes = doc.DocumentNode.SelectNodes(@".//script");
            if(scriptNodes?.Any() ?? false)
            {
                /*
                { type: "any-type", src: "any-url.m3u8" }
                */
                foreach(var scriptNode in scriptNodes)
                {
                    var matchResult = _regSourceLookup.Matches(scriptNode.InnerHtml);
                    if(matchResult.Count > 0 && matchResult[0].Groups.Count > 1)
                    {
                        var videoSrc = matchResult[0].Groups[1].Value.TrimStart().TrimEnd();
                        if(videoSrc.EndsWith(".m3u8"))
                        {
                            result = new ItemVideoDetails 
                            {
                                VideoUrl = videoSrc
                            };
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
