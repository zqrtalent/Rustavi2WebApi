using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services.Parser
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using HtmlAgilityPack;

    internal class ShowDetailParser : IHtmlParser<ShowItemDetail>
    {
        public const string _showPageUrl = @"http://rustavi2.ge/ka/shows$0";
        public const string _newsCoverImagePath = @"http://rustavi2.ge$0";
        public const string _showsSubAjaxUrl = "http://rustavi2.ge/includes/shows_sub_ajax.php";

        // load_videos('ka','3','');
        // load_videos('ka','3',5);
        public const string _regexLoadVideosParams = @"load_videos\(\s*\'([a-zA-Z0-9_ ]*)\s*\'\,\s*\'([a-zA-Z0-9_ ]*)\s*\'\s*\,\s*\'?([a-zA-Z0-9_ ]*)\s*\'?\s*\)";

        private readonly IHtmlParser<IEnumerable<ShowVideoItem>> _showVideosParser;

        public ShowDetailParser(IHtmlParser<IEnumerable<ShowVideoItem>> showVideosParser)
        {
            _showVideosParser = showVideosParser;
        }

        public async Task<ShowItemDetail> Parse(string htmlContent)
        {
            var result = new ShowItemDetail(); 
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            await Task.CompletedTask;

            var mainVideoNode = doc.DocumentNode.SelectSingleNode(@"//div[@class='vd']");
            if(mainVideoNode != null)
            {
                var mainVideoPLNode = mainVideoNode.SelectSingleNode(@"//div[@class='pl']");
                var mainVideoCoverUrlNode = mainVideoNode.SelectSingleNode(@"//div[@class='ph']");

                if(mainVideoCoverUrlNode != null)
                {
                    var mainVideoLink = mainVideoCoverUrlNode.SelectSingleNode(@"//a[@class='link']");

                    var mainVideo = new ShowVideoItem();
                    mainVideo.VideoPageUrl =  _newsCoverImagePath.Replace("$0", mainVideoLink.HrefAttribute());
                    mainVideo.Id = WebClientService.ExtractIdFromUrl(mainVideo.VideoPageUrl);
                    mainVideo.Title = mainVideoPLNode.SelectSingleNode(@"//div[@class='txt rioni']")?.InnerHtml ?? string.Empty;
                    mainVideo.CoverImageUrl = _newsCoverImagePath.Replace("$0", mainVideoCoverUrlNode.BackgroundImageUrl());
                    result.MainVideo = mainVideo;
                }

                // Show description.
                result.Desc = mainVideoNode.SelectSingleNode(@"//div[@class='r_txt rioni black']")?.InnerHtml ?? string.Empty;
            }

            var dicVideoSectionUrlByName = ParseVideoSections(doc.DocumentNode);
            if(dicVideoSectionUrlByName?.Any() ?? false)
            {
                var sectionVideoItems = new List<SectionVideoItems>();
                // Load videos by sections.
                foreach(var pair in dicVideoSectionUrlByName)
                {
                    var videoItems = await WebClientService.HttpGet(pair.Value, async (string html) => 
                    {
                        return await _showVideosParser.Parse(html);
                    });

                    if(videoItems?.Any() ?? false)
                    {
                        sectionVideoItems.Add(new SectionVideoItems
                        {
                            Section = pair.Key,
                            VideoItems = videoItems
                        });
                    }
                    
                    result.SectionVideoItems = sectionVideoItems;
                }
            }

            return result;
        }

        private IDictionary<string, string> ParseVideoSections(HtmlNode node)
        {
            var matchLoadVideoParams = new System.Text.RegularExpressions.Regex(_regexLoadVideosParams, RegexOptions.Compiled);
            var dicResult = new Dictionary<string, string>();
            var sectionHeaderElems = node.SelectNodes(@"//div[@class='video_hed nino blue']");
            if(sectionHeaderElems?.Any() ?? false)
            {
                foreach(var headerElem in sectionHeaderElems)
                {
                    var scriptElem = headerElem.NextSibling;
                    while(scriptElem != null && scriptElem.Name != "script")
                    {
                        scriptElem = scriptElem.NextSibling;
                    }

                    if(scriptElem != null)
                    {
                        // load_videos('ka','3','');                        
                        var matchResult = matchLoadVideoParams.Match(scriptElem.InnerHtml);
                        if(matchResult.Groups.Count == 4)
                        {
                            var lang = matchResult.Groups[1].Value;
                            var id = matchResult.Groups[2].Value;
                            var mode = matchResult.Groups[3].Value;

                            var sectionName = headerElem.InnerHtml.Replace("&nbsp", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                            dicResult[sectionName] = $"{_showsSubAjaxUrl}?l={lang}&id={id}&pos=0&mode={mode}&_=1543780631518";
                        }
                    }
                }
            }

            return dicResult;
        }
    }
}
