using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Snake2Movement : Snake
{
    protected override void Start()
    {
        base.Start();
        MoveKeys = MenuScript.Snake2Keys;
    }

   
}


