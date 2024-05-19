using Card_Management;
using UnityEngine;

namespace Input
{
    public class PlayerInputManager : MonoBehaviour
    {
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.MatchManager == null) return;
            if (GameManager.Instance.IsGamePaused()) return;

            if (!UnityEngine.Input.GetMouseButtonDown(0)) return; //Using MouseButtonDown for time constraints and it works for mobile
            
            var mouseWorldPos = _cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            mouseWorldPos.z = 0;
            GameManager.Instance.MatchManager.CheckForCardClick(mouseWorldPos);
        }
    }
}
