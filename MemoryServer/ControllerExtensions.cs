using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MemoryServer.Core.Business.Util
{
    public static class ControllerExtensions
    {
        [NotNull]
        public static Dictionary<string, List<string>> ToErrorDictionary([NotNull] this ModelStateDictionary @this)
        {
            return @this.GroupBy(a => a.Key)
                .ToDictionary(a => a.Key, a => a.First().Value.Errors.Select(b => b.ErrorMessage).ToList());
        }
    }
}