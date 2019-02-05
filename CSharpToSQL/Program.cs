using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CSharpToSQL
{
    class Program
    {
        //DSI-WORKSTATION\SQLEXPRESS
        static void Main(string[] args)
        {
            //var user = new User();
            var user = new User(0, "Trashy Tom", "bengals", "Tom", "Trash", "5555555555", "houseofpain@gmail.com", true, true);
            User[] users = User.GetAllUsers();
            var returnCode = User.InsertUser(user);
            foreach(var u in users)
            {
                if(u == null)
                {
                    continue;
                }
                Console.WriteLine($"{u.Firstname} {u.Lastname} , ={u.Username}");
            }
            User userpk = User.GetByPrimaryKey(1);
            Console.WriteLine($"{userpk.Firstname} {userpk.Lastname},{userpk.Username}");

            Console.ReadKey();
        }
    }
}
