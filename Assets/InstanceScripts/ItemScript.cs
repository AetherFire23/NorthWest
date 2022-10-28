using Assets.Inventory.Player_Item;
using Assets.Inventory.Slot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    public ItemUGI selfWrapper;
    // Start is called before the first frame update
    void Start()
    {

       var b=  this.GetComponent<Button>();
       

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TestCLICKBOI()
    {
        Debug.Log("yeahboi");
    }
}
