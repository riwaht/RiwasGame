// New modular class to track animation state changes only when needed
using UnityEngine;

namespace RiwasGame.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetFloat(string param, float value)
        {
            float current = animator.GetFloat(param);
            if (!Mathf.Approximately(current, value))
            {
                animator.SetFloat(param, value);
                Debug.Log($"[Anim] {param} set to: {value}");
            }
        }

        public void SetBool(string param, bool value)
        {
            if (animator.GetBool(param) != value)
            {
                animator.SetBool(param, value);
                if (value)
                    Debug.Log($"[Anim] {param} set to true");
            }
        }

        public void SetTrigger(string param)
        {
            animator.SetTrigger(param);
            Debug.Log($"[Anim] Triggered: {param}");
        }

        // Convenience wrappers for consistent param usage
        public void SetWalking(bool value) => SetBool("isWalking", value);
        public void SetRunning(bool value) => SetBool("isRunning", value);
        public void SetJumping(bool value) => SetBool("isJumping", value);
        public void SetFalling(bool value) => SetBool("isFalling", value);
        public void SetLanding(bool value) => SetBool("isLanding", value);
        public void SetDucking(bool value) => SetBool("isDucking", value);
        public void SetSliding(bool value) => SetBool("isSliding", value);

        public void SetPushing(bool value) => SetBool("isPushing", value);
        public void SetPulling(bool value) => SetBool("isPulling", value);

        public void SetHanging(bool value) => SetBool("isHanging", value);
        public void SetShimmying(bool value) => SetBool("isShimmying", value);
        public void SetClimbingLedge(bool value) => SetBool("isClimbingLedge", value);
        public void SetClimbingLadder(bool value) => SetBool("isClimbingLadder", value);

        public void TriggerStartWalking() => SetTrigger("StartWalking");
        public void TriggerStopWalking() => SetTrigger("StopWalking");
        public void TriggerStartRunning() => SetTrigger("StartRunning");
        public void TriggerStopRunning() => SetTrigger("StopRunning");

        public void TriggerStartPush() => SetTrigger("StartPush");
        public void TriggerStopPush() => SetTrigger("StopPush");
        public void TriggerStartPull() => SetTrigger("StartPull");
        public void TriggerStopPull() => SetTrigger("StopPull");

        public void TriggerStartHanging() => SetTrigger("StartHanging");
        public void TriggerStopHanging() => SetTrigger("StopHanging");

        public void TriggerStartClimbingLedge() => SetTrigger("StartClimbingLedge");
        public void TriggerStopClimbingLedge() => SetTrigger("StopClimbingLedge");
        public void TriggerStartClimbingLadder() => SetTrigger("StartClimbingLadder");
        public void TriggerStopClimbingLadder() => SetTrigger("StopClimbingLadder");

        public void TriggerLand() => SetTrigger("Land");
    }
}