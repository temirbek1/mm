using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API.MomsRecipes
{
    public class CreaterOrderAnswer : JSONCommonObject
    {
        public int customer_id { get; set; }
        public string phone { get; set; }
        public int count { get; set; }
        public int sum { get; set; }
        public string delivery { get; set; }
        public DateTime delivery_date { get; set; }
        public List<string> payment_guid { get; set; }
        public int delivery_sum { get; set; }
        public string status { get; set; }
        public int bonus_added { get; set; }
        public string name { get; set; }
        public string comments { get; set; }
        public int bonus_used { get; set; }
        public int payment_sum { get; set; }
        public string hash { get; set; }
        public List<string> payment { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public int id { get; set; }
    }
}