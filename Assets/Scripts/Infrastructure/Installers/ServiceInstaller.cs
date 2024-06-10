using Infrastructure.Services.Analytics;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using Infrastructure.Services.ResourceService;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private UnityConditionalLoggingService _unityConditionalLoggingService;

        public override void InstallBindings()
        {
            BindConditionalLoggingService();
            BindCoroutineRunnerService();
            BindSceneLoaderService();
            BindAnalytics();
            BindSaveService();
            BindResourceService();
        }


        private void BindConditionalLoggingService()
        {
            Container.Bind<IConditionalLoggingService>().To<UnityConditionalLoggingService>().FromInstance(_unityConditionalLoggingService).AsSingle();
        }

        private void BindCoroutineRunnerService()
        {
            Container.Bind<ICoroutineRunnerService>().To<CoroutineRunnerService>().FromNewComponentOnNewGameObject().AsSingle();
        }

        private void BindSceneLoaderService()
        {
            Container.BindInterfacesTo<SceneLoaderService>().FromNew().AsSingle();
        }

        private void BindResourceService()
        {
            Container.BindInterfacesAndSelfTo<ResourceService>().FromNew().AsSingle();
        }

        private void BindAnalytics()
        {
            Container.BindInterfacesTo<FirebaseAnalyticsLogService>().FromNew().AsSingle();
        }

        private void BindSaveService()
        {
            Container.BindInterfacesTo<JsonSaveService>().FromNew().AsSingle();

            //Container.BindInterfacesTo<BinarySaveService>().FromNew().AsSingle();
        }
    }
}