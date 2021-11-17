using System;

namespace Data
{
    public class BaseModel {

        public string Id { get; set; }

        public Microservice Microservice { get; set; }

        public string Action {get; set;}
    
        public string Type {get; set;}
        public string ApiRoute {get; set;}
        public string ObjectId {get; set;}
        public string DataSent {get; set;}
        public string OldData {get; set;}
        public string Description {get; set;}
        public DateTimeOffset CreatedDate { get; set; }

        public User User { get; set; }
}
}
