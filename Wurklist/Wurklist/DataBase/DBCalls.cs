using NPoco;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wurklist.Models;

namespace Wurklist.DataBase
{
    internal class DBCalls
    {
        IDatabase db;

        public DBCalls()
        {
            //conn.ConnectionString = "Server=10.110.110.121;Database=Wurklist;user Id=arjan; Password=YecGaa";
        }

        ////
        /// Select statements
        ////

        public List<CustomTask> getCustomTasksUser(int id)
        {
            return db.Fetch<CustomTask>("Where UserId=" + id);
            
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
