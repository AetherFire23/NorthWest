using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.OtherPlayers
{
    public class OtherPlayerInstance : InstanceWrapper<OtherCharacterBehaviour>
    {
        private const string otherCharacterPrefabPath = "someOtherCharacter";
        public global::Player model; // database 

        public OtherPlayerInstance(GameObject parent) : base(otherCharacterPrefabPath, parent)
        {

        }
    }
}
