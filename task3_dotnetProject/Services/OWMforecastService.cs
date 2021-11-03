using System;
using System.Net.Http;

namespace task3_dotnetProject.Services
{
    public class OWMforecastService
    {
        private readonly HttpClient OMWclient;

        public OWMforecastService(HttpClient OMWclient)
        {
            this.OMWclient = OMWclient;
        }

        //

    }
}
