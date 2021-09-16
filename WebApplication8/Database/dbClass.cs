using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace WebApplication8.Database
{
    class dbClass
    {
        internal const string _key = "P60dD3v";

        private IConfiguration configuration;

        internal string cost;

        public dbClass()
        {
            string UserID = configuration.GetSection("Database").GetSection("Connections").GetSection("UserID").Value;
            string Database = configuration.GetSection("Database").GetSection("Connections").GetSection("Database").Value;
            string Host = configuration.GetSection("Database").GetSection("Connections").GetSection("Host").Value;
            string port = configuration.GetSection("Database").GetSection("Connections").GetSection("port").Value;
            string pawo = configuration.GetSection("Database").GetSection("Connections").GetSection("password").Value;


            Cryptography crypto = new Cryptography()
            {
                PassPhrase = _key,
                SaltValue = _key + _key
            };

            string password = crypto.DecryptString(pawo);

            cost = string.Format(@" datasource={0}; database={1}; port={2}; username={3}; password={4}; sslmode=none; ", Host, Database, port, UserID, password);
         
        }

        public dbClass(IConfiguration Iconfig)
        {
            configuration = Iconfig;

            string UserID = configuration.GetSection("Database").GetSection("Connections").GetSection("UserID").Value;
            string Database = configuration.GetSection("Database").GetSection("Connections").GetSection("Database").Value;
            string Host = configuration.GetSection("Database").GetSection("Connections").GetSection("Host").Value;
            string port = configuration.GetSection("Database").GetSection("Connections").GetSection("port").Value;
            string pawo = configuration.GetSection("Database").GetSection("Connections").GetSection("password").Value;

            Cryptography crypto = new Cryptography()
            {
                PassPhrase = _key,
                SaltValue = _key + _key
            };

            string password = crypto.DecryptString(pawo);



           // string name = string.Format(@"name:", );

            cost = string.Format(@" datasource={0}; database={1}; port={2}; username={3}; password={4}; sslmode=none; ", Host, Database, port, UserID, password);

        }



        //public static HttpResponseMessage CreateResponse<T>(this HttpRequestMessage requestMessage, HttpStatusCode statusCode, T content)
        //{ 
        //    return new HttpResponseMessage() 
        //    { StatusCode = statusCode, Content = new StringContent(JsonConvert.SerializeObject(content)) 
        //    }; 
        //}

        public void Getdata(out string uname, out string pwd)
        {

            Console.WriteLine("I am inside getData");
            uname = string.Empty;
            pwd = string.Empty;
            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(cost))
            {
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand())
                    {
                        //command.CommandText = "SELECT * FROM services WHERE auto_id='" + auto_id + "' AND (service in ('accessibility', 'mathtype to image', 'alttext') AND (status>=10 AND status<30)) ORDER BY priority DESC, ping; ";
                        // command.CommandText = "SELECT username,password FROM users WHERE username='Aswin' and password='Aswinb@08'";
                        command.CommandText = $"SELECT username,password FROM users WHERE username= {uname} and password={pwd}";  //by Abhay
                        command.Connection = connection;
                        using (MySql.Data.MySqlClient.MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                try
                                {
                                    uname = Convert.ToString(reader["username"]);
                                    pwd = Convert.ToString(reader["password"]);
                                }
                                catch
                                { }

                            }
                        }



                    };
                }
                catch
                {

                }
            }

            //string UserID = configuration.GetSection("Database").GetSection("Connections").GetSection("UserID").Value;
            //string Database = configuration.GetSection("Logging").GetSection("Connections").GetSection("Database").Value;
            //string Host = configuration.GetSection("Database").GetSection("Connections").GetSection("Host").Value;
            //string port = configuration.GetSection("Database").GetSection("Connections").GetSection("port").Value;
            //string pawo = configuration.GetSection("Logging").GetSection("Connections").GetSection("password").Value;

            //Cryptography crypto = new Cryptography()
            //{
            //    PassPhrase = _key,
            //    SaltValue = _key + _key
            //};

            //string password = crypto.DecryptString(pawo);

        }


    }
}
