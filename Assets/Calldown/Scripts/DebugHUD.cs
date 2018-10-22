﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class DebugHUD : MonoBehaviour
{
    [SerializeField]
    private Text systemStatus;

    [SerializeField]
    private Text cameraPosition;

    [SerializeField]
    private Text sessionOriginPosition;

    [SerializeField]
    private Slider scaleSlider;

    [SerializeField]
    private Vector2 scaleRange;

    [SerializeField]
    private Vector2 rotationRange;

    [SerializeField]
    private Text playerScale;

    [SerializeField]
    private Text worldRotation;

    [SerializeField]
    private PlayerCameraAR arCamera;

    [SerializeField]
    private ARSessionOrigin arSessionOrigin;

    void Update()
    {
        systemStatus.text = ARSubsystemManager.systemState.ToString();
        cameraPosition.text = arCamera.transform.position.ToString();
        sessionOriginPosition.text = arSessionOrigin.transform.position.ToString();
        playerScale.text = string.Format("1:{0}", arCamera.worldScale.ToString("0.00"));
        worldRotation.text = string.Format("{0}°", arCamera.worldRotation.ToString("0"));
    }

    public void ScaleSession(float value)
    {
        arCamera.ScaleSession(Mathf.Lerp(scaleRange.x, scaleRange.y, value));
    }

    public void RotateSession(float value)
    {
        arCamera.RotateSession(Mathf.Lerp(rotationRange.x, rotationRange.y, value));
    }
}
