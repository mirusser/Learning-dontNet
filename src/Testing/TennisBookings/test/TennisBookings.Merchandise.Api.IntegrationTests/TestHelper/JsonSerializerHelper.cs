using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TennisBookings.Merchandise.Api.IntegrationTests.TestHelper
{
    public static class JsonSerializerHelper
    {
        public static JsonSerializerOptions DefaultSerialisationOptions => new()
        {
            IgnoreNullValues = true
        };

        public static JsonSerializerOptions DefaultDeserialisationOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
