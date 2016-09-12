using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Repository.Utils;
using Repository.DataBase;

namespace Repository.UserAccount
{
    public static class UserAccount
    {
        static IMongoClient _client = new MongoClient();
        static IMongoDatabase _database = _client.GetDatabase("GateKeeperDatabase");

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="NewUser">User data</param>
        /// <returns>Description of the result of the operation</returns>
        public static RequestResult Create(User NewUser)
        {
            RequestResult requestResult;

            try
            {
                if (DB.DatabaseConnected(_client))
                {
                    if (FindUserName(NewUser.userName) == null) //Check if the Username already exists
                    {
                        var Collec = _database.GetCollection<BsonDocument>("users");
                        var documnt = new BsonDocument
                                {
                                {"name", NewUser.name},
                                {"userName",NewUser.userName},
                                {"password",NewUser.password}
                                };
                        Collec.InsertOne(documnt);
                        requestResult = new RequestResult(ResultType.Success, "User created!");
                    }
                    else
                    {
                        requestResult = new RequestResult(ResultType.Failure, "User already exists!!");
                    }
                }
                else
                {
                    requestResult = new RequestResult(ResultType.Failure, "Unable to connect to the database. Please make sure that " + _client.Settings.Server.Host + " is online");
                }

            }
            catch (Exception ex)
            {
                requestResult = new RequestResult(ResultType.Failure, ex.Message);
            }
            

            return requestResult;
        }

        /// <summary>
        /// Edit a User
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        //public static RequestResult Edit(ObjectId IdUser)
        //{
        //    RequestResult requestResult;
        //    User UserEdit;
        //    try
        //    {
        //        if (DB.DatabaseConnected(_client))
        //        {
        //            UserEdit = FindUserById(IdUser);
        //            if ( UserEdit != null) //Check if the Username exists
        //            {
        //                var Collec = _database.GetCollection<User>("users");
        //                var documnt = new User
        //                        {
        //                        {"name", NewUser.name},
        //                        {"userName",NewUser.userName},
        //                        {"password",NewUser.password}
        //                        };
        //                Collec.InsertOne(documnt);
        //                requestResult = new RequestResult(ResultType.Success, "User created!");
        //            }
        //            else
        //            {
        //                requestResult = new RequestResult(ResultType.Failure, "User doesn't exists!!");
        //            }
        //        }
        //        else
        //        {
        //            requestResult = new RequestResult(ResultType.Failure, "Unable to connect to the database. Please make sure that " + _client.Settings.Server.Host + " is online");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        requestResult = new RequestResult(ResultType.Failure, ex.Message);
        //    }


        //    return requestResult;
        //}


        /// <summary>
        /// Search a user by it's UserName
        /// </summary>
        public static User FindUserName(string UserName)
        {
            //var Filter = Builders<BsonDocument>.Filter.Eq("UserName", UserName.ToLower());
            //var Users1 = _database.GetCollection<BsonDocument>("users").Find(Filter);

            var UserCollection = _database.GetCollection<User>("users");
            var Users = UserCollection.AsQueryable<User>();

            var UserQuery = from u in Users
                            where u.userName.ToLower() == UserName
                            select u;

            return UserQuery.FirstOrDefault();
            //Console.WriteLine(UserQuery);
            //return Users.Count() > 0 ? Users : null;             
        }

        /// <summary>
        /// Search a user by it's UserName
        /// </summary>
        /// <param name="idUser">MongnDB row's ObjectId </param>
        /// <returns></returns>
        public static User FindUserById(ObjectId idUser)
        {
            var UserCollection = _database.GetCollection<User>("users");
            return (from user in UserCollection.AsQueryable<User>()
                    where (ObjectId)user._id == idUser
                    select user).FirstOrDefault();            
        }

        public static IList<User> UsersList()
        {
            var UserCollection = _database.GetCollection<User>("users");
            var Users = UserCollection.AsQueryable<User>();

            var UserQuery = from user in Users
                            select user;

            return UserQuery.ToList();            
        }
    }
}
