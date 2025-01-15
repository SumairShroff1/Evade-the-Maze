using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int score;
    public TMP_Text scoreText;
    public Health health;
    public Animator endAnimation;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject enemys;


    private void Start() {
        scoreText.text = "lvl: " + LvlController.lvlNumber;
    }
    // private void SpawnSceletons(){
    //     int Rd = Random.Range(0, points.childCount);

    //     Instantiate(sceleton, points.GetChild(Rd).position, Quaternion.identity).SetActive(true);
    // }

    private void OnTriggerEnter(Collider other) {
        // if(other.tag == "Flag"){
        //     AddScore();
        //     ChangeFlagLocation(other.transform.parent.gameObject);
        // }
        if(other.tag == "Win"){
            LvlController.NextLvl();
            endAnimation.SetTrigger("Win");
        }
        if(other.tag == "Damage"){
            health.GetDamage();
        }
        if(other.tag == "Coin"){
            UseBoost(other.GetComponent<Boosts>().boostTipe);
            Destroy(other.gameObject);
        }
    }
    private void UseBoost(BoostsTipe _Boost){
        switch (_Boost)
        {
            case BoostsTipe.SpeedDown:
                SpeedDownEnemys();
                break;

            case BoostsTipe.SpeedUp:
                playerMovement.speed *= 1.29f;
                break;

            case BoostsTipe.Freez:
                FreezEnemy();
                break;
        }
    }
    private void SpeedDownEnemys(){
        foreach (Transform  item in enemys.transform)
        {
            Sceleton Sceleton = item.GetComponent<Sceleton>();

            Sceleton.SimpleSpeed *= 0.8f;
            Sceleton.TargetSpeed *= 0.8f;

            Sceleton.agent.speed *= 0.8f;
        }
    }
    private void FreezEnemy(){
        foreach (Transform  item in enemys.transform)
        {
            Sceleton Sceleton = item.GetComponent<Sceleton>();
            StartCoroutine(ActiveEnemy(Sceleton, Sceleton.SimpleSpeed, Sceleton.TargetSpeed, Sceleton.agent.speed));

            Sceleton.SimpleSpeed = 0f;
            Sceleton.TargetSpeed = 0f;

            Sceleton.agent.speed = 0f;
        }
    }
    IEnumerator ActiveEnemy(Sceleton _Sceleton, float S, float T, float A){
        yield return new WaitForSeconds(4f);

        _Sceleton.SimpleSpeed = S;
        _Sceleton.TargetSpeed = T;
        _Sceleton.agent.speed = A;
    }


    // private void AddScore(){
    //     score++;
    //     scoreText.text = "Score: " + score;

    //     if(score > 70){
    //         return;
    //     }

    //     if(score % 2 == 0){
    //         SpawnSceletons();
    //     }
    // }
    // private void ChangeFlagLocation(GameObject _Flag){
    //     int Rd = Random.Range(0, points.childCount);

    //     GameObject FlagPoint = points.GetChild(Rd).gameObject;

    //     if(FlagPoint.name == "Used"){
    //         ChangeFlagLocation(_Flag);
    //     }else{
    //         FlagPoint.name = "Used";

    //         int LastParsedIndex; 
    //         if (int.TryParse(_Flag.name, out LastParsedIndex))
    //         {
    //             if(LastParsedIndex >= 0){
    //                 points.GetChild(LastParsedIndex).gameObject.name = "Point"; 
    //             }
    //         }
    //         _Flag.name = Rd + "";
    //         _Flag.transform.position = FlagPoint.transform.position;
    //     }
    // }
}
