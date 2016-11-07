using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Snake1Movement : Snake
{
    protected override void Start()
    {
        base.Start();
        MoveKeys = MenuScript.Snake1Keys;
    }
    

    
}

