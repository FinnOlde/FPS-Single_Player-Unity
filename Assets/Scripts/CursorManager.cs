using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public bool CaptureCursor;

    void Update()
    {
        Cursor.lockState = CaptureCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
