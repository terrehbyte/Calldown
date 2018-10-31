using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Animator anim;

    private Vector3 lastPosition;

    public float health;

    void Start()
    {
        navMeshAgent.destination = Vector3.zero;
    }

    void FixedUpdate()
    {
        float speed = ((transform.position - lastPosition) / Time.deltaTime).magnitude;

        lastPosition = transform.position;

        anim.SetFloat("Speed", speed);
    }

    public float TakeDamage(float damageDealt)
    {
        health -= damageDealt;

        if(health <= 0.0f)
        {
            OnDeath();
        }

        return damageDealt;
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
