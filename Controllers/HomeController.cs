using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using System.Text.Json;
using testirovanie.Models;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;

namespace testirovanie.Controllers
{
	public class HomeController : Controller
	{
		List<Command> Commands;
		public static List<Event> Events = new List<Event>();
		//string token = "577f185da0e44570b75de85c";
		string token = "f0d17d3cae184917802e2ef2";


        public HomeController()
		{
			this.Commands = GetCommands();
		}
        [HttpGet]
		public IActionResult Index(int id = 1)
		{
			ViewData["id"] = id;
			ViewBag.Commands = Commands;
			ViewBag.Events = from e in HomeController.Events
							 where e.Term_Id == id
							 select e;
			return View();
		}
		[HttpPost]
		public IActionResult Index()
		{
			Event e = new Event();
			if (int.TryParse(Request.Form["id"], out var id) &&
				int.TryParse(Request.Form["parametr1_value"], out var v1) &&
				int.TryParse(Request.Form["parametr2_value"], out var v2) &&
				int.TryParse(Request.Form["parametr3_value"], out var v3) &&
				int.TryParse(Request.Form["command_id"], out var c_id))
			{
				e.commandName = this.Commands.FirstOrDefault(c => c.command_id == c_id).name;
				e.Term_Id = id;
				e.Val1 = v1;
				e.Val2 = v2;
				e.Val3 = v3;

				string url = $"http://178.57.218.210:398/terminals/{id}/commands?token={this.token}";
				var data = new {
					command_id = c_id,
					parameter1 = v1,
					parameter2 = v2,
                    parameter3 = v3
                };

				JObject o = JObject.Parse(SendCommandAsync(url, data, "POST").Result);

				if (o.ContainsKey("error"))
					e.Status = "Не отправлено. " + (string?)o["error"];
				else
				{
					e.Status = (string?)o["item"]["state_name"];
				}

				HomeController.Events.Add(e);
			}
            return RedirectPermanent($"/Home/Index/{id}");
        }
		[HttpGet]
		public Command GetCommand(int id)
		{
			return this.Commands.FirstOrDefault(c=>c.command_id == id);
		}

		[NonAction]
		public List<Command> GetCommands()
		{
			List<Command> cmds = new List<Command>();
			string url = "http://178.57.218.210:398/commands/types?token=" + this.token;
			string response = string.Empty;
            using (var wb = new HttpClient())
            {
				try
				{
                    response = wb.GetStringAsync(url).Result;
                }
				catch
				{
					response = string.Empty;
				}
            }
			if(response != string.Empty)
			{
                response = response.Substring(response.IndexOf('['), response.IndexOf(']') - (response.IndexOf('[')) + 1);
                var obj = JsonSerializer.Deserialize<IList<Command>>(response);
                foreach (var cmd in obj)
                {
                    cmds.Add(cmd);
                }
            }
			return cmds;
        }
		[NonAction]
		public async Task<string> SendCommandAsync(string url, object data, string method = "GET")
		{
			using(var  wb = new HttpClient())
			{
				string? str = string.Empty;
				switch (method.ToLower())
				{
					case "get":
						str = await wb.GetAsync(url).Result.Content.ReadAsStringAsync();
						break;
					case "post":
                        str = await wb.PostAsync(url, JsonContent.Create(data)).Result.Content.ReadAsStringAsync();
                        break;
				}
				return str;
			}
		}
	}
}
