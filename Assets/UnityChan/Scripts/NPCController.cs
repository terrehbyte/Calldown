using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float detectionRadius = 5.0f;

    public float health = 100.0f;
    public bool isDead { get{ return health <= 0.0f;} }

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private int maxDetections = 10;
    private Collider[] detectedColliders;

    private bool inDanger = false;

    private NPCController()
    {
        detectedColliders = new Collider[maxDetections];
    }

    private void OnDeath()
    {
        anim.SetBool("isDead", true);
    }

    private void Update()
    {
        if(health <= 0.0f)
        {
            OnDeath();
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        int results = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, detectedColliders);

        bool wasInDanger = inDanger;

        for(int i = 0; i < results; ++i)
        {
            var result = detectedColliders[i];
            if(result.GetComponent<EnemyController>())
            {
                inDanger = true;
                break;
            }
        }

        if(inDanger != wasInDanger) { anim.SetBool("inDanger", inDanger); }

        return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isDead) { return; }

        if(collision.gameObject.GetComponent<EnemyController>())
        {
            Destroy(collision.gameObject);
            health -= 10.0f;
            anim.SetTrigger("hurt");
        }
    }
}
