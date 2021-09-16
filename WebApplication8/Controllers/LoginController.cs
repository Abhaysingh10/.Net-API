using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Database;
using Microsoft.Extensions.Configuration;
using WebApplication8.Models;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using static WebApplication8.Cryptography;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration configuration;

        public LoginController(IConfiguration Iconfig)
        {
            configuration = Iconfig;
        }


        [HttpGet]
        public async Task<IActionResult> GetAsync(callback callback)
        {

            var client = new HttpClient();
            /*sqlDatabase sql = new sqlDatabase();
            ModelClass model = new ModelClass();*/
            string var = callback.Uri  + callback.username;
            Console.WriteLine(var);
            using (HttpResponseMessage response = await client.GetAsync(var)) {

                var responseContent = response.Content.ReadAsStringAsync().Result;
                response.EnsureSuccessStatusCode();
                return Ok(responseContent);

            }

        }

        [HttpPost("post")]
        public async Task<IActionResult> PostAsync(CallBackPost callBackPost)
        {

            var client = new HttpClient();
            //client.BaseAddress = new Uri(callBackPost.Uri);
            var postData = new
            {
                username = callBackPost.username,
                password = callBackPost.password,
                bookname = callBackPost.bookname,
                author = callBackPost.author,
                pages = callBackPost.pages,
                releaseDate = callBackPost.releaseDate
            };
            var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
            //StringContent httpConent = new StringContent(callBackPost.xmlRequest, Encoding.UTF8);
            //var res = client.PostAsync(callBackPost.Uri, httpConent );
            
            using (HttpResponseMessage response = await client.PostAsync(callBackPost.Uri, content))
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                response.EnsureSuccessStatusCode();
                return Ok(responseContent);
            }

        }

        [HttpGet("callme")]
        public async Task<ActionResult> Get(ModelClass modelClass)
        {
            ModelClass model = new ModelClass();
            model = modelClass;
            String userName = model.username;
            String pwd = model.password;
            sqlDatabase sqlDatabase = new sqlDatabase();
            Console.WriteLine("Inside of get Method");

            // sqlDatabase.connection();
            // Task.Delay(3000);
            /*string bookName =await sqlDatabase.GetData( );
             if (bookName == null)
             {
                 return NotFound();
             }
             else
             {
                 return Ok(bookName.ToString());

             }*/

            return Ok();
        }

        [HttpPost("usersignup")]
        public async Task<IActionResult> Post(CallBackPost user)
        {
            sqlDatabase sqlDatabase = new sqlDatabase();
            try
            {
                var response = await sqlDatabase.signUpUserEncrypted(user);
                if (response != null)
                {
                    return Ok();
                }


                else
                {
                    return Ok();
                }

            }
            catch (Exception)
            {
                throw;
            }


        }


        [HttpGet("userlogin")]
        public async Task<IActionResult> Get(CallBackPost user)
        {
            sqlDatabase sqlDatabase = new sqlDatabase();
            try
            {
                var response = await sqlDatabase.signInUserEncrypted(user);
                if (response != null)
                {
                    return Ok(response);
                }
                else{
                    return Ok("User does not exist");
                }

            }
            catch (Exception)
            {
                throw;
            }
            

        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(ModelClass modelClass)
        {
            ModelClass model = new ModelClass();
            string bookName = null;
            model = modelClass;
            String userName = model.username;
            String pwd = model.password;
            string book = model.bookname;
            sqlDatabase sqlDatabase = new sqlDatabase();
            //  sqlDatabase.connection();
          //  bookName = await sqlDatabase.postData( userName, pwd, book);
            if (bookName == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            } else
            {
                return Ok(bookName.ToString());

            }

        }

        [HttpPut]
        public ActionResult Put(ModelClass modelClass)
        {
            ModelClass model = new ModelClass();
            model = modelClass;
            String userName = model.username;
            String pwd = model.password;
            sqlDatabase sqlDatabase = new sqlDatabase();
            //  sqlDatabase.connection();
            string bookName = sqlDatabase.putData(ref userName, ref pwd);
            if (bookName == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return Ok(bookName.ToString());

            }


        }
        
    
    }
}