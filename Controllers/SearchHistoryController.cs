using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoutubeAPI.Databse;
using System.Linq;
using Newtonsoft.Json;

namespace YoutubeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchHistoryController : ControllerBase
    {
        //private readonly MusiNowDBContext _context;

        //public SearchHistoryController(MusiNowDBContext context)
        //{
        //    _context = context;
        //}

        //[HttpGet]
        //public async Task<string> Get()
        //{
        //    var data = _context.SearchHistories.ToList();

        //    return JsonConvert.SerializeObject(data);
        //}
    }
}
