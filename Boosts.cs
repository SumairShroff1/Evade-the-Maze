using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoostsTipe{SpeedDown,SpeedUp,Freez }
public class Boosts : MonoBehaviour
{

    [SerializeField] GameObject speedDown, speedUp, freez;
    [HideInInspector] public BoostsTipe boostTipe;

    void Start(){
        boostTipe = GetRandomEnumValue<BoostsTipe>();
        ActiveBoost();
    }

    private T GetRandomEnumValue<T>()
    {
        T[] values = (T[])System.Enum.GetValues(typeof(T));
        return values[Random.Range(0, values.Length)];
    }
    private void ActiveBoost(){
        if(boostTipe == BoostsTipe.SpeedDown){
            speedDown.SetActive(true);
        }else if(boostTipe == BoostsTipe.SpeedUp){
            speedUp.SetActive(true);
        }else{
            freez.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Ghost"){
            UseBoost(other.transform.parent.parent);
        }
    }
    private void UseBoost(Transform _Ghosts){
        switch (boostTipe)
        {
            case BoostsTipe.SpeedDown:
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().speed *= 0.9f;
                Destroy(this.gameObject);

                break;

            case BoostsTipe.SpeedUp:
                SpeedUpEnemys(_Ghosts);
                Destroy(this.gameObject);

                break;

            case BoostsTipe.Freez:
                PlayerMovement PlayerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
                PlayerMovement.ActiveSpeedPlayer(PlayerMovement.speed);

                PlayerMovement.speed = 0;
                Destroy(this.gameObject);

                break;
        }
    }
    private void SpeedUpEnemys(Transform _Ghosts){
        foreach (Transform item in _Ghosts.transform)
        {
            Sceleton Sceleton = item.GetComponent<Sceleton>();

            Sceleton.SimpleSpeed *= 1.2f;
            Sceleton.TargetSpeed *= 1.2f;

            Sceleton.agent.speed *= 1.2f;
        }
    }
    IEnumerator ActivePlayer(PlayerMovement _Player, float _Speed){
        yield return new WaitForSeconds(3f);

        _Player.speed = _Speed;
    }
}
