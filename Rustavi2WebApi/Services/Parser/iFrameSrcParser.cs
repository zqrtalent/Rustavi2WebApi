using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services.Parser
{
    using HtmlAgilityPack;

    internal class iFrameSrcParser : IHtmlParser<string>
    {
        public async Task<string> Parse(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            await Task.CompletedTask;

            var iframeNode = doc.DocumentNode.SelectSingleNode(@".//iframe");
            return iframeNode?.SrcAttribute() ?? string.Empty;
        }
    }
}
