using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public partial class Dialog2<TScript> : InstanceWrapper<TScript> // where t == le script ou la facade
{
    public bool IsShowing;
    public DialogResult DialogResult;
    
    public Dialog2(string resourceName, GameObject parent) : base(resourceName, parent)
    {

    }
}
