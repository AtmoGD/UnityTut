using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject character;
    [SerializeField] Transform cameraTransform;


    [Header("Movement")]
    [SerializeField] float velocity = 1800f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float movementThreshold = 1f;


    [Header("Gravity")]
    [SerializeField] float gravity = -200f;
    [SerializeField] float groundedGravity = -1f;
    float Gravity
    {
        get
        {
            return characterController.isGrounded ? groundedGravity : gravity;
        }
    }


    [Header("Jumping")]
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpTime = 0.25f;


    Vector3 desiredMovement = Vector3.zero;
    Quaternion desiredRotation = Quaternion.identity;
    float turnSmoothVelocity;
    bool isJumping = false;
    float jumpStartTime;

    void Update()
    {
        ApplyInputs();
    }

    void FixedUpdate()
    {
        if (desiredMovement.magnitude > movementThreshold)
            ApplyMoveInput();

        if (isJumping)
            ApplyJumpInput();
        else
            ApplyGravity();

        RotateCharacter();

        MoveCharacter();
    }

    void ApplyInputs()
    {
        desiredMovement = InputController.GetMovementInput();

        if (InputController.GetJumpInput())
        {
            isJumping = true;
            jumpStartTime = Time.time;
        }
    }

    void ApplyMoveInput()
    {
        float targetAngle = Mathf.Atan2(desiredMovement.x, desiredMovement.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        desiredRotation = Quaternion.Euler(0, angle, 0);

        desiredMovement = Quaternion.Euler(0, angle, 0) * Vector3.forward * velocity * Time.fixedDeltaTime;
    }

    void ApplyJumpInput()
    {
        float jumpTimeProgress = (Time.time - jumpStartTime) / jumpTime;
        float jumpHeightProgress = jumpCurve.Evaluate(jumpTimeProgress);

        desiredMovement.y = jumpHeight * jumpHeightProgress;

        if (jumpTimeProgress >= 1 || (characterController.isGrounded && desiredMovement.y < 0))
            isJumping = false;
    }

    void ApplyGravity()  => desiredMovement.y += Gravity * Time.fixedDeltaTime;

    void RotateCharacter() => character.transform.rotation = desiredRotation;

    void MoveCharacter() => characterController.Move(desiredMovement);
}