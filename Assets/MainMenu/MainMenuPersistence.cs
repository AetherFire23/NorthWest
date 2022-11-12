using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
