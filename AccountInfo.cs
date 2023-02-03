using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetrisServer {

    class AccountInfo {

        public ObjectId _id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        // public int Age { get; set; }
        // public bool Sex { get; set; }
    }
}
