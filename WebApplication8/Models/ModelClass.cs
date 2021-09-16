using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class ModelClass
    {
        public string username { get; set; }
        public string password { get; set; }
        public string bookname { get; set; }
        public string pages { get; set; }
        public string author { get; set; }
        public string releaseDate { get; set; }
    }

    public class callback
    {
        public  string username { get; set; }
        public string Uri { get; set; }
    }

    public class CallBackPost
    {
        public string username { get; set; }
        public string password { get; set; }
        public string Uri { get; set; }
        public string bookname { get; set; }
        public string pages { get; set; }
        public string author { get; set; }
        public string releaseDate { get; set; }
        
    }

    public class UserLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
