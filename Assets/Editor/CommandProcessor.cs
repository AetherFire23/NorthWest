using Assets.GameLaunch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
namespace Assets.Automation
{
    public class CommandProcessor
    {
        [MenuItem("Customs/Refresh Assets")]
        public static void RefreshAndPlay()
        {
            AssetDatabase.Refresh();
            Thread.Sleep(10);
            EditorApplication.isPlaying = true;
        }

        [MenuItem("Customs/Find missing Dependencies")]
        public static void FindMissingDependencies()
        {
            var gameAssembly = typeof(IRefreshable).Assembly;
            var monos = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().Where(x => x is not null);
            int referencesFixed = 0;
            foreach (MonoBehaviour mono in monos)
            {
                var superAssembly = typeof(IRefreshable).Assembly;
                var nullfields = mono.GetType()
                                     .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                                     .Where(x => x.GetCustomAttribute<SerializeField>() is not null)
                                     .Where(x => x.GetValue(mono) is null)
                                     .Where(x => x.FieldType.Assembly == superAssembly)
                                     .ToList();

                foreach (FieldInfo nullField in nullfields)
                {
                    var referencesFound = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>()
                                                                             .Where(x => x is not null)
                                                                             .Where(x => x.GetType() == nullField.FieldType).ToList();

                    string baseMissingMessage = $"On Gameobject [{mono.transform.gameObject.name}] and the component monobehaviour ${mono.GetType().Name}" +
                        $"{Environment.NewLine}" +

                        $"{Environment.NewLine}Missing:" +
                        $"{Environment.NewLine}" +
                        $"Field Type: {nullField.FieldType.Name}" +
                        $"{Environment.NewLine}" +
                        $"Name: {nullField.Name}." +
                        $"{Environment.NewLine}" +
                        $"{Environment.NewLine}";

                    if (referencesFound.Count == 0)
                    {
                        string missingMessage = $"{baseMissingMessage}However, no monoBehaviour was found in the scene that could fulfill this dependency.";
                        EditorUtility.DisplayDialog("Null reference", missingMessage, "Continue");
                        continue;
                    }

                    string referenceFoundMessage = $"{baseMissingMessage}A monobehaviour was found in the scene that could fulfill this dependency.";

                    if (referencesFound.Count > 1)
                    {
                        string multipleFound = $"{referenceFoundMessage}However, a count of [{referencesFound.Count}] of such depdencies were found. Therefore we will not proceed into fulfilling this dependency";
                        EditorUtility.DisplayDialog("Null reference", multipleFound, "Continue");
                        continue;
                    }

                    string validReferenceFillingMessage = $"{referenceFoundMessage}" +
                        $" Do you want to set it in the editor ?";

                    bool mustFulfillDependency = EditorUtility.DisplayDialog("Null reference", validReferenceFillingMessage, "Ok", "Cancel");

                    if (!mustFulfillDependency)
                    {
                        Debug.Log($"Filling dependency was declined for {nullField.Name}");
                        continue;
                    }

                    nullField.SetValue(mono, referencesFound.First());
                    referencesFixed++;
                }
            }

            if(referencesFixed ==0)
            {
                EditorUtility.DisplayDialog("No changes", "No changes were made to the scene", "Ok");
            }
        }
    }
}
