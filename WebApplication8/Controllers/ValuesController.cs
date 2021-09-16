using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Database;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new String[] { " Tony", " Mark42" };
        }


        [HttpGet("{dataFromApi}")]
        public async Task<ModelClass> GetData(string dataFromApi)
        {

            sqlDatabase sql = new sqlDatabase();
            ModelClass model = new ModelClass();
            Console.WriteLine("Inside second Api");
            model = await sql.getBookInfo(dataFromApi);
            return model;
        }

      /*  [HttpGet("{id}")]
        public string get(int id)
        {
            return "Is this what you've been sending me ? " + id ;
        }*/
        

        [HttpPost]
        public async Task<CallBackPost> PostData( CallBackPost callBackPost)
        {
            sqlDatabase sql = new sqlDatabase();
            CallBackPost cbp;
           cbp = await sql.postData(callBackPost);
            return cbp ;
        }
        
    }
}
