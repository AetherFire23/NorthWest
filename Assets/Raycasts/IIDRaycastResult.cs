using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Raycasts
{
    public class IIDRaycastResult
    {
        public List<RaycastHit2D> RaycastResults { get; set; }
        public bool HasFoundHit { get; set; }
        public RaycastHit2D FirstResult { get; set; }
        public GameObject HitObject { get; set; }

        public MonoBehaviour script { get; set; }
    };
}

