using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using task3_dotnetProject.JSONmodels;
using task3_dotnetProject.JSONmodels.JSONdatatypes;

namespace task3_dotnetProject.Services
{
    public class OWMforecastService
    {
        private readonly HttpClient OMWclient;
        private readonly JSONparser JSONparser;


        public OWMforecastService(HttpClient OMWclient, JSONparser JSONparser)
        {
            this.OMWclient = OMWclient;
            this.JSONparser = JSONparser;
        }

        public async Task<JSONcomponent> GetJSONWeatherForecasts(string city)
        {
            var response = await this.OMWclient.GetAsync(
                $"forecast?q={city}&lang=it&appid=9daff0e8d638ab18ce883d8a43130353");

            if (response == null || !response.IsSuccessStatusCode)
                return null;

            string responseContent = await response.Content.ReadAsStringAsync();

            TextReader tr = new StringReader(responseContent);
            string firstLine = tr.ReadLine();

            if (firstLine == null) return null;

            JSONcomponent json = this.JSONparser.Parse(new StringBuilder(firstLine), tr);
            return json;
        }

    }
}
