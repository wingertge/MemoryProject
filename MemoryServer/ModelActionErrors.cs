using System.Linq;
using MemoryCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MemoryServer
{
    public class ModelActionErrors : ActionErrors
    {
        public ModelActionErrors(string key, ModelErrorCollection errors)
        {
            Code = key;
            Messages = errors.Select(a => a.ErrorMessage).ToList();
        }
    }
}