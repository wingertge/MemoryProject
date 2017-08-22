using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MemoryCore
{
    public class ActionResult
    {
        public bool Succeeded { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
        public int ErrorCode { get; set; }

        [JsonIgnore]
        public string Error
        {
            get => Errors.Count > 0 ? Errors.ToList()[0].Value[0] : "";
            set
            {
                if(Errors[""] == null) Errors[""] = new List<string>();
                Errors[""].Add(value);
            }
        }
    }

    public class ActionResult<T>
    {
        public bool Succeeded { get; set; }
        public T WrappedObject { get; set; }
    }
}