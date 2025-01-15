using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveResult : MonoBehaviour
{
    public GameObject pauseMenuUI, newRecord, loseSound;
    public TMP_Text scoreText, recordeText;

    public void Lost(){ // int _Score
        int Score = LvlController.lvlNumber;

        pauseMenuUI.SetActive(true);
        loseSound.SetActive(true);

        if(PlayerPrefs.GetInt("PlayerScore", 0) < Score){
            newRecord.SetActive(true);
            PlayerPrefs.SetInt("PlayerScore", Score);
            PlayerPrefs.Save();
        }

        scoreText.text = "Your lvl: " + Score;
        recordeText.text = "Your recorde: " + PlayerPrefs.GetInt("PlayerScore");

        GamePause.isPaused = true;
    }
}
