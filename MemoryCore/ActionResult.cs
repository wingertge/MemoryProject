using System.Collections.Generic;
using Newtonsoft.Json;

namespace MemoryCore
{
    public class ActionResult
    {
        public bool Succeeded { get; set; }
        public List<(string Key, string Value)> Errors { get; set; } = new List<(string, string)>();
        public int ErrorCode { get; set; }

        [JsonIgnore]
        public string Error
        {
            get => Errors.Count > 0 ? Errors[0].Value : "";
            set => Errors.Add(("", value));
        }
    }

    public class ActionResult<T>
    {
        public bool Succeeded { get; set; }
        public T WrappedObject { get; set; }
    }
}