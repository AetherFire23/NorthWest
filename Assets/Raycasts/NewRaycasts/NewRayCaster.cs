using Assets.Input_Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Raycasts.NewRaycasts
{
    public class NewRayCaster // ZenJected
    {
        private readonly NewInputManager _newInputManager;
        public NewRayCaster(NewInputManager newInputManager)
        {
            _newInputManager = newInputManager;
        }

        public IIDRaycastResult PointerPhysicsRaycast(Func<RaycastHit2D, bool> raycastFilter)
        {
            RaycastHit[] rayhits;

            Vector3 mouseScreenPosInWorldPosition = Camera.main.ScreenToWorldPoint(_newInputManager.PointerPosition);

            var hits = Physics2D.RaycastAll(mouseScreenPosInWorldPosition, Vector2.zero).Where(raycastFilter).ToList();
            bool foundHit = hits.Any();
            IIDRaycastResult newRayCastResult = new()
            {
                HasFoundHit = foundHit,
                FirstResult = hits.FirstOrDefault(),
                HitObject = foundHit ? hits.FirstOrDefault().transform.gameObject : null,
                RaycastResults = hits,
            };

            return newRayCastResult;
        }

        public IIDRaycastResult PointerPhysicsRaycast<TScript>() where TScript: MonoBehaviour // Utilise le fait quun component est attached ou non.
        {
            RaycastHit[] rayhits;

            Vector3 mouseScreenPosInWorldPosition = Camera.main.ScreenToWorldPoint(_newInputManager.PointerPosition);

            var hits = Physics2D.RaycastAll(mouseScreenPosInWorldPosition, Vector2.zero).Where(x => x.transform.gameObject.GetComponent<TScript>() is not null).ToList();
            bool foundHit = hits.Any();
            IIDRaycastResult newRayCastResult = new()
            {
                HasFoundHit = foundHit,
                FirstResult = hits.FirstOrDefault(),
                HitObject = foundHit ? hits.FirstOrDefault().transform.gameObject : null,
                RaycastResults = hits,
                script = foundHit ? hits.FirstOrDefault().transform.gameObject.GetComponent<TScript>() : null,
            };
            return newRayCastResult;
        }

        public UIRaycastResult PointerUIRayCast(Func<RaycastResult, bool> filter)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = _newInputManager.PointerPosition;
            List<RaycastResult> raycastResult = new List<UnityEngine.EventSystems.RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResult);
            raycastResult = raycastResult.Where(filter).ToList();

            bool foundHit = raycastResult.Any();

            var newResult = new UIRaycastResult()
            {
                GameObject = foundHit ? raycastResult.FirstOrDefault().gameObject : null,
                raycastResults = raycastResult,
                HasFoundHit = foundHit,
                FirstResult = foundHit ? raycastResult.FirstOrDefault() : new RaycastResult()
            };
            return newResult;
        }
    }
}
