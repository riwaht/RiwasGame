using UnityEngine;

namespace StaticBloom.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetWalking(bool isWalking)
        {
            animator.SetBool("IsWalking", isWalking);
        }

        public void SetRunning(bool isRunning)
        {
            animator.SetBool("IsRunning", isRunning);
        }

        public void SetJumping(bool isJumping)
        {
            animator.SetBool("IsJumping", isJumping);
        }

        public void SetHanging(bool isHanging)
        {
            animator.SetBool("IsHanging", isHanging);
        }

        public void SetDucking(bool isDucking)
        {
            animator.SetBool("IsDucking", isDucking);
        }

        public void SetVerticalSpeed(float ySpeed)
        {
            animator.SetFloat("VerticalSpeed", ySpeed);
        }

        public void PlayOneShot(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
