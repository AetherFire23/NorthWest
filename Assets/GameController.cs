using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class GameController : IInitializable, ITickable // je pourrais mettre le gamecontroller dans un monobehaviour pour utiliser les coroutines sincerement. , juste changer le constrcuteur pis tada 
{
    private readonly PlayerScript _playerFacade;
    //Si je remplaces les coroutines par du async, je peux tout remettre dans le controller
    public GameController(
        PlayerScript playerFacade)
    {
        _playerFacade = playerFacade;
        
    }

    public void Initialize()
    {
    }

    public void Tick()
    {
    }

    public void InitializeLocalPlayer()
    {


    }

    public void InitializeOtherCharacters()
    {

    }

    public void UpdateLocalPlayerPosition()
    {

    }

    public void GetOtherPlayersPositions()
    {

    }
}
