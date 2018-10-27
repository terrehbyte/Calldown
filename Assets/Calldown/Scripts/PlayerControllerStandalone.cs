using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerStandalone : MonoBehaviour
{
    [SerializeField]
    private PlayerLocomotionFly locomotion;

    [SerializeField]
    private PlayerGun[] guns;

    private bool lockCursor = false;

    void Start()
    {
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            Vector3 posDelta = new Vector3(Input.GetAxisRaw("Horizontal"),
                                        Input.GetAxisRaw("Hover"),
                                        Input.GetAxisRaw("Vertical"));

            locomotion.Move(posDelta);

            Vector3 rotDelta = new Vector3(-Input.GetAxisRaw("Mouse Y"),
                                            Input.GetAxisRaw("Mouse X"),
                                            Input.GetAxisRaw("Roll"));

            locomotion.Turn(rotDelta);
            locomotion.isSprinting = Input.GetButton("Sprint");
        }

        foreach(var gun in guns)
        {
            gun.isFiring = Input.GetButton("Fire1");
        }
    }
}
