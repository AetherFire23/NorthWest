using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MainMenu.Authentication
{
    public class BooleanEventBinder
    {
        public bool Value { get; private set; }
        private List<Func<bool, UniTask>> _tasks = new List<Func<bool, UniTask>>();

        public BooleanEventBinder(bool value)
        {
            Value = value;
        }

        public async UniTask AddOnChangeEvent(Func<bool, UniTask> unitask) // think () => needed
        {
            _tasks.Add(unitask);
        }

        public async UniTask SetValue(bool value)
        {
            Value = value;
            await InvokeEvents();
        }

        public async UniTask Toggle()
        {
            Value = !Value;
            await InvokeEvents();
        }

        public async UniTask InvokeEvents()
        {
            foreach (var task in _tasks)
            {
                await task.Invoke(Value);
            }
        }
    }
}
