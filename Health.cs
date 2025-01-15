using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart, emptyHeart;
    // public MaterialController material;
    public int health = 3;
    public float immunity = 2f;
    public SaveResult saveResult;
    public Player player;

    private bool iCanTakeDamage = true;

    private void Start() 
    {
        // if(!material){GameObject.FindWithTag("Player").GetComponent<MaterialController>();}
        ChekHert();
    }
    public void GetDamage(){
        if(!iCanTakeDamage){
            return;
        }
        health--;
        ChekHert();

        if(health <= 0){
            saveResult.Lost();
        }else{
            StartCoroutine(Immunity());
        }
    }
    // public void GetFullDamage(){
    //     health = 0;
    //     ChekHert();

    //     Debug.Log("ГГ");
    // }
    IEnumerator Immunity(){
        iCanTakeDamage = false;
        // material.Flicker(true);

        yield return new WaitForSeconds(immunity);

        // material.Flicker(false);
        yield return new WaitForSeconds(0.2f);
        iCanTakeDamage = true;
    }
    public void ChekHert(){
        for (int i = 0; i < 3; i++){
            if(health > i){
                hearts[i].sprite = fullHeart;
            }else{
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
