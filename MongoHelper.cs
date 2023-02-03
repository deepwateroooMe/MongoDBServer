using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace tetrisServer {

    // 定义的是：从远程服务器发回来的总结信息。如果要返回原数据，如何交互?应该就是需要protobuf隆重登场，来在服务器与客户端之间传递？
    // 所以，需要扩展这个项目，使用protobuf进程间消息传递的协议？
    public class DbMessage {
        // 反馈数量
        public long iFlg { get; set; }
        // 反馈文字描述
        public string Ex { get; set; }

        public override string ToString() {
            return $"返回数据库消息: 是否成功:{string.IsNullOrEmpty(Ex)}, 反馈数量:{iFlg}, 描述:{Ex}";
        }
    }

    // 泛型类的写法：以客户端来连接电脑(远程)服务器
    public class MongoHelper<T> {

        private string connStr = ""; // 服务器网址
        private string dbName = "";  // 数据库名称
        private IMongoClient client; // 连接客户端
        private IMongoDatabase db;   // 连接数据库
        private string collName;     // 集合名称

        public MongoHelper(string connStr, string dbName, string collName) {
            this.connStr = connStr; // 连接字符串
            this.dbName = dbName; 　// 数据库名字
            this.collName = collName; // 表的名字
            this.Init(); 　　　　　　　　// 初始化
        }
        private void Init() {
            if (client == null)
                client = new MongoClient(this.connStr);
            if (db == null)
                db = client.GetDatabase(this.dbName);
        }
 // Insert()
        public DbMessage Insert(T obj) {
            try {
                var collection = db.GetCollection<T>(this.collName);
                collection.InsertOne(obj);
                return new DbMessage() { Ex = string.Empty, iFlg = 1 };
            }
            catch (Exception e) {
                Console.WriteLine("Insert错误!!!");
                return new DbMessage() { Ex = e.Message, iFlg = -1 };
            }
        }
        public DbMessage Insert(Dictionary<string,object> dicInfo) {
            try {
                var collection = db.GetCollection<BsonDocument>(this.collName);
                var document = new BsonDocument(dicInfo);
                collection.InsertOne(document);
                return new DbMessage() { Ex = string.Empty, iFlg = 1 };
            }
            catch (Exception e) {
                Console.WriteLine("Insert错误!!!");
                return new DbMessage() { Ex = e.Message, iFlg = -1 };
            }
        }
        public DbMessage Insert(List<T> documents) {
            try {
                var collection = db.GetCollection<T>(this.collName);
                collection.InsertMany(documents);
                return new DbMessage() { Ex = string.Empty, iFlg = documents.Count };
            }
            catch (Exception e) {
                Console.WriteLine("Insert出错!!!");
                return new DbMessage() { Ex = e.Message, iFlg = -1 };
            }
        }
 // InsertAsync()  异步
        public async Task<DbMessage> InsertAsync(T obj) {
            try {
                var collection = db.GetCollection<T>(this.collName);
                await collection.InsertOneAsync(obj);
                return new DbMessage() { Ex = string.Empty, iFlg = 1 };
            }
            catch (Exception e) {
                Console.WriteLine("InsertAsync错误!!!");
                return new DbMessage() { Ex = e.Message, iFlg = -1 };
            }
        }
        public async Task<DbMessage> InsertAsync(Dictionary<string, object> dicInfo) {
            try {
                var collection = db.GetCollection<BsonDocument>(this.collName);
                var document = new BsonDocument(dicInfo);
                await collection.InsertOneAsync(document);
                return new DbMessage() { Ex = string.Empty, iFlg = 1 };
            }
            catch (Exception e) {
                Console.WriteLine("InsertAsync错误!!!");
                return new DbMessage() { Ex = e.Message, iFlg = -1 };
            }
        }
        public async Task<DbMessage> InsertManyAsync(List<T> documents) {
            try {
                var collection = db.GetCollection<T>(this.collName);
                await collection.InsertManyAsync(documents);
                return new DbMessage() { Ex = string.Empty, iFlg = documents.Count };
            }
            catch (Exception e) {
                Console.WriteLine("InsterManyAsync出错!!!");
                return new DbMessage() { Ex = e.Message, iFlg = -1 };
            }
        }
 // QueryMany()
        public List<T> QueryMany() {
            try {
                var collection = db.GetCollection<T>(this.collName);
                var rest = collection.Find(Builders<T>.Filter.Empty);
                return rest.As<T>().ToList();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<T> QueryMany(string field, object val) {
            try {
                var collection = db.GetCollection<T>(collName);
                var rest = collection.Find(Builders<T>.Filter.Eq(field, val));
                return rest.As<T>().ToList();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<T> QueryMany(BsonDocument document) {
            try {
                var collection = db.GetCollection<T>(collName);
                var rest =collection.Find(document);
                return rest.As<T>().ToList();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }
 // QueryManyAsync()
        public async Task<List<T>> QueryManyAsync() {
            try {
                var collection = db.GetCollection<T>(this.collName);
                var rest = await collection.FindAsync(Builders<T>.Filter.Empty);
                return await rest.ToListAsync<T>();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<T>> QueryManyAsync(string field, object val) {
            try {
                var collection = db.GetCollection<T>(collName);
                var rest = await collection.FindAsync(Builders<T>.Filter.Eq(field, val));
                return await rest.ToListAsync<T>();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<T>> QueryManyAsync(BsonDocument document) {
            try {
                var collection = db.GetCollection<T>(collName);
                var rest = await collection.FindAsync(document);
                return await rest.ToListAsync();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }
 // UpdateOne()
        public DbMessage UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> update, bool upsert = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                var rest = collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = upsert });
                return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
            }
            catch (Exception e) {
                Console.WriteLine("UpdateOne错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
        public DbMessage UpdateOne(string filterKey, object filterVal, string setKey, object setVal, bool upsert = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                var filter = Builders<T>.Filter.Eq(filterKey, filterVal);
                var update = Builders<T>.Update.Set(setKey, setVal);
                var rest = collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = upsert });
                return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
            }
            catch (Exception e) {
                Console.WriteLine("UpdateOne错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
 // UpdateMany()
        public DbMessage UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> update, bool upsert = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                var rest = collection.UpdateMany(filter, update, new UpdateOptions { IsUpsert = upsert });
                return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
            }
            catch (Exception e) {
                Console.WriteLine("UpdateMany错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
        public DbMessage UpdateMany(string filterKey, object filterVal, string setKey, object setVal, bool upsert = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                var filter = Builders<T>.Filter.Eq(filterKey, filterVal);
                var update = Builders<T>.Update.Set(setKey, setVal);
                var rest = collection.UpdateMany(filter, update, new UpdateOptions { IsUpsert = upsert });
                return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
            }
            catch (Exception e) {
                Console.WriteLine("UpdateMany错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
 // UpdateOneAsync()
        public async Task<DbMessage> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, bool upsert = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                var rest = await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = upsert });
                return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
            }
            catch (Exception e) {
                Console.WriteLine("UpdateOneAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
        public async Task<DbMessage> UpdateOneAsync(string filterKey, object filterVal, string setKey, object setVal, bool upsert = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                var filter = Builders<T>.Filter.Eq(filterKey, filterVal);
                var update = Builders<T>.Update.Set(setKey, setVal);
                var rest = await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = upsert });
                return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
            }
            catch (Exception e) {
                Console.WriteLine("UpdateOneAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
 // UpdateManyAsync()
        public async Task<DbMessage> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, bool upsert = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                var rest = await collection.UpdateManyAsync(filter, update, new UpdateOptions { IsUpsert = upsert });
                return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
            }
            catch (Exception e) {
                Console.WriteLine("UpdateManyAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
        public async Task<DbMessage> UpdateManyAsync(string filterKey, object filterVal, string setKey, object setVal, bool upsert = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                var filter = Builders<T>.Filter.Eq(filterKey, filterVal);
                var update = Builders<T>.Update.Set(setKey, setVal);
                var rest = await collection.UpdateManyAsync(filter, update, new UpdateOptions { IsUpsert = upsert });
                return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
            }
            catch (Exception e) {
                Console.WriteLine("UpdateManyAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
 // DeleteOne()
        public DbMessage DeleteOne(string filterKey, string filterVal, bool isTrueDelete = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                if (isTrueDelete) {
                    var rest = collection.DeleteOne(Builders<T>.Filter.Eq(filterKey, filterVal));
                    return new DbMessage { Ex = string.Empty, iFlg = rest.DeletedCount };
                } else {
                    var msg = UpdateOne(filterKey, filterVal, "IsDelete", true);
                    if (msg.iFlg == -1 || !string.IsNullOrEmpty(msg.Ex))
                        throw new Exception("DeleteOne中更新字段IsDelete出错!!!");
                    return new DbMessage { Ex = msg.Ex, iFlg = msg.iFlg };
                }
            }
            catch (Exception e) {
                Console.WriteLine("DeleteOneAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
 // DeleteMany()
        public DbMessage DeleteMany(string filterKey, string filterVal, bool isTrueDelete = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                if (isTrueDelete) {
                    var rest = collection.DeleteMany(Builders<T>.Filter.Eq(filterKey, filterVal));
                    return new DbMessage { Ex = string.Empty, iFlg = rest.DeletedCount };
                } else {
                    var msg = UpdateMany(filterKey, filterVal, "IsDelete", true);
                    if (msg.iFlg == -1 || !string.IsNullOrEmpty(msg.Ex))
                        throw new Exception("DeleteOneAsync中更新字段IsDelete出错!!!");
                    return new DbMessage { Ex = msg.Ex, iFlg = msg.iFlg };
                }
            }
            catch (Exception e) {
                Console.WriteLine("DeleteManyAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
        public DbMessage DeleteMany(FilterDefinition<T> filter, bool isTrueDelete = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                if (isTrueDelete) {
                    var rest = collection.DeleteMany(filter);
                    return new DbMessage { Ex = string.Empty, iFlg = rest.DeletedCount };
                } else {
                    try {
                        var rest = collection.UpdateMany(filter, Builders<T>.Update.Set("IsDelete", true), new UpdateOptions { IsUpsert = false });
                        return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
                    }
                    catch {
                        throw new Exception("DeleteOneAsync中更新字段IsDelete出错!!!");
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("DeleteOneAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
 // DeleteOneAsync()
        public async Task<DbMessage> DeleteOneAsync(FilterDefinition<T> filter,bool isTrueDelete = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                if (isTrueDelete) {
                    var rest = await collection.DeleteOneAsync(filter);
                    return new DbMessage { Ex = string.Empty, iFlg = rest.DeletedCount };
                } else {
                    try {
                        var rest = await collection.UpdateOneAsync(filter, Builders<T>.Update.Set("IsDelete", true), new UpdateOptions { IsUpsert = false });
                        return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
                    }
                    catch {
                        throw new Exception("DeleteOneAsync中更新字段IsDelete出错!!!");
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("DeleteOneAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
 // DeleteManyAsync()
        public async Task<DbMessage> DeleteManyAsync(string filterKey, string filterVal, bool isTrueDelete = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                if (isTrueDelete) {
                    var rest = await collection.DeleteManyAsync(Builders<T>.Filter.Eq(filterKey, filterVal));
                    return new DbMessage { Ex = string.Empty, iFlg = rest.DeletedCount };
                } else {
                    var msg = await UpdateManyAsync(filterKey, filterVal, "IsDelete", true);
                    if (msg.iFlg == -1 || !string.IsNullOrEmpty(msg.Ex))
                        throw new Exception("DeleteOneAsync中更新字段IsDelete出错!!!");
                    return new DbMessage { Ex = msg.Ex, iFlg = msg.iFlg };
                }
            }
            catch (Exception e) {
                Console.WriteLine("DeleteManyAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
        public async Task<DbMessage> DeleteManyAsync(FilterDefinition<T> filter, bool isTrueDelete = false) {
            try {
                var collection = db.GetCollection<T>(collName);
                if (isTrueDelete) {
                    var rest = await collection.DeleteManyAsync(filter);
                    return new DbMessage { Ex = string.Empty, iFlg = rest.DeletedCount };
                } else {
                    try {
                        var rest = await collection.UpdateManyAsync(filter, Builders<T>.Update.Set("IsDelete", true), new UpdateOptions { IsUpsert = false });
                        return new DbMessage { Ex = string.Empty, iFlg = rest.ModifiedCount };
                    }
                    catch {
                        throw new Exception("DeleteOneAsync中更新字段IsDelete出错!!!");
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("DeleteOneAsync错误!!!");
                return new DbMessage { Ex = e.Message, iFlg = -1 };
            }
        }
    }
}