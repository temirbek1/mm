namespace API.MomsRecipes
{
    public class AuthJSONRequest : JSONCommonObject
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}