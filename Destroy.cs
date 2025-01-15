using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public void Destrooy(){
        Destroy(transform.parent.gameObject);
    }
    public void TpToMaze(){
        SceneTransition.SwitchToScene("Maze");
    }
}
