using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Main : MonoBehaviour
{
    public TMP_Text recordeText;

    private void Start() {
        recordeText.text = "Your recorde: " + PlayerPrefs.GetInt("PlayerScore", 0);
    }
    
    public void Play()
    {
        SceneTransition.SwitchToScene("Maze");
    }
}
