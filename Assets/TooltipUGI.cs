using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class TooltipUGI : InstanceWrapper<ToolTipResizer>
    {
        public TooltipUGI(GameObject parent, TooltipInfo tooltipInfo) : base("Tooltip", parent)
        {
            this.AccessScript.HeaderText.text = tooltipInfo.Header;
            this.AccessScript.BodyText.text = tooltipInfo.Body;
        }
    }
}
