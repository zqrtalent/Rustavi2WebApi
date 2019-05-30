using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace rustavi2WebApi.DI
{
    using rustavi2WebApi.Models.Services;
    using rustavi2WebApi.Services;
    using rustavi2WebApi.Services.Parser;

    public static class DiConfig
    {
        public static IServiceCollection RegisterWebAppServices(this IServiceCollection collection)
        {
            collection.AddTransient<INewsService, NewsService>();
            collection.AddTransient<IShowsService, ShowsService>();

            // Parsers
            collection.AddTransient<IHtmlParser<IEnumerable<NewsItem>>, NewsArchiveParser>();
            collection.AddTransient<IHtmlParser<NewsItemDetail>, NewsDetailParser>();
            collection.AddTransient<IHtmlParser<IEnumerable<ShowItem>>, ShowsParser>();
            collection.AddTransient<IHtmlParser<ShowItemDetail>, ShowDetailParser>();
            collection.AddTransient<IHtmlParser<IEnumerable<ShowVideoItem>>, ShowVideosParser>();
            collection.AddTransient<IHtmlParser<string>, iFrameSrcParser>();
            collection.AddTransient<IHtmlParser<ItemVideoDetails>, ItemVideoParser>();

            return collection;
        }
    }
}
