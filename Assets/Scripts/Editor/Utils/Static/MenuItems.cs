using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils.Extensions;
using Utils.MonoBehaviours;
using Zenject;

namespace Editor.Utils.Static
{
    public static class MenuItems
    {
        [MenuItem("Assets/Project/To Entry Point", false, 7)]
        private static void ToEntryPoint()
        {
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
        }

        [MenuItem("Assets/Project/Select Project Context", false, 5)]
        private static void SelectProjectContext()
        {
            Selection.activeObject = Resources.Load<ProjectContext>("ProjectContext");
        }

        [MenuItem("Assets/Project/Select Config Utility", false, 8)]
        private static void SelectConfigUtility()
        {
            Selection.activeObject = Resources.Load<ConfigUtility>("ConfigUtility");
        }

        [MenuItem("Tools/Open PersistentDataPath", false, 8)]
        private static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Tools/ResetAllUnusedRaycastTargets", false, 7)]
        private static void ResetAllUnusedRaycastTargets()
        {
            var maskableGraphics = Object.FindObjectsOfType<MaskableGraphic>();

            var overridableRequesters = new List<GraphicRequester>(10);

            foreach (var graphic in maskableGraphics)
            {
                if (graphic.TryGetComponent<GraphicRequester>(out var requester))
                {
                    if (requester.OverrideAllChildrensMasksToThis) overridableRequesters.Add(requester);
                }

                graphic.raycastTarget = false;
                graphic.maskable = false;
            }

            for (var index = 0; index < overridableRequesters.Count; index++)
            {
                var requester = overridableRequesters[index];
                var graphics = requester.GetComponentsInChildren<MaskableGraphic>();
                for (var i = 0; i < graphics.Length; i++)
                {
                    var graphic = graphics[i];
                    graphic.maskable = requester.Maskable;
                }
            }
        }
    }
}