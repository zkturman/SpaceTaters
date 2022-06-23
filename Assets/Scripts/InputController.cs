using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputController
{
    public static KeyCode[] LeftKeyCodes = new KeyCode[2] { KeyCode.LeftArrow, KeyCode.A };
    public static KeyCode[] RightKeyCodes = new KeyCode[2] { KeyCode.RightArrow, KeyCode.D };

    public static bool IsKeyInGroupPressed(IEnumerable<KeyCode> keyCodeGroup)
    {
        bool groupCodeIsPressed = false;
        foreach (KeyCode code in keyCodeGroup)
        {
            if (Input.GetKey(code))
            {
                groupCodeIsPressed = true;
            }
        }
        return groupCodeIsPressed;
    }
}
