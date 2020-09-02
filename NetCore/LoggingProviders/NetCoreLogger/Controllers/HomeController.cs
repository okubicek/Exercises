using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreLogger.Models;

namespace NetCoreLogger.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly Executor _executor;

		public HomeController(ILogger<HomeController> logger, Executor executor)
		{
			_logger = logger;
			_executor = executor;
		}

		public IActionResult Index()
		{
			_logger.LogInformation("Homepage visited {asat}", DateTime.Now);
			return View();
		}

		public IActionResult Privacy()
		{
			using (_logger.BeginScope(new Dictionary<string, object> { { "RandomId", new Random().Next() } }))
			{
				_logger.LogInformation("Privacy page visited {asat}", DateTime.Now);
				_executor.Execute();
			}
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			_logger.LogError("Privacy page visited {traceId}", HttpContext.TraceIdentifier);
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
