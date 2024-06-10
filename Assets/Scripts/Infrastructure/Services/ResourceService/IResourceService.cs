using System;

namespace Infrastructure.Services.ResourceService
{
    public interface IResourceService
    {
        public event Action OnAnyUpdated;
        public event Action<long> OnGoldUpdated;
        public event Action<long> OnDiamondsUpdated;

        public long Gold { get; set; }
        public long MaxGold { get; }
        public long Diamonds { get; set; }
    }
}