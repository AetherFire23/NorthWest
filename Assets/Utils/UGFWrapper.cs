//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

///// <summary>
///// The script must be installed as a ZenJect Binding while using the UGFWrapper
///// Also, the class that inherits the wrapper must be installed in the gameinstaller
///// </summary>
//public abstract class UGFWrapper // appeler ca <<wrapper>>
//{
//    public MonoBehaviour AccessScript; // this should should be made private readonly and instantiated in inherited class 
//    public GameObject GameObject => AccessScript.gameObject;

    
//    public UGFWrapper(MonoBehaviour script) // Is injected in inherited class in constructor 
//    {
//        AccessScript = script;
//    }
//}