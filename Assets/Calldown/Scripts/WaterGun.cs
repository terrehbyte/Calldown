using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class WaterGun : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private ARSession arSession;

    [SerializeField]
    private ARSessionOrigin arSessionOrigin;

    private List<ARRaycastHit> lastHits = new List<ARRaycastHit>();

    [SerializeField]
    private PlayerCameraAR arCamera;

    void Update()
    {
        if (!arCamera.readyToPlace) { return; }

        if(Input.touchCount > 1)
        {
            Vector3 center = cam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.0f));

            arSessionOrigin.Raycast(center, lastHits);

            if(lastHits.Count == 0) { return; }

            arCamera.MoveSession(lastHits[0].pose.position);
        }
    }
}
