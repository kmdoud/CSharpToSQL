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
            var user = new User(0, "Crumby Cam", "bengals", "Cam", "Crumb", "5555555555", "houseofpain@gmail.com", true, true);
            User[] users = User.GetAllUsers();
            //var returnCode = User.InsertUser(user);
            foreach(var u in users)
            {
                if(u == null)
                {
                    continue;
                }
                Console.WriteLine(u);
            }
            const int ID = 5;
            User userpk = User.GetByPrimaryKey(ID);
            Console.WriteLine(userpk);

            userpk.Password = "thiscodeisdifficult";
            var updateSuccess = User.UpdateUser(userpk);
            if(updateSuccess)
            {
                Console.WriteLine("Update Successful");
            }
            else
            {
                Console.WriteLine("Update Failed");
            }

            var deleteSuccess = User.DeleteUser(ID);
            if (!deleteSuccess)
            {
                Console.WriteLine("Delete Failed!");
            }

            Console.ReadKey();
        }
    }
}
