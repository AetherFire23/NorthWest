using Assets.GameLaunch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
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
            var monobehavioursInScene = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().Where(x => x is not null);
            bool hasChangedReferences = false;

            foreach (MonoBehaviour monobehaviour in monobehavioursInScene)
            {
                foreach (FieldInfo nullField in CommandProcessor.GetNullFields(monobehaviour))
                {
                    var referencesFound = GetReferencesInSceneForNullField(nullField);

                    var sb = new StringBuilder();
                    sb.AppendLine($"On Gameobject [{monobehaviour.transform.gameObject.name}] and the component monobehaviour {monobehaviour.GetType().Name}");
                    sb.AppendLine();
                    sb.AppendLine("Missing:");
                    sb.AppendLine($"Field Type: {nullField.FieldType.Name}");
                    sb.AppendLine($"Name: {nullField.Name}.");
                    sb.AppendLine();

                    if (!IsValidReferenceCountOrPrompt(sb, referencesFound.Count)) continue;

                    sb.AppendLine($"A MonoBehaviour was found in the scene that could fulfill this dependency.");
                    sb.AppendLine($"Do you want to set it in the editor ?");

                    bool mustFillDependenciesPrompt = EditorUtility.DisplayDialog("Null reference", sb.ToString(), "Ok", "Cancel");
                    if (!mustFillDependenciesPrompt)
                    {
                        UnityEngine.Debug.Log($"Filling dependency was declined for {nullField.Name}");
                        continue;
                    }

                    nullField.SetValue(monobehaviour, referencesFound.First());
                    hasChangedReferences = true;
                }
            }

            if (!hasChangedReferences)
            {
                EditorUtility.DisplayDialog("No changes", "No changes were made to the scene", "Ok");
            }
        }

        private static bool IsValidReferenceCountOrPrompt(StringBuilder builder, int referenceCount)
        {
            if (referenceCount == 1) return true;

            string overOrUnderMessage = referenceCount > 1
                ? $"However, a count of[{referenceCount}] were found, which is more than one.Therefore, we will not proceed into fulfilling this dependency"
                : $"However, no MonoBehaviour was found in the scene that could fulfill this dependency.";

            builder.AppendLine(overOrUnderMessage);

            EditorUtility.DisplayDialog("Null reference", builder.ToString(), "Continue");
            return false;
        }

        private static List<FieldInfo> GetNullFields(MonoBehaviour mono)
        {
            var superAssembly = typeof(IRefreshable).Assembly;

            var nullFields = mono.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<SerializeField>() is not null)
                .Where(x => x.GetValue(mono) is null)
                .Where(x => x.FieldType.Assembly == superAssembly)
                .ToList();

            return nullFields;
        }

        private static List<MonoBehaviour> GetReferencesInSceneForNullField(FieldInfo nullFieldInfo)
        {
            var references = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>()
                .Where(x => x is not null)
                .Where(x => x.GetType() == nullFieldInfo.FieldType).ToList();
            return references;
        }
    }
}
