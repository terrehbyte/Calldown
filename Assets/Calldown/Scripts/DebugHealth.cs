using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHealth : MonoBehaviour, IDamageable
{
    public float health;

    public float TakeDamage(float damageDealt)
    {
        health -= damageDealt;

        if(health < 0.0f)
        {
            Destroy(gameObject);
        }

        return damageDealt;
    }
}
