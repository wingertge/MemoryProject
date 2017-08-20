using MemoryServer.Core.Business;
using MemoryServer.Core.Business.Impl;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MemoryServer.Tests
{
    public class AssignmentTests
    {
        [TestClass]
        public class ReviewStoreTests
        {
            private readonly IReviewStore _reviewStore;
            public ReviewStoreTests()
            {
                _reviewStore = new ReviewStore(new MemoryDistributedCache(new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions())));
            }

            [TestMethod]
            public void EntityValid()
            {
                var result = JsonConvert.SerializeObject(new ReviewCardContext());
                Assert.AreEqual(result, @"{""Hand"":[],""DiscardPile"":[],""UpcomingQueue"":[]}");
            }
        }
    }
}