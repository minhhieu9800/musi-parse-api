using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using YoutubeAPI.Databse;
using YoutubeAPI.Models;

namespace YoutubeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly string KEY;

        private readonly MusiNowDBContext _context;
        private readonly IConfiguration configuration;
        public static IWebHostEnvironment environment;

        public UserDataController(IConfiguration configuration, MusiNowDBContext context, IWebHostEnvironment _environment)
        {
            this.configuration = configuration;
            this._context = context;
            environment = _environment;

            KEY = this.configuration.GetConnectionString("KEY");
        }

        [HttpGet]
        public async Task<string> Get([FromHeader]string key, string email)
        {
            if (KEY == key)
            {
                var data = _context.UserData.Where(x => x.Email == email).FirstOrDefault();

                // return null if new user
                return JsonConvert.SerializeObject(data);
            }

            return "wrong key";
        }

        [HttpPost]
        public async Task<string> Post([FromHeader] string key, UserDatum userData)
        {
            if (KEY == key)
            {
                var data = _context.UserData.Where(x => x.Email == userData.Email).FirstOrDefault();

                // if user exists
                if (data != null)
                {
                    // update new data
                    data.DataBackUp = userData.DataBackUp;

                    data.LastBackUp = userData.LastBackUp;

                    data.DeviceBackUp = userData.DeviceBackUp;

                    _context.SaveChanges();

                    return "1";
                }
                else
                // if new user
                {
                    _context.UserData.Add(new UserDatum()
                    {
                        Email = userData.Email,
                        DeviceBackUp = userData.DeviceBackUp,
                        LastBackUp = userData.LastBackUp,
                        DataBackUp = userData.DataBackUp,
                    });

                    _context.SaveChanges();

                    return "0";
                }


            }

            return "wrong key";
        }


        [HttpGet("newapp")]
        public async Task<bool> GetNewApp([FromHeader] string key)
        {
            if (KEY == key)
            {
                var data = this.configuration.GetConnectionString("NEW_APP");

                // return null if new user
                try
                {
                    return bool.Parse(data);
                } catch (Exception)
                {
                    return false;
                }
               
            }

            return false;
        }
    }
}
