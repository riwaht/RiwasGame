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
                // Uncomment for debugging
                // Debug.Log($"[Anim] {param} set to: {value}");
            }
        }

        public void SetBool(string param, bool value)
        {
            if (animator.GetBool(param) != value)
            {
                animator.SetBool(param, value);
                // Uncomment for debugging
                // if (value)
                //     Debug.Log($"[Anim] {param} set to true");
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
        // landing is a trigger, so it should be set only when landing occurs
        public void SetLanding() => SetTrigger("Landing");
        public void SetDucking(bool value) => SetBool("isDucking", value);
        public void SetSliding(bool value) => SetBool("isSliding", value);

        public void SetPushing(bool value) => SetBool("isPushing", value);
        public void SetPulling(bool value) => SetBool("isPulling", value);

        public void SetHanging(bool value) => SetBool("isHanging", value);
        public void SetShimmying(bool value) => SetBool("isShimmying", value);
        public void SetClimbingLedge(bool value) => SetBool("isClimbingLedge", value);
        public void SetClimbingLadder(bool value) => SetBool("isClimbingLadder", value);
        // public void SetDeath(bool value) => SetBool("isDead", value);
    }
}