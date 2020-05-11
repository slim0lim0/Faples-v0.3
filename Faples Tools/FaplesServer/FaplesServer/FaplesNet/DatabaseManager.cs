using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace FaplesNet
{
    public class DatabaseManager
    {
        private const string SERVER_NAME = ".";
        private const string DATABASE_NAME = "fplServer";

        private const string DATABASE_USERS = "Users";

        private IMongoDatabase oFaplesDB;

        public void InitDatabase()
        {
            var client = new MongoClient();
            oFaplesDB = client.GetDatabase(DATABASE_NAME);
            //InsertRecord("Users", new UserLogin { Username = "TEST", Password = "123" });
        }

        public T GetRecordByKey<T>(string sTable, string sKey, string sKeyValue)
        {
            T oRecord = default(T);

            var colTable = oFaplesDB.GetCollection<T>(sTable);
            var filter = Builders<T>.Filter.Eq(sKey, sKeyValue);

            if(colTable.Find(filter).CountDocuments() > 0)
                oRecord = colTable.Find(filter).First();

            return oRecord;
        }

        public List<T> GetRecords<T>(string sTable)
        {
            var colTable = oFaplesDB.GetCollection<T>(sTable);
            return colTable.Find(new BsonDocument()).ToList();
        }

        public void InsertRecord<T>(string sTable, T oRecord)
        {
            var colTable = oFaplesDB.GetCollection<T>(sTable);
            colTable.InsertOne(oRecord);
        }

        public void UpsertRecord<T>(string sTable, T oRecord)
        {
            var colTable = oFaplesDB.GetCollection<T>(sTable);

            var result = colTable.ReplaceOne(new BsonDocument(), oRecord, new UpdateOptions { IsUpsert = true });
        }

        public void UpdateRecord<T>(string sTable, T oRecord)
        {
            var colTable = oFaplesDB.GetCollection<T>(sTable);

            var result = colTable.ReplaceOne(new BsonDocument(), oRecord, new UpdateOptions { IsUpsert = false });
        }

        public void DeleteRecord<T>(string sTable, string sKey, string sKeyValue)
        {
            var colTable = oFaplesDB.GetCollection<T>(sTable);
            var filter = Builders<T>.Filter.Eq(sKey, sKeyValue);
            colTable.DeleteOne(filter);
        }
    }
}
