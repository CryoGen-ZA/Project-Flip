using System;
using System.Collections;
using System.Collections.Generic;
using Card_Management;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.MatchManager == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            GameManager.Instance.MatchManager.CheckForCardClick(mouseWorldPos);
        }
    }
}
