using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scratch
{
    public class PlayerDataPersistenceOptions : MonoBehaviour // should jsut be tempoptionscript
    {
        [SerializeField] private bool _mustOverrideSavedData;

        [SerializeField] private string jsonPersistenceModel;
        public PersistenceModel PersistedData { get; private set; }

        void Awake()
        {
            if (_mustOverrideSavedData)
            {
                PersistedData = JsonConvert.DeserializeObject<PersistenceModel>(jsonPersistenceModel);
            }

            else
            {
                PersistedData = PersistenceAccess.LoadPersistentData<PersistenceModel>();
            }
            Debug.Log("Loaded Persistence data successfully");
        }
    }
}
