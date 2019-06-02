using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using rustavi2WebApi.Extensions;
using rustavi2WebApi.Models.Services;
using rustavi2WebApi.Settings;

namespace rustavi2WebApi.Services
{
    internal class ShowsService : IShowsService
    {
        private readonly string _urlShows = "http://rustavi2.ge/ka/shows";
        private readonly string _urlVideoPage = "http://rustavi2.ge/ka/video/{0}?v=2";
        private readonly IHtmlParser<IEnumerable<ShowItem>> _showsParser;
        private readonly IHtmlParser<ShowItemDetail> _showDetailParser;
        private readonly IHtmlParser<string> _iframeSrcParser;
        private readonly IHtmlParser<ItemVideoDetails> _itemVideoParser;
        private readonly IOptionsMonitor<UrlReplaceSettings> _urlReplaceSettings;

        public ShowsService(
            IHtmlParser<IEnumerable<ShowItem>> showsParser, 
            IHtmlParser<ShowItemDetail> showDetailParser,
            IHtmlParser<string> iframeSrcParser, 
            IHtmlParser<ItemVideoDetails> itemVideoParser,
            IOptionsMonitor<UrlReplaceSettings> urlReplaceSettings)
        {
            _showsParser = showsParser;
            _showDetailParser = showDetailParser;
            _iframeSrcParser = iframeSrcParser;
            _itemVideoParser = itemVideoParser;
            _urlReplaceSettings = urlReplaceSettings;
        }

        public async Task<IEnumerable<ShowItem>> GetShows()
        {
            var listShows = await WebClientService.HttpGet(_urlShows, async (string html) => 
            {
                return await _showsParser.Parse(html);
            });

            listShows = listShows?.Select(x => {
                x.CoverImageUrl = _urlReplaceSettings.CurrentValue.ImageUrlHostReplace.ReplaceAllTheOccurences(x.CoverImageUrl);
                x.PageUrl = _urlReplaceSettings.CurrentValue.ImageUrlHostReplace.ReplaceAllTheOccurences(x.PageUrl);
                return x;
            })?.ToList();

            return listShows;
        }

        public async Task<ShowItemDetail> GetShowDetail(string id)
        {
            var result = await WebClientService.HttpGet($"{_urlShows}/{id}", async (string html) => 
            {
                return await _showDetailParser.Parse(html);
            });

            if(result != null)
            {
                result.Id = id;

                if(result.MainVideo != null)
                {
                    result.MainVideo.CoverImageUrl = _urlReplaceSettings.CurrentValue.ImageUrlHostReplace.ReplaceAllTheOccurences(result.MainVideo.CoverImageUrl);
                    result.MainVideo.VideoPageUrl = _urlReplaceSettings.CurrentValue.ImageUrlHostReplace.ReplaceAllTheOccurences(result.MainVideo.VideoPageUrl);
                }

                if(result.SectionVideoItems?.Any() ?? false)
                {
                    foreach(var pair in result.SectionVideoItems)
                    {
                        if(pair.VideoItems?.Any() ?? false)
                        {
                            foreach(var item in pair.VideoItems)
                            {
                                item.CoverImageUrl = _urlReplaceSettings.CurrentValue.ImageUrlHostReplace.ReplaceAllTheOccurences(item.CoverImageUrl);
                                item.VideoPageUrl = _urlReplaceSettings.CurrentValue.ImageUrlHostReplace.ReplaceAllTheOccurences(item.VideoPageUrl);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<ItemVideoDetails> GetShowVideoDetail(string name, string videoId)
        {
            var iframeSrcUrl = await WebClientService.HttpGet(string.Format(_urlVideoPage, videoId), async (string html) => 
            {
                return await _iframeSrcParser.Parse(html);
            });

            if(string.IsNullOrEmpty(iframeSrcUrl))
            {
                return null;
            }

            var videoDetails = await WebClientService.HttpGet(iframeSrcUrl, async (string html) => 
            {
                return await _itemVideoParser.Parse(html);
            });
            
            if(videoDetails != null)
            {
                videoDetails.Id = videoId;
                videoDetails.VideoType = ItemVideoType.NewsVideo;
                videoDetails.VideoUrl = _urlReplaceSettings.CurrentValue.HlsUrlHostReplace.ReplaceAllTheOccurences(videoDetails.VideoUrl);
            }

            return videoDetails;
        }
    }
}
