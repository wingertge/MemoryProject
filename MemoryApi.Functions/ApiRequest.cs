using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace MemoryApi.Functions
{
    public class ApiRequest<T>
    {
        public Dictionary<string, string> Query { get; set; }
        public string AccessToken { get; set; }
        public T Data { get; set; }

        internal ApiRequest()
        {

        }
    }

    public class ApiRequest
    {
        [ItemNotNull]
        public static async Task<ApiRequest<TData>> Create<TData>([NotNull] HttpRequestMessage origin)
        {
            var result = new ApiRequest<TData> { Query = origin.GetQueryStrings() };

            var json = await origin.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<BodyObject<TData>>(json);
            result.AccessToken = jsonObj.AccessToken;
            result.Data = jsonObj.Data;
            return result;
        }

        private class BodyObject<TData>
        {
            public string AccessToken { get; set; }
            public TData Data { get; set; }
        }
    }
}