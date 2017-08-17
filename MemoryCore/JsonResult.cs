using MemoryCore.DbModels;

namespace MemoryCore
{
    public class JsonResult<T>
    {
        public T Result { get; set; }

        public JsonResult() { }

        public JsonResult(T result)
        {
            Result = result;
        }
    }
}