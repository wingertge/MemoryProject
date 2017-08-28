using System.Collections.Generic;

namespace MemoryApi.Core.DbModels
{
    public class ListUpdateTrackerSet : DbModel
    {
        public string UserId { get; set; }
        public List<ListUpdateTracker> UpdateTrackers { get; set; }
    }
}