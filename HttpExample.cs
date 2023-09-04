using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Vaccination_Status_Query
{
    public static class HttpExample
    {

        [FunctionName("id")]
        public static async Task<IActionResult> Run(

             [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "id")] HttpRequest req/*, string name*/, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string[] idNumbers = { "0312095594086", "7208145594086", "0310095594086", "1002085475072" };
            string[] names = { "James Smith", "Steve John", "Sam Jones", "Phil Burns" };
            string[] vaccineCentre = { "Dischem", "Momentum", "Alpha Pharmacy", "Dischem" };
            string[] vaccine = { "Pfizer", "J&J", "Pfizer", "Pfizer" };
            int[] dosage = { 2, 1, 2, 2 };

            string name = req.Query["name"];
            var result = Array.IndexOf(idNumbers, name);


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string active;
            active =
                    "This HTTP triggered function executed successfully." +
                    "\n-------------------------------------------------" +
                    "\nVaccination Information" +
                    "\n-------------------------------------------------" +
                    "\nID: " + idNumbers[result] +
                    "\nFull Name: " + names[result] +
                    "\nVaccination Centre: " + vaccineCentre[result] +
                    "\nVaccine Name: " + vaccine[result] +
                    "\nDosage Amount: " + dosage[result] +
                    "\n-------------------------------------------------";


            string responseMessage = string.IsNullOrEmpty(active)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{active} \nThis HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
