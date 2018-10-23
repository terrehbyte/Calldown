using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class PlayerCameraAR : MonoBehaviour
{
    [SerializeField]
    private ARSessionOrigin arSessionOrigin;

    [SerializeField]
    private Transform worldRoot;

    private float _worldScale = 1.0f;
    public float worldScale
    {
        get
        {
            return _worldScale;
        }
        set
        {
            ScaleSession(_worldScale);
        }
    }

    private float _worldRotation = 0.0f;
    public float worldRotation
    {
        get
        {
            return _worldRotation;
        }
        set
        {
            RotateSession(_worldRotation);
        }
    }

    private Vector3 worldFakeLocation;


    public void Start()
    {
        ScaleSession(_worldScale);
        ARSubsystemManager.systemStateChanged += OnSystemStateChanged;
        SystemStateChanged(ARSubsystemManager.systemState);
    }

    private void OnSystemStateChanged(ARSystemStateChangedEventArgs obj)
    {
        SystemStateChanged(obj.state);
    }
    private void SystemStateChanged(ARSystemState newState)
    {
        worldRoot.gameObject.SetActive(newState == ARSystemState.SessionTracking);

    }

    public void ScaleSession(float value)
    {
        _worldScale = value;
        arSessionOrigin.transform.localScale = Vector3.one * _worldScale;

        arSessionOrigin.MakeContentAppearAt(worldRoot, worldFakeLocation);
    }

    public void RotateSession(float value)
    {
        _worldRotation = value;

        arSessionOrigin.MakeContentAppearAt(worldRoot, Quaternion.AngleAxis(_worldRotation, Vector3.up));
    }

    public void MoveSession(Vector3 newLocation)
    {
        worldFakeLocation = newLocation;
        arSessionOrigin.MakeContentAppearAt(worldRoot, worldFakeLocation);
    }
}
