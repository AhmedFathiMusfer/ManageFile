using ManageFile.Models;
using ManageFile.Processor;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ManageFile.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Text_Processor(IFormFile input_file, string sorted_actions)
        {
            string modifiedContent;
            var actions = JsonConvert.DeserializeObject<List<ActionRequest>>(sorted_actions);
            using (var reader = new StreamReader(input_file.OpenReadStream()))
            {
                var fileContent = reader.ReadToEnd();

                modifiedContent = fileContent;
            }
            foreach (var action in actions)
            {
                if (action.Type == "remove_null")
                {
                    modifiedContent = TextProcessor.remove_null(modifiedContent);
                }
                else if (action.Type == "remove_number_from_start")
                {
                    modifiedContent = TextProcessor.remove_number_from_start(modifiedContent, action.Params);
                }
                else if (action.Type == "remove_contain")
                {
                    modifiedContent = TextProcessor.remove_contain(modifiedContent, action.Params);
                }
                else if (action.Type == "replace")
                {

                    modifiedContent = TextProcessor.replace(modifiedContent, action.Params);
                }
                else if (action.Type == "remove_between")
                {
                    modifiedContent = TextProcessor.remove_between(modifiedContent, action.Params);
                }
                else if (action.Type == "remove_space")
                {
                    modifiedContent = TextProcessor.remove_space(modifiedContent);
                }
                else if (action.Type == "remove_lines_starting_with")
                {
                    modifiedContent = TextProcessor.remove_lines_starting_with(modifiedContent, action.Params);
                }
                else if (action.Type == "remove_from_start")
                {
                    modifiedContent = TextProcessor.remove_from_start(modifiedContent, action.Params);
                }
                else if (action.Type == "searching")
                {
                    modifiedContent = TextProcessor.searching(modifiedContent, action.Params);
                }
            }

            var fileBytes = System.Text.Encoding.UTF8.GetBytes(modifiedContent);
            var fileName = $"Processed_{input_file.FileName}";
            return File(fileBytes, "text/plain", fileName);
        }


    }
}