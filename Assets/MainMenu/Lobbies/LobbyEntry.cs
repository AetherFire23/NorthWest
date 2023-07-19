using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MainMenu.Lobbies
{
    public class LobbyEntry : PrefabScriptBase, IEntity
    {
        public Guid Id { get;}

        public async UniTask Initialize(LobbyDto lobby)
        {

        }
    }
}
