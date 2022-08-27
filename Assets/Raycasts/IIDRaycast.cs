using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.RoomTransitioning;
namespace Assets.Raycasts
{
    public static class IIDRaycast
    {
        public static IIDRaycastResult MouseRaycast(Func<RaycastHit2D, bool> raycastFilter)
        {
            Vector3 mouseScreenPosInWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hits = Physics2D.RaycastAll(mouseScreenPosInWorldPosition, Vector2.zero).Where(raycastFilter);

            IIDRaycastResult rayResult = new IIDRaycastResult(hits.ToList());

            return rayResult;
        }
    }
}
