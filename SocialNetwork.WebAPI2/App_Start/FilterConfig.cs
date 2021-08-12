using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.WebAPI2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
