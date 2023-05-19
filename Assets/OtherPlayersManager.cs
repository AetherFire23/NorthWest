using Assets;
using Assets.AssetLoading;
using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using Shared_Resources.Models;
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

    private GameObjectDatabaseRefresher<OtherCharacterScript, Player> _databaseRefresh;

    public async UniTask Initialize(GameState gameState)
    {
        _databaseRefresh = new GameObjectDatabaseRefresher<OtherCharacterScript, Player>(CreatePlayer);
        var excludeLocalPlayer = gameState.Players.Where(x => x.Id == gameState.PlayerUID).ToList();
        await _databaseRefresh.RefreshEntities(excludeLocalPlayer);
    }
    public async UniTask Refresh(GameState gameState)
    {
        var excludeLocalPlayer = gameState.Players.Where(x=> x.Id != gameState.PlayerUID).ToList();
        await _databaseRefresh.RefreshEntities(excludeLocalPlayer);
    }
    public async UniTask<OtherCharacterScript> CreatePlayer(Player player)
    {
        var otherCharacter = await _prefabLoader.CreateInstanceOfAsync<OtherCharacterScript>(_otherCharactersContainer.transform.gameObject);
        await otherCharacter.Initialize(player);

        return otherCharacter;
    }


}
