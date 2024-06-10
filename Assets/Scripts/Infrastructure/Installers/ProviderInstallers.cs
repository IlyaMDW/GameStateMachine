using Infrastructure.Providers;
using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Providers.PlayerDataProvider;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ProviderInstallers : MonoInstaller
    {
        [SerializeField] private InEditorConfig inEditorConfig;
        [SerializeField] private AssetReferenceProvider _assetReferenceProvider;
        [SerializeField] private DefaultConfigProvider _defaultConfigProvider;
        [SerializeField] private SpriteLibraryProvider spriteLibraryProvider;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _defaultConfigProvider.OnValidate();
        }
#endif
        public override void InstallBindings()
        {
#if UNITY_EDITOR
            inEditorConfig.RegisterSingleton();
#endif
            BindGameLoopStateMachineProvider();
            BindAssetReferenceProvider();
            BindDefaultConfigProvider();
            BindPlayerDataProvider();
            BindResourceAssetsProvider();
        }

        private void BindPlayerDataProvider()
        {
            Container.BindInterfacesAndSelfTo<PlayerDataProvider>().FromNew().AsSingle();
        }

        private void BindGameLoopStateMachineProvider()
        {
            Container.Bind<GameLoopStateMachineProvider>().FromNew().AsSingle();
        }

        private void BindAssetReferenceProvider()
        {
            Container.Bind<AssetReferenceProvider>().FromInstance(_assetReferenceProvider).AsSingle();
        }

        private void BindDefaultConfigProvider()
        {
            Container.BindInterfacesTo<DefaultConfigProvider>().FromInstance(_defaultConfigProvider).AsSingle();
        }

        private void BindResourceAssetsProvider()
        {
            Container.BindInterfacesTo<SpriteLibraryProvider>().FromInstance(spriteLibraryProvider).AsSingle();
        }
    }
}