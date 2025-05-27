using UnityEngine;
using RiwasGame.Player;

namespace RiwasGame.Utils
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private float overshootDistance = 1f;
        [SerializeField] private float overshootReturnSpeed = 2f;

        private Vector3 targetPosition;
        private Vector3 velocity = Vector3.zero;
        private float previousPlayerX;

        private void Awake()
        {
            if (player == null)
            {
                player = GameObject.FindObjectOfType<PlayerController>()?.transform;
            }

            previousPlayerX = player != null ? player.position.x : 0f;
        }

        private void LateUpdate()
        {
            if (player == null) return;

            float deltaX = player.position.x - previousPlayerX;
            previousPlayerX = player.position.x;

            // Determine direction: 1 for right, -1 for left, 0 for idle
            float direction = Mathf.Sign(deltaX);

            // When moving, overshoot ahead of player slightly
            if (Mathf.Abs(deltaX) > 0.01f)
            {
                targetPosition = new Vector3(
                    player.position.x + direction * overshootDistance,
                    transform.position.y,
                    transform.position.z
                );
            }
            else
            {
                // Return to player's position when idle (centered)
                targetPosition = new Vector3(
                    player.position.x,
                    transform.position.y,
                    transform.position.z
                );
            }

            // Smooth camera movement
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity,
                direction != 0 ? 1f / followSpeed : 1f / overshootReturnSpeed);
        }
    }
}
