using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceYeahManager : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        SpawnStuff();
    }

    async UniTask SpawnStuff()
    {
        var pref = Resources.Load("IceYeah");

        bool isOver = false;

        while (!isOver)
        {
            for (float x = -250; x < 500; x += 2.5f)
            {
                for (float y = -250; y < 500; y += 2.5f)
                {
                    GameObject obj = GameObject.Instantiate(pref) as GameObject;
                    obj.transform.position = new Vector3(x, y, 0);
                    var r = obj.GetComponent<SpriteRenderer>();

                    r.sortingOrder = -100;

                  //  await UniTask.Yield();
                }
            }

            isOver = true;
        }
    }
}
