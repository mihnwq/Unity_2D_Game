using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameStates : MonoBehaviour 
{
    public KeyCode pauseKey;

    public static bool isGamePaused = false;

    public static void ChangeGameState()
    {
        Time.timeScale = (isGamePaused) ? 1 : 0;
        isGamePaused = !isGamePaused;
    }

    public static void ChangeGameStateTo(bool state)
    {
        Time.timeScale = (state) ? 1 : 0;
    }
        
    public void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            ChangeGameState();
    }

}

