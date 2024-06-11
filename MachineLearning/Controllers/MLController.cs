using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MachineLearning.Models;

public class MLController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MLController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Input()
    {
        return View(new PredictionInputModel());
    }

    [HttpPost]
    public async Task<IActionResult> Predict(PredictionInputModel input)
    {
        var client = _httpClientFactory.CreateClient();
        var apiUrl = "https://c02e-104-199-152-77.ngrok-free.app/predict"; // URL từ ngrok

        var inputData = new
        {
            gender = input.Gender,
            customer_type = input.CustomerType,
            age = input.Age,
            type_of_travel = input.TypeOfTravel,
            Travelclass = input.Class,
            flight_distance = input.FlightDistance,
            inflight_wifi_service = input.InflightWifiService,
            departure_arrival_time_convenient = input.DepartureArrivalTimeConvenient,
            ease_of_online_booking = input.EaseOfOnlineBooking,
            gate_location = input.GateLocation,
            food_and_drink = input.FoodAndDrink,
            online_boarding = input.OnlineBoarding,
            seat_comfort = input.SeatComfort,
            inflight_entertainment = input.InflightEntertainment,
            on_board_service = input.OnBoardService,
            leg_room_service = input.LegRoomService,
            baggage_handling = input.BaggageHandling,
            checkin_service = input.CheckinService,
            inflight_service = input.InflightService,
            cleanliness = input.Cleanliness,
            departure_delay_in_minutes = input.DepartureDelayInMinutes,
            arrival_delay_in_minutes = input.ArrivalDelayInMinutes
        };

        var content = new StringContent(JsonConvert.SerializeObject(inputData), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(apiUrl, content);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var predictionResponse = JsonConvert.DeserializeObject<Dictionary<string, int>>(responseString);
            if (predictionResponse != null && predictionResponse.ContainsKey("prediction"))
            {
                var predictionValue = predictionResponse["prediction"];
                ViewBag.Prediction = predictionValue == 0 ? "The customer is satisfiied" : "The customer is neutral or dissatisfied";
                
            }
        }
        else
        {
            ViewBag.Prediction = "Error in prediction API call.";
        }

        return View("Input", input);
    }
}
