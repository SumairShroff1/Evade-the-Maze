using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum defining different types of boosts available
public enum BoostsTipe { SpeedDown, SpeedUp, Freez }

public class Boosts : MonoBehaviour
{
    // Serialized fields to link boost-related GameObjects in Unity Inspector
    [SerializeField] GameObject speedDown, speedUp, freez;

    // Public variable to store the type of the boost (hidden in the inspector)
    [HideInInspector] public BoostsTipe boostTipe;

    // Start is called before the first frame update
    void Start() {
        // Randomly selects a boost type
        boostTipe = GetRandomEnumValue<BoostsTipe>();

        // Activates the corresponding boost GameObject
        ActiveBoost();
    }

    // Helper method to get a random value from an enum
    private T GetRandomEnumValue<T>() {
        T[] values = (T[])System.Enum.GetValues(typeof(T));
        return values[Random.Range(0, values.Length)];
    }

    // Activates the GameObject associated with the selected boost type
    private void ActiveBoost() {
        if (boostTipe == BoostsTipe.SpeedDown) {
            speedDown.SetActive(true);
        } else if (boostTipe == BoostsTipe.SpeedUp) {
            speedUp.SetActive(true);
        } else {
            freez.SetActive(true);
        }
    }

    // Handles trigger collisions
    private void OnTriggerEnter(Collider other) {
        // Check if the collider belongs to a ghost
        if (other.tag == "Ghost") {
            // Applies the boost effect to the ghost group
            UseBoost(other.transform.parent.parent);
        }
    }

    // Applies the effect of the boost based on its type
    private void UseBoost(Transform _Ghosts) {
        switch (boostTipe) {
            case BoostsTipe.SpeedDown:
                // Reduces player speed by 10%
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().speed *= 0.9f;
                Destroy(this.gameObject); // Removes the boost object from the scene
                break;

            case BoostsTipe.SpeedUp:
                // Increases speed of all enemies in the ghost group
                SpeedUpEnemys(_Ghosts);
                Destroy(this.gameObject);
                break;

            case BoostsTipe.Freez:
                // Stops player movement temporarily
                PlayerMovement PlayerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
                PlayerMovement.ActiveSpeedPlayer(PlayerMovement.speed);

                PlayerMovement.speed = 0;
                Destroy(this.gameObject);
                break;
        }
    }

    // Increases the speed of all enemies in a given ghost group
    private void SpeedUpEnemys(Transform _Ghosts) {
        foreach (Transform item in _Ghosts.transform) {
            Sceleton Sceleton = item.GetComponent<Sceleton>();

            Sceleton.SimpleSpeed *= 1.2f; // Increases normal speed by 20%
            Sceleton.TargetSpeed *= 1.2f; // Increases target speed by 20%

            Sceleton.agent.speed *= 1.2f; // Increases agent's navigation speed by 20%
        }
    }

    // Coroutine to restore player speed after a delay
    IEnumerator ActivePlayer(PlayerMovement _Player, float _Speed) {
        yield return new WaitForSeconds(3f); // Waits for 3 seconds

        _Player.speed = _Speed; // Restores the player's original speed
    }
}
