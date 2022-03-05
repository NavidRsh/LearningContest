using Microsoft.Extensions.Configuration;

namespace LearningContest.Application.Features
{
    public class BaseHandler
    {
        public string RayanBoUrl { get; }
        public static string RayanBoToken { get; set; }
        public string RayanBoDsName { get; }
        public string RayanBoPasssword { get; }
        public string RayanBoUsername { get; }


        public string MabnaUrl { get; }
        public static string MabnaToken { get; set; }

        public BaseHandler(IConfiguration configuration)
        {
            RayanBoUrl = configuration.GetValue<string>("RayanBo:BO_Url");
            RayanBoDsName = configuration.GetValue<string>("RayanBo:BO_dsName");
            RayanBoUsername = configuration.GetValue<string>("RayanBo:Username");
            RayanBoPasssword = configuration.GetValue<string>("RayanBo:Password");

            MabnaUrl = configuration.GetValue<string>("Mabna:Url");
            MabnaToken = configuration.GetValue<string>("Mabna:Token");

        }

       


    }
}