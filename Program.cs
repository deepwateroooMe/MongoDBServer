namespace tetrisServer {

    class Program {
        static void Main(string[] args) {

 // 狠简单：开了一个异步线程，以客户端的形式去连接电脑本机上的远程数据库，连接到名为MeMeMe的数据库，表格名字是AccountInfo            
            Thread thread = new Thread(async () => {
                MongoHelper<AccountInfo> studentsMongoHeaper = new MongoHelper<AccountInfo>("mongodb://127.0.0.1:27017", "MeMeMe", "AccountInfo");
 // 然后以　远程客户端　的形式　操作　远程数据库　服务器
                var msg = await studentsMongoHeaper.DeleteManyAsync("Account","1",true);
                Console.WriteLine(msg); // 把返回消息打印出来
            });
            thread.Start();
            Console.ReadKey();
        }
    }
}    