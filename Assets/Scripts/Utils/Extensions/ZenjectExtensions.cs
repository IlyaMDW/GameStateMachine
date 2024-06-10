using UnityEngine;
using Zenject;

namespace Utils.Extensions
{
    public class ZenjectExtensions
    {
        private static DiContainer _projectContextContainer;

        public static DiContainer ProjectContextContainerContainer
        {
            get
            {
                if (_projectContextContainer == null) _projectContextContainer = Object.FindObjectOfType<ProjectContext>().Container;
                return _projectContextContainer;
            }
        }
    }
}