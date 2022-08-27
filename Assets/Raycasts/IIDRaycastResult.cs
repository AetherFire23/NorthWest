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

        public IIDRaycastResult(List<RaycastHit2D> raycastResults)
        {
            this.RaycastResults = raycastResults;
            this.HasFoundHit = RaycastResults.Any();

            if(HasFoundHit)
            {
                this.FirstResult = this.RaycastResults.First();
                this.HitObject = this.FirstResult.transform.gameObject;
            }
        }
    }
}
