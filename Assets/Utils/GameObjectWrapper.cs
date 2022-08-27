using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameObjectWrapper // appeler ca <<wrapper>>
{
    public MonoBehaviour Behaviour; // this should should be made private readonly and instantiated in inherited class 
    public GameObject GameObject => Behaviour.gameObject;

    public GameObjectWrapper(MonoBehaviour script) // Is injected in inherited class in constructor 
    {
        Behaviour = script;
    }
}