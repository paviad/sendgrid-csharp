﻿using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Xunit;

namespace SendGrid.Tests
{
    public class ResponseTests
    {
        [Fact]
        public async Task DeserializeResponseBodyAsync_NullHttpContent_ReturnsEmptyDictionary()
        {
            var response = new Response(HttpStatusCode.OK, null, null);
            Dictionary<string, dynamic> responseBody = await response.DeserializeResponseBodyAsync();
            Assert.Empty(responseBody);
        }

        [Fact]
        public async Task DeserializeResponseBodyAsync_JsonHttpContent_ReturnsBodyAsDictionary()
        {
            var content = "{\"scopes\": [\"alerts.read\"]}";
            var response = new Response(HttpStatusCode.OK, new StringContent(content), null);
            Dictionary<string, dynamic> responseBody = await response.DeserializeResponseBodyAsync();
            var actual = responseBody["scopes"];
            var success = actual is JsonElement { ValueKind: JsonValueKind.Array } m &&
                          m.EnumerateArray().Single() is { ValueKind: JsonValueKind.String } m2 &&
                          m2.GetString() == "alerts.read";
            Assert.True(success, "Result is not [\"alerts.read\"]");
        }

        [Fact]
        public async Task DeserializeResponseBodyAsync_OverrideHttpContent_ReturnsBodyAsDictionary()
        {
            var content = "{\"scopes\": [\"alerts.read\"]}";
            var response = new Response(HttpStatusCode.OK, null, null);
            Dictionary<string, dynamic> responseBody = await response.DeserializeResponseBodyAsync(new StringContent(content));
            var actual = responseBody["scopes"];
            var success = actual is JsonElement { ValueKind: JsonValueKind.Array } m &&
                          m.EnumerateArray().Single() is { ValueKind: JsonValueKind.String } m2 &&
                          m2.GetString() == "alerts.read";
            Assert.True(success, "Result is not [\"alerts.read\"]");
        }

        [Fact]
        public void DeserializeResponseHeaders_NullHttpResponseHeaders_ReturnsEmptyDictionary()
        {
            var response = new Response(HttpStatusCode.OK, null, null);
            Dictionary<string, string> responseHeadersDeserialized = response.DeserializeResponseHeaders();
            Assert.Empty(responseHeadersDeserialized);
        }

        [Fact]
        public void DeserializeResponseHeaders_HttpResponseHeaders_ReturnsHeadersAsDictionary()
        {
            var message = new HttpResponseMessage();
            message.Headers.Add("HeaderKey", "HeaderValue");
            var response = new Response(HttpStatusCode.OK, null, message.Headers);
            Dictionary<string, string> responseHeadersDeserialized = response.DeserializeResponseHeaders();
            Assert.Equal("HeaderValue", responseHeadersDeserialized["HeaderKey"]);
        }

        [Fact]
        public void DeserializeResponseHeaders_OverrideHttpResponseHeaders_ReturnsHeadersAsDictionary()
        {
            var message = new HttpResponseMessage();
            message.Headers.Add("HeaderKey", "HeaderValue");
            var response = new Response(HttpStatusCode.OK, null, null);
            Dictionary<string, string> responseHeadersDeserialized = response.DeserializeResponseHeaders(message.Headers);
            Assert.Equal("HeaderValue", responseHeadersDeserialized["HeaderKey"]);
        }
    }
}
