using System.Collections.Generic;
using Newtonsoft.Json;

namespace MemoryCore
{
    public class ActionResult
    {
        public bool Succeeded { get; set; }
        public List<ActionErrors> Errors { get; set; } = new List<ActionErrors>();
        public int ErrorCode { get; set; }

        [JsonIgnore]
        public string Error
        {
            get => Errors.Count > 0 ? Errors[0].Messages[0] : "";
            set => Errors.Add(new ActionErrors("", value));
        }
    }

    public class ActionResult<T>
    {
        public bool Succeeded { get; set; }
        public T WrappedObject { get; set; }
    }
}