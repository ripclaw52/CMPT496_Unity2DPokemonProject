using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : State<GameController>
{
    public static PauseState i { get; private set; }
    private void Awake()
    {
        i = this;
    }
}
