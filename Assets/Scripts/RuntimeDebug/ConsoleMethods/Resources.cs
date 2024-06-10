using Infrastructure.Services.ResourceService;
using IngameDebugConsole;
using Utils.Extensions;

namespace RuntimeDebug.ConsoleMethods
{
    public static class Resources
    {
        [ConsoleMethod("resource.set-g", "sets gold"), UnityEngine.Scripting.Preserve]
        public static void SetGold(long newGold)
        {
            ZenjectExtensions.ProjectContextContainerContainer.Resolve<ResourceService>().Gold += newGold;
        }

        [ConsoleMethod("resource.set-d", "sets diamonds"), UnityEngine.Scripting.Preserve]
        public static void SetDiamonds(long newDiamonds)
        {
            ZenjectExtensions.ProjectContextContainerContainer.Resolve<ResourceService>().Diamonds += newDiamonds;
        }

        [ConsoleMethod("resource.set-endless-g-and-d", "sets energy"), UnityEngine.Scripting.Preserve]
        public static void EndlessGoldAndDiamonds()
        {
            var resourceService = ZenjectExtensions.ProjectContextContainerContainer.Resolve<ResourceService>();

            resourceService.Gold = resourceService.MaxGold;
            resourceService.Diamonds = 999999;

            resourceService.OnAnyUpdated += OnResourceServiceOnOnAnyUpdated;

            void OnResourceServiceOnOnAnyUpdated()
            {
                resourceService.OnAnyUpdated -= OnResourceServiceOnOnAnyUpdated;
                resourceService.Gold = resourceService.MaxGold;
                resourceService.Diamonds = 999999;
                resourceService.OnAnyUpdated += OnResourceServiceOnOnAnyUpdated;
            }
        }
    }
}