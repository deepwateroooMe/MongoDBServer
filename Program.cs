namespace tetrisServer {
    class Program {
        static void Main(string[] args) {

            //var connStr = "mongodb://127.0.0.1:27017";
            //var client = new MongoDB.Driver.MongoClient(connStr);
            //var db = client.GetDatabase("test");
            //var studentCollection = db.GetCollection<Students>("Students");
            //var student = new Students()
            //{
            //    _id = ObjectId.GenerateNewId(),
            //    Name = "zzs",
            //    Age = 18,
            //    Sex = true,
            //};
            //Console.WriteLine("开始插入");
            //studentCollection.InsertOneAsync(student);
            //Console.WriteLine("插入完成");

            //Thread thread = new Thread(async () =>
            //{
            //    MongoHelper<Students> studentsMongoHeaper = new MongoHelper<Students>("mongodb://127.0.0.1:27017", "test", "Students");
            //    var msg = await studentsMongoHeaper.InsterManyAsync(new List<Students>()
            //    {
            //        new Students{Name = "wy",Age = 14,Sex = true},
            //        new Students{Name = "pnb",Age = 20,Sex = true}
            //    });
            //    Console.WriteLine(msg);
            //    Console.WriteLine("插入完成!!!");
            //});
            //thread.Start();

            //Thread thread = new Thread(async () =>
            //{
            //    MongoHelper<Students> studentsMongoHeaper = new MongoHelper<Students>("mongodb://127.0.0.1:27017", "test", "Students");
            //    var doc = new BsonDocument();
            //    doc.Add("Age",18);
            //    doc.Add("Sex",false);
            //    var allList = await studentsMongoHeaper.QueryAsync(doc);
            //    Console.WriteLine(allList[0].Name);
            //});
            //thread.Start();

            //Thread thread = new Thread(async () =>
            //{
            //    MongoHelper<Students> studentsMongoHeaper = new MongoHelper<Students>("mongodb://127.0.0.1:27017", "test", "Students");
            //    var msg = await studentsMongoHeaper.UpdateManyAsync
            //        (
            //        Builders<Students>.Filter.Eq("Name", "zzs"),
            //        Builders<Students>.Update.Set("Age", 777)
            //        );
            //    Console.WriteLine(msg);
            //});
            //thread.Start();

 // 狠简单：开了一个异步线程，以客户端的形式去连接电脑本机上的远程数据库，连接到名为MeMeMe的数据库，表格名字是AccountInfo            
            Thread thread = new Thread(async () => {
                MongoHelper<AccountInfo> studentsMongoHeaper = new MongoHelper<AccountInfo>("mongodb://127.0.0.1:27017", "MeMeMe", "AccountInfo");
 // 然后以　远程客户端　的形式　操作　远程数据库　服务器
                var msg = await studentsMongoHeaper.DeleteManyAsync("Account","1",true);
                Console.WriteLine(msg); // 把返回消息打印出来
            });
            thread.Start();
            Console.ReadKey();

            
            // // 下面的：　类里面的东西需要完善一下            
            // MongoDBHelper<Video> videoHelper = new MongoHelper<Video>();
            // // 新增
            // videoHelper.Collection.InsertOne(new Video() {
            //         Title = "ggg"
            //     });
            // // 查找集合
            // var list = videoHelper.Collection.Find(videoHelper.Filter.Eq(e => e.Category, "Horror")).ToList();
            // list = videoHelper.Collection.Find(videoHelper.Filter.Eq(e => e.Title, "ggg")).ToList();
            // // 方式一：拼接查找条件
            // List<FilterDefinition<Video>> listFilter = new List<FilterDefinition<Video>>() {
            //     videoHelper.Filter.Eq(e=>e.Minutes,118),
            //     videoHelper.Filter.Eq(e=>e.Title,"The Perfect Developer")
            // };
            // // 根据查找多个条件筛选集合
            // list = videoHelper.Collection.Find(videoHelper.Filter.And(listFilter)).ToList();
            // // 方式二：拼接查找条件
            // var filter = videoHelper.Filter.Eq(e => e.Title, "cys") & videoHelper.Filter.Eq(e => e.Category, "cys");
            // list = videoHelper.Collection.Find(filter).ToList();
            // // 拼接更新字段
            // var updateDefinition = new List<UpdateDefinition<Video>>() {
            //     videoHelper.Update.Set(e=>e.Title, "cys"),
            //     videoHelper.Update.Set(e=>e.Category, "cys")
            // };
            // // 按照条件更新
            // videoHelper.Collection.UpdateMany(videoHelper.Filter.And(listFilter),
            //                                   videoHelper.Update.Combine(updateDefinition));
            // // 删除
            // videoHelper.Collection.DeleteOne(videoHelper.Filter.Eq(e => e.Title, "Lost In Frankfurt am Main"));
            // // 获取某个字段
            // filter = videoHelper.Filter.Eq(e => e.Title, "ggg");
            // var bson = videoHelper.Collection.Find(filter).Project(videoHelper.Projection.Include(e => e.Title).Exclude(e => e._id)).FirstOrDefault();
        }
    }
}    