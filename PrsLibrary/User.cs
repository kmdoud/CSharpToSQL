using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToSQL
{
    public class User
    {
        private static string CONN_STRING = @"server=DSI-WORKSTATION\SQLEXPRESS;database=PrsDb;trusted_connection=true;"; 

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }

        private static SqlConnection CreateAndCheckConnection()
        {
            var Connection = new SqlConnection(CONN_STRING);
            Connection.Open();
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connection FAILED");
                return null;
            }
            return Connection;
        }

        public static bool UpdateUser(User user)
        {
            var Connection = CreateAndCheckConnection();
            if(Connection == null)
            {
                return false;
            }
            var isReviewer = user.IsReviewer ? 1 : 0;
            var isAdmin = user.IsAdmin ? 1 : 0;
            var sql = "update users set ";
            sql += "Username = '" + user.Username + "',";
            sql += "Password = '" + user.Password + "',";
            sql += "Firstname = '" + user.Firstname + "',";
            sql += "Lastname = '" + user.Lastname + "',";
            sql += "Phone = '" + user.Phone + "',";
            sql += "Email = '" + user.Email + "',";
            sql += "IsReviewer = " + (user.IsReviewer ? 1 : 0) + ",";
            sql += "IsAdmin = " + (user.IsAdmin ? 1 : 0);
            sql += $" where Id = {user.Id}";
            var Command = new SqlCommand(sql, Connection);
            var recsAffected = Command.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;

        }

        public static bool DeleteUser(int Id)
        {
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return false;
            }
            var sql = $"delete from users where id = {Id}";
            var Command = new SqlCommand(sql, Connection);
            var recsAffected = Command.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;
        }

        public static bool InsertUser(User user)
        {
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return false;
            }
            var isReviewer = user.IsReviewer ? 1 : 0;
            var isAdmin = user.IsAdmin ? 1 : 0;
            var sql = $"insert into Users (Username, Password, Firstname, Lastname, Email, Phone, IsReviewer, IsAdmin)"+ $"values ('{user.Username}','{user.Password}','{user.Firstname}'," +
                $"'{user.Lastname}','{user.Email}','{user.Phone}',{isReviewer}, {isAdmin})";
            var Command = new SqlCommand(sql, Connection);
            var recsAffected = Command.ExecuteNonQuery();
            Connection.Close();
            return recsAffected == 1;
        }
        private static SqlDataReader CreateSqlReaderAndCheck(string sql, SqlConnection Connection)
        {

            var Command = new SqlCommand(sql, Connection);
            var reader = Command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Result does not contain any rows");
                Connection.Close();
                return null;
            }
            return reader;
        }

        public static User GetByPrimaryKey(int Id)
        {
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return null;
            }
            var sql = $"select * from users where Id = {Id};";
            CreateSqlReaderAndCheck(sql, Connection);
            var reader = CreateSqlReaderAndCheck(sql, Connection);
            reader.Read();
           
                var user = new User();
                user.Id = (int)reader["Id"];
                user.Username = (string)reader["Username"];
                user.Firstname = (string)reader["Firstname"];
                user.Lastname = (string)reader["Lastname"];
                //user.Fullname = $", {Firstname} {Lastname}";
                user.Phone = reader["Phone"] == DBNull.Value ? null : (string)reader["Phone"];
                user.Email = reader["Email"] == DBNull.Value ? null : (string)reader["Email"];
                user.IsReviewer = (bool)reader["IsReviewer"];
                user.IsAdmin = (bool)reader["IsAdmin"];

            
            Connection.Close();
            return user;
        }



        public User(int id, string username, string password, string firstname, string lastname, string phone, string email, bool isReviewer, bool isAdmin)
        {
            Id = id;
            Username = username;
            Password = password;
            Firstname = firstname;
            Lastname = lastname;
            Phone = phone;
            Email = email;
            IsReviewer = isReviewer;
            IsAdmin = isAdmin;
        }

        public static User[] GetAllUsers()
        {
            var Connection = CreateAndCheckConnection();
            if (Connection == null)
            {
                return null;
            }
            var sql = "select * from users;";
            CreateSqlReaderAndCheck(sql, Connection);
            var reader = CreateSqlReaderAndCheck(sql, Connection);
            if (!reader.HasRows)
            {
                Console.WriteLine("Result does not contain any rows");
                Connection.Close();
                return null;
            }
            var users = new User[100];
            var idx = 0;
            while (reader.Read())
            {
                var user = new User();
                user.Id = (int)reader["Id"];
                user.Username = (string)reader["Username"];
                user.Firstname = (string)reader["Firstname"];
                user.Lastname = (string)reader["Lastname"];
                //user.Fullname = $", {Firstname} {Lastname}";
                user.Phone = reader["Phone"] == DBNull.Value ? null : (string)reader["Phone"];
                user.Email = reader["Email"] == DBNull.Value ? null : (string)reader["Email"];
                user.IsReviewer = (bool)reader["IsReviewer"];
                user.IsAdmin = (bool)reader["IsAdmin"];

                users[idx++] = user;
            }
            Connection.Close();
            return users;
        }

        public User()
        {

        }


        public override string ToString()
        {
            return $"[ToPrint()] Id={Id}, Username= {Username}, Password {Password}, Name = {Firstname} {Lastname}";
        }
    }
}
