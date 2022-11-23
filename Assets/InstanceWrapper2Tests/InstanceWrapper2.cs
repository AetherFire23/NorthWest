//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets.Utils
//{
//    public class InstanceWrapper2<TAccessScript> where TAccessScript : AccessScript2<InstanceWrapper2<TAccessScript>>
//    {
//        public GameObject UnityInstance;
//        public TAccessScript AccessScript;

//        private GameObject _prefab;
//        private string _resourceName;

//        /// This constructor instantiates the unity instance under the given gameobject 
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        public InstanceWrapper2(string resourceName, GameObject parent)
//        {
//            _resourceName = resourceName;
//            _prefab = LoadPrefab();
//            UnityInstance = CreateInstanceUnderParent(_prefab, parent);
//            AccessScript = UnityInstance.GetComponentSafely<TAccessScript>();
//            this.AccessScript.SelfWrapper = this;
//        }

//        ~InstanceWrapper2()
//        {
//            this.UnityInstance.SelfDestroy();
//        }

//        private GameObject LoadPrefab()
//        {
//            return UnityExtensions.LoadPrefabSafely(_resourceName) as GameObject;
//        }

//        private GameObject CreateInstanceUnderParent(GameObject toInstantiate, GameObject parent)
//        {
//            var instantiatedPrefab = GameObject.Instantiate(toInstantiate, parent.transform);
//            return instantiatedPrefab;
//        }
//    }
//}
