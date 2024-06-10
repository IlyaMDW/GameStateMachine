using JetBrains.Annotations;

namespace Infrastructure.Services.Saving
{
    public interface IDataSaveable<TSave>
    {
        [NotNull]
        string SaveId { get; }
        
        TSave SaveData { get; set; }

        [NotNull]
        TSave Default { get; }
    }
}