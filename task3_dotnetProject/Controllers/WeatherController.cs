using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using task3_dotnetProject.JSONmodels.JSONdatatypes;
using task3_dotnetProject.Services;

namespace task3_dotnetProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly OWMforecastService OWMservice;


        public WeatherController(OWMforecastService OWMservice)
        {
            this.OWMservice = OWMservice;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetAsync(string city, string date)
        {
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(date))
                return BadRequest();

            string[] dateFields = date.Split("-");

            if (dateFields.Length != 3)
                return BadRequest();

            if(dateFields[0].Length != 2 || dateFields[1].Length != 2 || dateFields[2].Length != 4)
            {
                ModelState.AddModelError("date", "date must be in the format dd-mm-aaaa");
                return BadRequest(ModelState);
            }

            bool dayParsed = int.TryParse(dateFields[0], out int day);
            bool monthParsed = int.TryParse(dateFields[1], out int month);
            bool yearParsed = int.TryParse(dateFields[2], out int year);

            if(!dayParsed || !monthParsed || !yearParsed)
            {
                ModelState.AddModelError("date", $"{date} is not a valid date");
                return BadRequest(ModelState);
            }

            DateTime requestedDate;

            try
            {
                requestedDate = new DateTime(year, month, day);
            }
            catch(ArgumentOutOfRangeException)
            {
                ModelState.AddModelError("date", $"{date} is not a valid date");
                return BadRequest(ModelState);
            }

            DateTime now = DateTime.Now;
            DateTime today = new DateTime(now.Year, now.Month, now.Day);
            DateTime plus5Days = today.AddDays(5);

            if (today.CompareTo(new DateTime(today.Year, today.Month, today.Day, 21, 0, 0)) > 0)
                today = today.AddDays(1);

            if (now.AddDays(5).CompareTo(new DateTime(plus5Days.Year, plus5Days.Month, plus5Days.Day, 3, 0, 0)) < 0)
                plus5Days = plus5Days.AddDays(-1);

            if(requestedDate.CompareTo(today) < 0 || requestedDate.CompareTo(plus5Days) > 0)
            {
                string todayPrinted = string.Format("{0:D2}-{0:D2}-{0:D4}", today.Day, today.Month, today.Year);
                string plus5DaysPrinted = string.Format("{0:D2}-{0:D2}-{0:D4}", plus5Days.Day, plus5Days.Month, plus5Days.Year);

                ModelState.AddModelError("date", $"{date} is in the past or too far in the future." +
                    $" Choose a date between {todayPrinted} and {plus5DaysPrinted}");
                return BadRequest(ModelState);
            }

            // here 'date' is a valid date

            JSONcomponent next5daysForecasts = await this.OWMservice.GetJSONWeatherForecasts(city);

            if(next5daysForecasts == null)
            {
                ModelState.AddModelError("city", $"{city} is not a valid city");
                return BadRequest(ModelState);
            }

            string wantedKey = "dt_txt";
            string wantedDate = string.Format("{2:D4}-{1:D2}-{0:D2}", requestedDate.Day, requestedDate.Month, requestedDate.Year);

            // retrieve all the hourly forecasts associated with the requested date
            JSONarray desiredForecasts = next5daysForecasts.RetrieveObjectsWithProperty(wantedKey, wantedDate);

            // remove 'dt' property, as it is shown in the output file

            string propertyToRemove = "dt";
            desiredForecasts.RemoveObjectProperty(propertyToRemove);

            return Ok(desiredForecasts.Print(0));
        }
    }
}
