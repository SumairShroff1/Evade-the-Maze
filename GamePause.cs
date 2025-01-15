using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        LvlController.lvlNumber = 1; 
        LvlController.mapX = 5; 
        LvlController.mapY = 5; 
        LvlController.boosters = 1; 
        LvlController.ghost = 1;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Main()
    {
        Resume();

        SceneTransition.SwitchToScene("Main");
    }
    public void Again()
    {
        Resume();
        
        SceneTransition.SwitchToScene("Maze");
    }
}
