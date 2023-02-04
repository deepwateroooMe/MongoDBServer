using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetrisServer {

    public class MongoDB {
        // 数据库所在主机的端口
        private readonly int MONGO_CONN_PORT = 27017;
        // 设置连接超时15秒
        private readonly int CONNECT_TIME_OUT = 15;
        // 设置最大连接数
        private readonly int MAXConnectionPoolSize = 99;
        // 设置最小连接数
        private readonly int MINConnectionPoolSize = 1;

        // 获得数据库实例
        // <param name="MONGO_CONN_HOST">数据库主机链接</param>
        // <param name="DB_Name">数据库名称</param>
        public MongoDatabase GetDataBase(string MONGO_CONN_HOST, string DB_Name) {
            MongoClientSettings mongoSetting = new MongoClientSettings();
            mongoSetting.ConnectTimeout = new TimeSpan(CONNECT_TIME_OUT * TimeSpan.TicksPerSecond);  // 设置超时连接
            mongoSetting.Server = new MongoServerAddress(MONGO_CONN_HOST, MONGO_CONN_PORT);  // 设置数据库服务器
            mongoSetting.MaxConnectionPoolSize = MAXConnectionPoolSize;  // 设置最大连接数
            mongoSetting.MinConnectionPoolSize = MINConnectionPoolSize;  // 设置最小连接数
            MongoClient client = new MongoClient(mongoSetting);  // 创建Mongo客户端
            return client.GetServer().GetDatabase(DB_Name);  // 得到服务器端并生成数据库实例
        }

        // 得到数据库服务器
        // <param name="MONGO_CONN_HOST">数据库主机链接</param>
        public MongoServer GetDataBaseServer(string MONGO_CONN_HOST) {
            MongoClientSettings mongoSetting = new MongoClientSettings();
            mongoSetting.ConnectTimeout = new TimeSpan(CONNECT_TIME_OUT * TimeSpan.TicksPerSecond);  // 设置超时连接
            mongoSetting.Server = new MongoServerAddress(MONGO_CONN_HOST, MONGO_CONN_PORT);  // 设置数据库服务器
            mongoSetting.MaxConnectionPoolSize = MAXConnectionPoolSize;  // 设置最大连接数
            mongoSetting.MinConnectionPoolSize = MINConnectionPoolSize;  // 设置最小连接数
            MongoClient client = new MongoClient(mongoSetting);  // 创建MongoDB客户端
            return client.GetServer();
        }
    }
}
