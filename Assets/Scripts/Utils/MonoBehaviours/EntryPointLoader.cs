using Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.MonoBehaviours
{
    public class EntryPointLoader : MonoBehaviour
    {
        public static bool IsGameStartedNotFromEntryPoint;

        private void Awake()
        {
            if (EntryPoint.IsAwakened)
                Destroy(gameObject);
            else
            {
                IsGameStartedNotFromEntryPoint = true;
                SceneManager.LoadScene(0);
            }
        }
    }
}