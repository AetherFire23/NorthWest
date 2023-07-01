using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scratch
{
    public static class PersistenceAccess // make extension method instead to make this reusable
    {
        /// <summary>Use unique types. If you need more than one instance, use List</summary>
        public static void SaveData<T>(T data) where T : class, new()
        {
            string savePath = GetPathOf<T>();

            if (File.Exists(savePath))
            {
                Debug.Log($"Data already exists. Will overwrite{savePath}");
                File.Delete(savePath);
            }

            File.WriteAllText(savePath, JsonConvert.SerializeObject(data));

        }

        public static T LoadPersistentData<T>() where T : class, new()
        {
            string savePath = GetPathOf<T>();

            if (!File.Exists(savePath)) throw new Exception("could not load persistent data");

            T data =  GetSerializedDataOrDefault<T>() ?? throw new Exception($"Could not load data for: {typeof(T)} ");


            return null;
        }

        public static T LoadPersistentOrCreate<T>() where T : class, new()
        {
            string savePath = GetPathOf<T>();

            if (!File.Exists(savePath))
            {
                Debug.LogError("Tried to load non-existent file, will create");
                T data = new();
                SaveData<T>(data);
                return data;
            }

            else
            {
                T data = GetSerializedDataOrDefault<T>();
                return data;
            }
        }

        private static T GetSerializedDataOrDefault<T>() where T : class, new()
        {
            string savePath = GetPathOf<T>();

            try
            {
                string jsonData = File.ReadAllText(savePath);
                T data = JsonConvert.DeserializeObject<T>(jsonData);
                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"could not deserialize json data for type {typeof(T)}{ex.Message}");
            }
            return null;
        }

        private static string GetPathOf<T>() where T : class, new()
        {
            string subpath = $"{typeof(T).Name}.json";
            string path = Path.Combine(Application.persistentDataPath, subpath);
            return path;
        }
    }
}
