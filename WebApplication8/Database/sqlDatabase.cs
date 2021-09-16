using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication8.Models;
using Microsoft.AspNetCore.Http;

namespace WebApplication8.Database
{
    public class sqlDatabase
    {
        private string _connectionString;
        SqlConnection _conn;
        SqlConnection _connection;
        SqlDataReader reader;

        public sqlDatabase() {
               _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DemoDB;Integrated Security=True ";
                using (_conn = new SqlConnection(_connectionString)) {
                _conn.Open();
            }
           
        }

             public void connection()
             {
                 _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DemoDB;Integrated Security=True ";
                 using ( _connection = new SqlConnection(_connectionString))
                 {
                     try
                     {
                         _connection.Open();
                         

                     }
                     catch(SqlException se)
                     {
                         Console.WriteLine(se); 
                     }
                 }  
             }

        public async Task<CallBackPost> signUpUserEncrypted(CallBackPost userLogin) {
            Cryptography cryptography = new Cryptography();
            string encryptedData;
            userLogin.password = cryptography.EncryptString(userLogin.password);
            var response =  postData(userLogin);

            return response.Result;
        }

        public async Task<ModelClass> getBookInfo( string dataFromApi)
        {
            ModelClass model = new ModelClass();
            SqlCommand sqlCommand = new SqlCommand();
            _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DemoDB;Integrated Security=True ";
            using(_connection = new SqlConnection(_connectionString))
            {
                if (_connection.State == System.Data.ConnectionState.Closed) {
                    try
                    {
                        _connection.Open();
                        sqlCommand.CommandText = $"SELECT * FROM [dbo].[User] WHERE username = '{dataFromApi}' ";
                        sqlCommand.Connection = _connection;
                        using (reader = sqlCommand.ExecuteReader()) {
                            if (reader.Read())
                            {
                                model.username = Convert.ToString(reader["username"]);
                               // model.username = Convert.ToString(reader["password"]);
                                model.bookname = Convert.ToString(reader["book"]);
                                model.pages = Convert.ToString(reader["pages"]);
                                model.author = Convert.ToString(reader["author"]);
                                model.releaseDate= Convert.ToString(reader["releaseData"]);
                                return model;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                } else { return null; }
            }

            return null;
        }
        public async Task<CallBackPost> getBookInfoPost(string dataFromApi)
        {
            CallBackPost model = new CallBackPost();
            SqlCommand sqlCommand = new SqlCommand();
            _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DemoDB;Integrated Security=True ";
            using (_connection = new SqlConnection(_connectionString))
            {
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        _connection.Open();
                        sqlCommand.CommandText = $"SELECT * FROM [dbo].[User] WHERE username = '{dataFromApi}' ";
                        sqlCommand.Connection = _connection;
                        using (reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                model.username = Convert.ToString(reader["username"]);
                                // model.username = Convert.ToString(reader["password"]);
                                model.bookname = Convert.ToString(reader["book"]);
                                model.pages = Convert.ToString(reader["pages"]);
                                model.author = Convert.ToString(reader["author"]);
                                model.releaseDate = Convert.ToString(reader["releaseData"]);
                                return model;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else { return null; }
            }

            return null;
        }

        public async Task<CallBackPost> signInUserEncrypted(CallBackPost callBackPost)
        {
            CallBackPost model = new CallBackPost();
            model = await getData(callBackPost);
            return model;
        }
        public async Task<CallBackPost>getData(CallBackPost data)
        {
            Cryptography c = new Cryptography();
            string dUsername ;
            string dPassword ;
            string author;
            string pages;
            string encryptedPassword = c.EncryptString(data.password);
            SqlCommand sqlCommand = new SqlCommand();
            _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DemoDB;Integrated Security=True ";
            using (_connection = new SqlConnection(_connectionString))
            {
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        _connection.Open();
                        sqlCommand.CommandText = $"SELECT * FROM [dbo].[User] WHERE username = '{data.username}'  ";
                        sqlCommand.Connection = _connection;
                        using (reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                try
                                {
                                    dUsername = Convert.ToString(reader["username"]);
                                    dPassword = Convert.ToString(reader["password"]);

                                    if (dUsername == data.username && dPassword == encryptedPassword)
                                    {
                                        data.author = Convert.ToString(reader["author"]);
                                        data.bookname= Convert.ToString(reader["book"]);
                                        data.pages = Convert.ToString(reader["pages"]);
                                        return data;
                                    }

                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                return null;

                            }
                            else
                            {
                                return null;
                            }

                        }
                    }
                    catch (Exception )
                    {
                        throw;
                    }
                }
                else
                {
                    return null;
                }
            }


        }

        public async Task<CallBackPost> postData(CallBackPost callBackPost)
        {
            string dUsername = callBackPost.username;
            string author = callBackPost.author;
            string pwd = callBackPost.password;
            string pages = callBackPost.pages;
            string releaseDate = callBackPost.releaseDate;
            string bookName = callBackPost.bookname;


            SqlCommand sqlCommand = new SqlCommand();

            _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DemoDB;Integrated Security=True ";
            using (_connection = new SqlConnection(_connectionString))
            {
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                    sqlCommand.CommandText = $"INSERT into [dbo].[User] (username, password ,book, pages,author) " +
                        $"VALUES ('{dUsername}','{pwd}','{bookName}','{pages}', '{author}' )";
                    sqlCommand.Connection = _connection;
                    try
                    {
                        using (reader = sqlCommand.ExecuteReader())
                        {
                            return await getBookInfoPost(dUsername);
                        }

                    }
                    catch { throw; }
                }
                else
                {
                    return null;
                }
            }
        }



        public string putData(ref string userName, ref string pwd)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = $"SELECT * FROM [dbo].[User] WHERE username = 'Abhay' and password= 'abhay123' ";
            sqlCommand.Connection = _conn;
            try
            {
                using (reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToString(reader["book"]);
                    }
                }
            }
            catch
            {

            }
            return "created";
        }
       
    }
}
    