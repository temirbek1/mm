using System.Collections.Generic;

namespace API.MomsRecipes
{
    public class Link : JSONCommonObject
    {
        public string url { get; set; }
        public string label { get; set; }
        public bool active { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public object prev { get; set; }
        public object next { get; set; }
    }
    public class Meta : JSONCommonObject
    {
        public int current_page { get; set; }
        public int from { get; set; }
        public int last_page { get; set; }
        public List<Link> links { get; set; }
        public string path { get; set; }
        public int per_page { get; set; }
        public int to { get; set; }
        public int total { get; set; }
    }

    public class CustomerSearchAnswer : JSONCommonObject
    {
        public List<Customer> data { get; set; }
        public Link links { get; set; }
        public Meta meta { get; set; }
    }
}