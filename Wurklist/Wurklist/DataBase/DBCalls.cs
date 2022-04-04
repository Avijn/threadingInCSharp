using MySql.Data.MySqlClient;
using NPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wurklist.Models;

namespace Wurklist.DataBase
{
    internal class DBCalls
    {
        string connectionString;
        MySqlConnection conn;
        MySqlCommand cmd;

        public DBCalls()
        {
            connectionString = @"server=10.110.110.121;database=Wurklist;user id=arjan;password=YecGaa";
        }

        ////
        /// Select statements
        ////

        public void getCustomTasksUser(int id)
        {
            string sql = @"SELECT * FROM Task";
            conn = new MySqlConnection(connectionString);
            cmd = new MySqlCommand(sql, conn);
            conn.Open();
            DataTable dataTable = new DataTable();

            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                da.Fill(dataTable);
            }
            conn.Close();
        }


        ////
        /// Insert statements
        ////


        ////
        /// Update statements
        ////


        ////
        /// Delete statements
        ////
        

    }
}
