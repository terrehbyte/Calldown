using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerStandalone : MonoBehaviour
{
    [SerializeField]
    private PlayerLocomotionFly locomotion;

    [SerializeField]
    private PlayerGun[] guns;

    [SerializeField]
    private bool lockCursor = false;

    void Start()
    {
        AttemptCursorLock();
    }

    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            AttemptCursorLock();
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

        if(Input.GetMouseButton(0))
        {
            AttemptCursorLock();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        foreach (var gun in guns)
        {
            gun.isFiring = Input.GetButton("Fire1");
        }
    }

    private void AttemptCursorLock()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
