using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using Xunit;

namespace Miscellaneous.ObjectToUri
{
	public class ObjectToUriTest
	{
		[Fact]
		public void ObjectShouldConvertToExpectedQueryString()
		{
			var toSerialize = new
			{
				DateCreated = new DateTime(2020, 12, 12),
				DateExecued = new DateTimeOffset(2020, 12, 13, 12, 12, 01, new TimeSpan()),
				RoleIds = new List<int> { 3, 2 },
				Name = "Ondrej"
			};

			var dict = toSerialize.ToKeyValue();
			var res = QueryHelpers.AddQueryString("https://something.com", dict);

			Assert.Equal("https://something.com?DateCreated=2020-12-12T00%3A00%3A00.0000000&DateExecued=2020-12-13T12%3A12%3A01.0000000%2B00%3A00&RoleIds%5B0%5D=3&RoleIds%5B1%5D=2&Name=Ondrej", res);
		}
	}
}
