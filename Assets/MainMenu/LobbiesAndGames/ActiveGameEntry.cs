using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using Shared_Resources.Interfaces;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MainMenu.LobbiesAndGames
{
    public class ActiveGameEntry : PrefabScriptBase, IEntity
    {
        [SerializeField] private TextMeshProUGUI _roleNameMesh;
        [SerializeField] private TextMeshProUGUI _dateCreated;
        [SerializeField] private Button _button;

        public string RoleName
        {
            get { return _roleNameMesh.text; }
            set { _roleNameMesh.text = value; }
        }

        public DateTime DateCreated
        {
            get
            {
                DateTime.TryParse(_dateCreated.text, out DateTime result);
                return result;
            }
            set { _dateCreated.text = value.ToString("g"); } // smaller i think
        }

        public Guid Id { get; private set; } = Guid.Empty;

        public async UniTask InitializeAsync(GameDto gameDto, RoleType role, Func<UniTask> joinGameFunc)
        {
            Id = gameDto.Id;
            DateCreated = DateTime.UtcNow;
            RoleName = role.ToString();
            _button.AddTaskFunc(joinGameFunc);
        }
    }
}
