using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LvlStart : MonoBehaviour
{
    [SerializeField] private TMP_Text infoOnStart;
    [SerializeField] private MazeSpawner mazeSpawner;

    private void Start() {
        infoOnStart.text = $"lvl: {LvlController.lvlNumber} \nBoosts: {LvlController.boosters} \nGhosts: {LvlController.ghost} \nMap: {LvlController.mapX}x{LvlController.mapY}";

        if(mazeSpawner != null) mazeSpawner.SpawnMaze(LvlController.mapX, LvlController.mapY, LvlController.boosters);
    }
}
