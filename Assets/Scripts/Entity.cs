using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity Variables")]
    public int health = 1;
    [HideInInspector] public GameHandler gameHandler;
    // VFXDeath
    // VFXHit
    private void Awake()
    {
        gameHandler = GameHandler.Instance;
    }
    public void TakeDamage(int damageToTake)
    {
        health -= damageToTake;
        //Update Health Bar (if any)
        if (health <= 0)
        {
            //Play Death VFX
            Death();
        }
        //Play Hit VFX
    }
    public void Death()
    {
        // how does the entity die
        GameHandler.Instance.EndGame(false);
    }
}
