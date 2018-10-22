using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionFly : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rbody;

    private Vector3 moveWish = Vector3.zero;
    private Vector3 turnWish = Vector3.zero;

    public float moveSpeed = 3.0f;
    public float turnSpeed = 180.0f;

    public bool isSprinting = false;
    public float sprintMultiplier = 4.0f;

    public void MovePosition(Vector3 newPosition)
    {
        rbody.position = (newPosition - rbody.position);
    }

    public void MoveUp(float value)
    {
        moveWish.y += value;
    }

    public void MoveRight(float value)
    {
        moveWish.x += value;
    }

    public void MoveForward(float value)
    {
        moveWish.z += value;
    }

    public void Move(Vector3 value)
    {
        moveWish += value;
    }

    public void TurnRotation(Quaternion newRotation)
    {
        throw new System.NotImplementedException();
    }

    public void Turn(Quaternion delta)
    {
        throw new System.NotImplementedException();
    }

    public void Turn(Vector3 delta)
    {
        turnWish += delta;
    }

    public void TurnYaw(float yaw)
    {
    }

    public void TurnRoll(float roll)
    {
        throw new System.NotImplementedException();
    }

    public void TurnPitch(float pitch)
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdate()
    {
        float frameMoveSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1.0f);
        float frameTurn = turnSpeed * Time.deltaTime;

        rbody.velocity = transform.TransformDirection(moveWish) * frameMoveSpeed;

        rbody.rotation = Quaternion.AngleAxis(turnWish.y * frameTurn, Vector3.up);
        rbody.rotation *= Quaternion.AngleAxis(turnWish.x * frameTurn, Vector3.right);
        rbody.rotation *= Quaternion.AngleAxis(turnWish.z * frameTurn, Vector3.forward);

        moveWish = Vector3.zero;
    }
}
