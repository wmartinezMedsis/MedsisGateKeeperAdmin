using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;

namespace Repository.DataBase
{
    
    public static class DB
    {
        //public static List<BsonDocument> SelectDocument(string DocumentName)
        //{

        //}

        public static bool DatabaseConnected(IMongoClient mongoClient)
        {
            bool connected = true;
            int count = 0;

            while (mongoClient.Cluster.Description.State.ToString() == "Disconnected")
            {
                Thread.Sleep(100);
                if (count++ >= 30)
                {
                    connected = false;
                    break;
                }

            }

            return connected;
        }
    }
}
