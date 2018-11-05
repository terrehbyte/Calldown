using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerGun : MonoBehaviour
{
    private bool _isFiring;
    public bool isFiring
    {
        get
        {
            return _isFiring;
        }
        set
        {
            bool change = _isFiring != value;

            _isFiring = value;

            if(change) { OnIsFiringChanged.Invoke(value); }
        }
    }

    public float fireInterval = 0.3f;
    public float fireTimer { get; protected set; }

    public UnityEventBool OnIsFiringChanged = new UnityEventBool();    // TODO: duplicate this as delegate to shave on performance
    public UnityEvent OnShotFired = new UnityEvent();             // TODO: duplicate this as delegate to shave on performance

    public int tracerInterval = 1;

    [SerializeField]
    private ParticleSystem fireParticles;

    [SerializeField]
    private GameObject hitEffect;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] fireSounds;

    private int fireCount = 0;

    public Vector3 targetLocation { get; private set; }
    public bool aimEnabled = false;
    public LayerMask targetingMask;

    private Quaternion initialRotation = Quaternion.identity;
    private float rateOfReturn = 90.0f;

    [SerializeField]
    private Transform playerCam;

    [SerializeField]
    private bool aimDisableOverride = true;

    protected virtual void OnFiringStart()
    {
        fireTimer = 0.0f;
    }

    protected virtual void OnFiringExit()
    {

    }

    protected virtual void OnFiringStay()
    {
        Vibration.Vibrate(100);
        OnShotFired.Invoke();
        audioSource.PlayOneShot(fireSounds[Random.Range(0, fireSounds.Length)]);
        fireCount++;

        if(fireCount % tracerInterval == 0)
        {
            //fireParticles.Emit(1);
        }

        var hits = Physics.RaycastAll(playerCam.position, playerCam.forward, Mathf.Infinity, targetingMask);
        aimEnabled = hits.Length > 0;

        if(hits.Length < 1) { return; }

        int closestIndex = -1;
        float closestDistance = Mathf.Infinity;
        for(int i = 0; i < hits.Length; ++i)
        {
            if(hits[i].distance < closestDistance)
            {
                closestIndex = i;
                closestDistance = hits[i].distance;
            }
        }
        var closestHit = hits[closestIndex];
        //var baby = Instantiate(hitEffect, closestHit.point, Quaternion.identity);
        //baby.transform.up = closestHit.normal;

        var damaged = closestHit.collider.GetComponent<IDamageable>();
        if(damaged != null)
        {
            damaged.TakeDamage(10.0f);
        }
    }

    private void InternalOnFireHandler(bool newFireState)
    {
        if(newFireState) { OnFiringStart(); }
        else             { OnFiringExit(); }
    }

    protected virtual void Awake()
    {
        OnIsFiringChanged.AddListener(InternalOnFireHandler);
        //initialRotation = transform.localRotation;
    }

    protected virtual void Update()
    {
        if(isFiring)
        {
            fireTimer += Time.deltaTime;
            if(fireTimer >= fireInterval)
            {
                fireTimer = 0.0f;

                OnFiringStay();
            }
        }

        // todo: process hits

        var hits = Physics.RaycastAll(playerCam.position, playerCam.forward, Mathf.Infinity, targetingMask);
        aimEnabled = hits.Length > 0;

        if(aimEnabled && !aimDisableOverride)
        {
            targetLocation = hits[0].point;
            transform.LookAt(targetLocation);
        }
        else
        {
            return;
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                        initialRotation,
                                                        rateOfReturn * Time.deltaTime );
        }
    }

    protected virtual void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }
}