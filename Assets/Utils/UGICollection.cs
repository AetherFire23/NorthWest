//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets.Utils
//{
//    public class UGICollection<TUGI, TAccessScript> where TUGI : InstanceWrapper<TAccessScript> // implement with IUGICollection
//    {
//        public ReadOnlyCollection<TUGI> UGIs => _uGIs.AsReadOnly();

//        private List<TUGI> _uGIs = new();

//        public UGICollection()
//        {
//        }


//        public void RemoveAndDestroy(TUGI ugi)
//        {
//            var ugiInList = _uGIs.First(x => object.ReferenceEquals(x, ugi));
//            ugiInList.UnityInstance.SelfDestroy();
//            _uGIs.Remove(ugiInList);
//        }

//        public void Clear()
//        {
//            _uGIs.ForEach(x => x.UnityInstance.SelfDestroy());
//            _uGIs.Clear();
//        }

//        public void RemoveMany(List<TUGI> remove)
//        {
//            foreach (var ugi in remove)
//            {
//                RemoveAndDestroy(ugi);
//            }
//        }

//        public bool Contains(TUGI ugi)
//        {
//            var ugiInList = _uGIs.FirstOrDefault(x => object.ReferenceEquals(x, ugi));
//            return ugiInList is not null;
//        }

//        public TUGI Add(TUGI ugi)
//        {
//            _uGIs.Add(ugi);
//            return ugi;
//        }

//        public void AddMany(List<TUGI> ugis)
//        {
//            _uGIs.AddRange(ugis);
//        }    
//    }
//}
