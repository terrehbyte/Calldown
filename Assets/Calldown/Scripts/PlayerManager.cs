using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject arController;
    public GameObject desktopController;

    public bool forceAR;

    public void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if(Application.isEditor && !forceAR)
        {
            GameRoot.Instantiate(desktopController);
        }
        else
        {
            GameRoot.Instantiate(arController);
        }
    }
}
