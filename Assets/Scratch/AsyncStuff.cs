using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AsyncStuff : MonoBehaviour
{
    [SerializeField] private int _count;
    // Start is called before the first frame update
    async void Start()
    {
        await VeryLongMethod();
    }

    public async UniTask VeryLongMethod()
    {
        int i = 0;
        while (i < 20)
        {
            //await UniTask.Delay(2000);
            await CountAsync();
            await UniTask.Yield();
            i++;
            Debug.Log($"Just finished lolzida{i}");

        }

    }

    public async UniTask<int> CountAsync()
    {
        await UniTask.SwitchToThreadPool();
        for (int i = 0; i < _count; i++)
        {

        }
        await UniTask.SwitchToMainThread();
        return 3;
    }
}
