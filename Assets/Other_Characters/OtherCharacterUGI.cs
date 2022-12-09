using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.OtherCharacters
{
    public class OtherCharacterUGI : InstanceWrapper<OtherCharacterAS>, IEntity
    {
        public Guid Id => DbModel.Id;
        public Player DbModel { get; set; }
        public OtherCharacterUGI(GameObject parent, Player dbModel) : base("otherCharacterPrefab", parent)
        {
            DbModel = dbModel;
            this.AccessScript.TextOverHead.text = dbModel.Name;
            this.AccessScript.ProfessionText.text = dbModel.Profession.ToString();
            this.AccessScript.SelfWrapper = this;
        }
    }
}
