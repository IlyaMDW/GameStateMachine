using System;

namespace Infrastructure.Providers.PlayerDataProvider
{
    public interface IPlayerDataProvider
    {
        public event Action<int> OnGoldChangedByEntity;
        public event Action<int> OnDiamondsChangedByEntity;
        public event Action<long> OnExperienceUpdated;
    }
}