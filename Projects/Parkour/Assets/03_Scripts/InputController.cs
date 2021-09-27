using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputState {
    public float x;
    public float z;
    public bool jump;
}

public static class InputController
{
    public static InputState InputData {
        get {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            bool jump = Input.GetButtonDown("Jump");
            return new InputState { x = x, z = z, jump = jump };
        }
    }
    public static Vector3 GetMovementInput() {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    public static bool GetJumpInput() {
        return Input.GetButtonDown("Jump");
    }
}
