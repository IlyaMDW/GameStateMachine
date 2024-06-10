using UI.SpriteLibrary;

namespace Infrastructure.Providers
{
    public interface ISpriteLibraryProvider
    {
        string DiamondsIcon { get; }
        string GoldIcon { get; }
        SpriteLibrary SpriteLibrary { get; }
    }
}