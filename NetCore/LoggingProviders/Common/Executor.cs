using Microsoft.Extensions.Logging;
using System;

namespace Common
{
	public class Executor
	{
		private ILogger _logger;

		public Executor(ILogger<Executor> logger)
		{
			_logger = logger;
		}

		public void Execute()
		{
			try
			{
				throw new NotImplementedException("Well we don't do a thing here");
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Executor failed. {randomParam}", "random");
			}
		}
	}
}
