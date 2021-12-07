using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform PlayerBody;
    public Transform PlayerHead;

    public CharacterController CC;

    public float MaxSpeed;
    public float Acceleration;
    [Range(0, 1)]
    public float AirAccelerationFactor;

    public float Gravity;
    public float StickToGroundGravity;
    public float JumpHeight;

    public Vector2 mouseSens;

    private Vector3 speedFlat;
    private Vector3 speedFall;
    //private bool wasGroundedLastFrame;

    private float CameraRotation = 0;


    public void Update()
    {
        Looking();
        Movement();
    }

    #region Looking

    private void Looking()
    {
        VerticalLooking();
        HorizontalLooking();
    }

    private void VerticalLooking()
    {
        float deltaRotation = Input.GetAxis("Mouse Y") * mouseSens.y;
        CameraRotation += deltaRotation;
        CameraRotation = Mathf.Clamp(CameraRotation, -90, 90);
        PlayerHead.localRotation = Quaternion.Euler(CameraRotation, 0, 0);
    }

    private void HorizontalLooking()
    {
        float deltaRotation = Input.GetAxis("Mouse X") * mouseSens.x;
        PlayerBody.rotation = Quaternion.Euler(new Vector3(0, PlayerBody.rotation.eulerAngles.y + deltaRotation, 0));
    }

    #endregion

    #region Movement

    private void Movement()
    {
        MainMovement();
        ApplyGravity();
        Jump();
        CC.Move((speedFlat + speedFall) * Time.deltaTime);
    }

    private void Jump()
    {
        if (CC.isGrounded && Input.GetButton("Jump"))
        {
            speedFall.y = Mathf.Sqrt(-2 * Gravity * JumpHeight);
        }
    }

    private void ApplyGravity()
    {
        if (CC.isGrounded)
        {
            speedFall.y = Gravity * .2f;
            //wasGroundedLastFrame = true;
        }
        /* else
        {
            if (wasGroundedLastFrame && speedFall.y < 0)
            {
                speedFall.y = Gravity * Time.deltaTime;
            }
            wasGroundedLastFrame = false;
        } */
        speedFall.y += Gravity * Time.deltaTime;
    }

    private void MainMovement()
    {
        Vector3 input = Vector3.zero;
        input += Input.GetAxisRaw("Horizontal") * PlayerBody.right;
        input += Input.GetAxisRaw("Vertical") * PlayerBody.forward;
        input = Vector3.ClampMagnitude(input, 1);

        speedFlat = Vector3.Lerp(speedFlat, input * MaxSpeed, Acceleration * (CC.isGrounded ? 1 : AirAccelerationFactor) * Time.deltaTime);
    }

    #endregion
}
