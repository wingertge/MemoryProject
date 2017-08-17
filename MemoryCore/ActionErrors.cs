using System.Collections.Generic;
using System.Linq;

namespace MemoryCore
{
    public class ActionErrors
    {
        public string Code { get; set; }
        public List<string> Messages { get; set; }

        public ActionErrors() { }

        public ActionErrors(string code, List<string> messages)
        {
            Code = code;
            Messages = messages;
        }

        public ActionErrors(string code, params string[] messages)
        {
            Code = code;
            Messages = messages.ToList();
        }
    }
}