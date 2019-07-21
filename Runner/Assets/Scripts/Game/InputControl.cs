using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.EventSystem;

public class InputControl : MonoSingleton<InputControl>
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.Notify(this, new GameEventArgs(Events.InputEvents.BACK_BUTTON_PRESSED));
        }
    }
}
