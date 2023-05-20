using Assets;
using Assets.AssetLoading;
using Assets.GameLaunch;
using Assets.Utils;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using Shared_Resources.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OtherPlayersManager : MonoBehaviour, IStartupBehavior, IRefreshable
{
    [SerializeField]
    private PrefabLoader _prefabLoader;

    [SerializeField]
    private RectTransform _otherCharactersContainer;

    private List<OtherCharacterScript> _otherPlayers = new List<OtherCharacterScript>();

    public async UniTask Initialize(GameState gameState)
    {
        var excludeLocalPlayer = gameState.Players.Where(x => x.Id != gameState.PlayerUID).ToList();
        await RefreshCharacters(_otherPlayers, excludeLocalPlayer);
        UpdateTargetPositions(excludeLocalPlayer);

    }

    public async UniTask Refresh(GameState gameState)
    {
        var excludeLocalPlayer = gameState.Players.Where(x => x.Id != gameState.PlayerUID).ToList();
        await RefreshCharacters(_otherPlayers, excludeLocalPlayer);
        UpdateTargetPositions(excludeLocalPlayer);
    }

    public async UniTask<OtherCharacterScript> CreatePlayer(Player player)
    {
        var otherCharacter = await _prefabLoader.CreateInstanceOfAsync<OtherCharacterScript>(_otherCharactersContainer.transform.gameObject);
        await otherCharacter.Initialize(player);
        return otherCharacter;
    }

    public async UniTask RefreshCharacters(List<OtherCharacterScript> old, List<Player> newEntities)
    {
        var result = RefreshResult<OtherCharacterScript, Player>.GetRefreshResult(old, newEntities);

        foreach (var appeared in result.Appeared)
        {
            var go = await _prefabLoader.CreateInstanceOfAsync<OtherCharacterScript>(_otherCharactersContainer.gameObject);
            await go.Initialize(appeared);
            _otherPlayers.Add(go);

        }

        foreach (var disappeared in result.Disappeared)
        {
            _otherPlayers.Remove(disappeared);
            GameObject.Destroy(disappeared.gameObject);
        }
    }

    public void UpdateTargetPositions(List<Player> newEntities)
    {
        foreach (var prefab in _otherPlayers)
        {
            var findNew = newEntities.FirstOrDefault(x => x.Id == prefab.Id);
            prefab.SetTargetPosition(findNew.GetPosition());
        }
    }
}
