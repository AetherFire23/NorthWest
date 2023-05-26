using System;
using UnityEngine;

namespace Assets.MainMenu
{
    [CreateAssetMenu(fileName = "fuck", menuName = "bigdick/energy")]
    public class MainMenuPersistence : ScriptableObject
    {

        public Guid MainPlayerId;
        [SerializeField]
        public string receivedId;
    }
}
