using Cysharp.Threading.Tasks;
using Shared_Resources.Models.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SSE
{
    public interface IStrategizedMethods<TEnum> where TEnum : Enum
    {
        public Dictionary<TEnum, Func<SSEClientData,UniTask>> EnumDelegates { get; set; }
        public void PopulateEnumDelegates();
    }
}
