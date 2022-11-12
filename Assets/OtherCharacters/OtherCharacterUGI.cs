using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.OtherCharacters
{
    public class OtherCharacterUGI : InstanceWrapper<OtherCharacterAS>, IDbKey
    {
        public Guid Key => DbModel.Id;
        public Player DbModel { get; set; }
        public OtherCharacterUGI(GameObject parent, Player dbModel) : base("otherCharacterPrefab", parent)
        {
            DbModel = dbModel;
            this.AccessScript.TextOverHead.text = dbModel.Name;
        }
    }
}
