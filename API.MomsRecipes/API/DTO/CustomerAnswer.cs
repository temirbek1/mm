using System;
using System.Collections.Generic;

namespace API.MomsRecipes
{  
    public class Customer : JSONCommonObject
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public object address { get; set; }
        public string birthday { get; set; }
        public Decimal bonus { get; set; }
        public string hash { get; set; }
        public int user_added { get; set; }
        public string token { get; set; }
        public Notify notify { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object deleted_at { get; set; }
        public string qr { get; set; }
        public string email { get; set; }
    }

    public class Notify : JSONCommonObject
    {
        public int orders { get; set; }
        public int promotions { get; set; }
    }

    public class CustomerAnswer : JSONCommonObject
    {
        public Customer data { get; set; }
    }
}