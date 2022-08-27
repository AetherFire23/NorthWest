using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class PlayerInputState
{
    public Vector2 TargetPosition { get; set; }
    public Vector2 ClickMousePosition { get; set; }
    public bool IsMouseClicked { get; set; }
    public bool IsMovingLeft { get; set; }
    public bool IsMovingRight { get; set; }
    public bool IsMovingUp { get; set; }
    public bool IsMovingDown { get; set; }
    public bool IsMovingUpLeft { get; set; }
    public bool IsMovingUpRight { get; set; }
    public bool IsMovingDownLeft { get; set; }
    public bool IsMovingDownRight { get; set; }
}
