using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scratch
{
    public class ScriptScratch : MonoBehaviour, IStartupBehavior
    {
        [SerializeField] DialogManager _dialogManager;
        [SerializeField] Transform _dropDownTransform;
        [SerializeField] TMPro.TMP_Dropdown _dropDownComponent;
        [SerializeField] private TaskBuilder _taskBuilder;
        //[SerializeField] Text 


        public async UniTask Initialize(GameState gameState)
        {
            //var li = Enumerable.Range(0, 10).ToList();
            //var options = await _dialogManager.CreateOptionsDialog(li);
            //await options.WaitForResolveCoroutine();

            //var targets = options.GetSelections<int>();
            //await options.Destroy();

            //List<GameTaskBase> init = typeof(GameTaskBase).Assembly.GetTypes()
            //    .Where(x => x.IsClass && !x.IsAbstract && typeof(GameTaskBase).IsAssignableFrom(x))
            //    .Select(x => Activator.CreateInstance(x) as GameTaskBase).ToList();

            //var firstTask = init.First();
            //await _taskBuilder.ConstructTaskAction(gameState, firstTask);

           
        }

        public async UniTask AskForStuffAsync()
        {
            var pre = await _dialogManager.CreateDialog<MessageBox>();
            await pre.Initialize("test");
            await pre.WaitForResolveCoroutine();
            //await _dialogManager.DeleteDialog(pre);
            await pre.Destroy();
        }

        private void Update()
        {
            //if (!Input.GetMouseButtonDown(0)) return;

            //var roomInventory = UIRaycast.TagExists("RoomInventory");

            //Debug.Log(roomInventory);
        }
    }
}
