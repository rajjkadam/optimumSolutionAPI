using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using weapon.Models;

namespace optimumSolutionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {

        private readonly HttpClient _httpClient;

        public InvoiceController(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        [HttpGet("Customer")]
        public async Task<IActionResult> CustomersList()
        {
            string apiUrl = "https://getinvoices.azurewebsites.net/api/Customers";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            else
            {
                // Handle the error or return an appropriate response.
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {



                _httpClient.BaseAddress = new Uri("https://getinvoices.azurewebsites.net/");

                var response = await _httpClient.GetAsync($"api/Customer/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var customer = await response.Content.ReadAsStringAsync();
                    return Ok(customer);
                }
                else
                {
                    return BadRequest("Failed to retrieve customer from the external API.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the request.
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("Customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] customerDTO customer)
        {
            try
            {

                string customerJson = Newtonsoft.Json.JsonConvert.SerializeObject(customer);



                _httpClient.BaseAddress = new Uri("https://getinvoices.azurewebsites.net/");

                var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Customer", content);

                if (response.IsSuccessStatusCode)
                {

                    return Ok("Customer created successfully.");
                }
                else
                {
                    return BadRequest("Failed to create customer in the external API.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("Customer/{id}")]
        public async Task<IActionResult> RemoveCustomer(int id)
        {
            try
            {



                _httpClient.BaseAddress = new Uri("https://getinvoices.azurewebsites.net/");

                var response = await _httpClient.DeleteAsync($"api/Customer/{id}");

                if (response.IsSuccessStatusCode)
                {

                    return Ok("Customer removed successfully.");
                }
                else
                {

                    return BadRequest("Failed to remove customer from the external API.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}

