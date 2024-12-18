﻿using System.Linq;
using System.Threading.Tasks;
#if NETSTANDARD2_0
using System.Text.Json;
using System.Text.Json.Nodes;

#else
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif

namespace SendGrid.Permissions
{
    /// <summary>
    /// Utility extension methods that bridge the gap between the <see cref="ISendGridClient"/> and the <see cref="SendGridPermissionsBuilder"/>.
    /// </summary>
    public static class SendGridClientExtensions
    {
        /// <summary>
        /// Create a permissions builder instance that masks the emitted scopes to exclude any scopes not
        /// contained in the list of scopes already granted for the given API Key the <paramref name="client"/> was initialized with.
        /// </summary>
        /// <param name="client">The SendGrid client.</param>
        /// <returns>A <see cref="SendGridPermissionsBuilder"/> instance.</returns>
        public static async Task<SendGridPermissionsBuilder> CreateMaskedPermissionsBuilderForClient(this ISendGridClient client)
        {
            var response = await client.RequestAsync(method: SendGridClient.Method.GET, urlPath: "scopes");
            var body = await response.DeserializeResponseBodyAsync();
#if NETSTANDARD2_0
            var userScopesJArray = ((JsonElement)body["scopes"]).EnumerateArray();
            var includedScopes = userScopesJArray.Select(z => z.GetString()).ToArray();
#else
            var userScopesJArray = (body["scopes"] as JArray);
            var includedScopes = userScopesJArray.Values<string>().ToArray();
#endif
            var builder = new SendGridPermissionsBuilder();
            builder.Exclude(scope => !includedScopes.Contains(scope));
            return builder;
        }

        /// <summary>
        /// Create a new API key for the scopes contained in the <paramref name="permissions"/>.
        /// </summary>
        /// <param name="client">The SendGrid client.</param>
        /// <param name="permissions">The permissions builder.</param>
        /// <param name="name">The API key name.</param>
        /// <returns>The <see cref="Response"/> from the SendGrid API call.</returns>
        public static async Task<Response> CreateApiKey(this ISendGridClient client, SendGridPermissionsBuilder permissions, string name)
        {
            var scopes = permissions.Build();
            var payload = new
            {
                name,
                scopes
            };
#if NETSTANDARD2_0
            var data = JsonSerializer.Serialize(payload);
#else
            var data = JsonConvert.SerializeObject(payload);
#endif
            var response = await client.RequestAsync(method: SendGridClient.Method.POST, urlPath: "api_keys", requestBody: data);
            return response;
        }
    }
}
