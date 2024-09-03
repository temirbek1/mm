using System.Collections.Generic;

namespace API.MomsRecipes
{
    public class AuthJSONAnswer : JSONCommonObject

    {
        public string token { get; set; }
        public int user_id { get; set; }
        public int customer_id { get; set; }
        public List<Error> errors { get; set; }
        public string message { get; set; }
    }
}