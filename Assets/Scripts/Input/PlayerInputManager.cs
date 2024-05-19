using Card_Management;
using UnityEngine;

namespace Input
{
    public class PlayerInputManager : MonoBehaviour
    {
        private void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.MatchManager == null) return;
            if (GameManager.Instance.IsGamePaused()) return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                mouseWorldPos.z = 0;
                GameManager.Instance.MatchManager.CheckForCardClick(mouseWorldPos);
            }
        }
    }
}
