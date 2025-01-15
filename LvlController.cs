using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LvlController // LvlController.
{
    public static int lvlNumber = 1,  mapX = 5, mapY = 5, boosters = 1, ghost = 1;

    public static void NextLvl(){
        lvlNumber++;
        mapX++; mapY++;
        
        //if(lvlNumber % 2 == 1) mapX++; else mapY++;

        ghost = 1 + lvlNumber / 3;

        boosters = 1 + Random.Range(0, (mapX / 4)+2);
    }
}
