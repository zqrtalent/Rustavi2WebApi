using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services
{
    public interface IHtmlParser<T> where T : class
    {
        Task<T> Parse(string htmlContent);
    }
}
