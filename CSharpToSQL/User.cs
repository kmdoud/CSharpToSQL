using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToSQL
{
    class User
    {


        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }

        public static bool InsertUser(User user)
        {
            var connStr = (@"server=DSI-WORKSTATION\SQLEXPRESS;database=PrsDb;trusted_connection=true;"); //"uid=sa;pwd=sa;" instead of trusted_connection
            var Connection = new SqlConnection(connStr);
            Connection.Open();
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connection FAILED");
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
        public static User GetByPrimaryKey(int Id)
        {
            var connStr = (@"server=DSI-WORKSTATION\SQLEXPRESS;database=PrsDb;trusted_connection=true;"); //"uid=sa;pwd=sa;" instead of trusted_connection
            var Connection = new SqlConnection(connStr);
            Connection.Open();
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connection FAILED");
                return null;
            }
            var sql = $"select * from users where Id = {Id};";
            var Command = new SqlCommand(sql, Connection);
            var reader = Command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine("Result does not contain any rows");
                Connection.Close();
                return null;
            }
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
            var connStr = (@"server=DSI-WORKSTATION\SQLEXPRESS;database=PrsDb;trusted_connection=true;"); //"uid=sa;pwd=sa;" instead of trusted_connection
            var Connection = new SqlConnection(connStr);
            Connection.Open();
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Connection FAILED");
                return null;
            }
            var sql = "select * from users;";
            var Command = new SqlCommand(sql, Connection);
            var reader = Command.ExecuteReader();
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


        public string ToPrint()
        {
            return $"[ToPrint()] Id={Id}, Username= {Username}, Name = {Firstname} {Lastname}";
        }
    }
}
