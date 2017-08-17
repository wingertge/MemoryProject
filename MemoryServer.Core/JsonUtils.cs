using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using MemoryCore;

namespace MemoryServer.Core
{
    public class JsonUtils
    {
        public static async Task<string> SerializeResult<T>(JsonResult<T> result)
        {
            var serializer = new DataContractJsonSerializer(typeof(JsonResult<T>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, result);
                var resultString = await new StreamReader(stream).ReadToEndAsync();
                return resultString;
            }
        }
    }
}