using System;
using System.Linq;
using UnityEngine;
namespace Assets.Raycasts
{
    public static class IIDRaycast
    {
        public static IIDRaycastResult MouseRaycast(Func<RaycastHit2D, bool> raycastFilter)
        {
            Vector3 mouseScreenPosInWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hits = Physics2D.RaycastAll(mouseScreenPosInWorldPosition, Vector2.zero).Where(raycastFilter);
            bool foundHit = hits.Any();
            IIDRaycastResult rayResult = new IIDRaycastResult()
            {
                HasFoundHit = foundHit,
                FirstResult = foundHit ? hits.First() : null,
                HitObject = foundHit ? hits.First().transform.gameObject : null,
                RaycastResults = hits.ToList(),
            };

            return rayResult;
        }

        // quand meme une fonction critique faque jveux pas faire une cinquantaine de queries par clic de bouton 
        // jpourrais faire un genre de FrameClickCache 
        public static T MouseRaycastScriptOrDefault<T>() where T : MonoBehaviour
        {
            Vector3 mouseScreenPosInWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hits = Physics2D.RaycastAll(mouseScreenPosInWorldPosition, Vector2.zero);

            var scriptHits = hits.FirstOrDefault(x => x.transform.gameObject.GetComponent<T>() is not null);

            foreach (var item in hits) 
            {
                var script = item.transform.gameObject.GetComponent<T>();
                if (script is not null) return script;
            }

            return null;
        }

        public static bool TryGetScript<T>(out T script) where T : MonoBehaviour
        {
            Vector3 mouseScreenPosInWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hits = Physics2D.RaycastAll(mouseScreenPosInWorldPosition, Vector2.zero);

            var validHits = hits.Where(x=> x.transform.gameObject.GetComponent<T>() is not null).ToList();

            bool hasValue = validHits.Any();    
            script = hasValue
                ? validHits.First().transform.gameObject.GetComponent<T>()
                : null;

            return hasValue;
        }

    }
}
