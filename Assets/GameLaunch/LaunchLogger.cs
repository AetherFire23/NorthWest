using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
namespace Assets.GameLaunch
{
    public static class LaunchLogger
    {
        public static async UniTask WarnForMissingSerializables()
        {
            var gameObjects = GameObject.FindObjectsOfType<GameObject>().ToList();

            foreach (var gameObject in gameObjects)
            {
                var monoBehaviours = gameObject.GetComponents<MonoBehaviour>().Where(x => x is not null).ToList();

                foreach (var monoBehaviour in monoBehaviours)
                {
                    var fieldWithAttribute = monoBehaviour.GetType()
                     .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                     .Where(x => x.GetCustomAttribute<SerializeField>() is not null)
                     .ToList();

                    var nulls = fieldWithAttribute.Where(x => x.GetValue(monoBehaviour) is null).ToList();

                    foreach (var nullField in nulls)
                    {
                        // Reflection finds some default unity gameobjects that are not actually valid in-game
                        if (nullField.FieldType.Assembly != Assembly.GetExecutingAssembly()) continue;
                        Debug.LogError($"Gameobject {gameObject.name} has monobehaviour ${monoBehaviour} with null field ${nullField.Name} with type {nullField.FieldType.Name}");
                    }

                }
            }
        }
    }
}
