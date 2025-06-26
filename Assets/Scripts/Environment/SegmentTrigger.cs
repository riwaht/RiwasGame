using UnityEngine;

namespace RiwasGame.Environment
{
    public class SegmentTrigger : MonoBehaviour
    {
        public string currentSegmentId;
        public BackgroundController.SegmentType segmentType;
        public BackgroundController backgroundController;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            backgroundController.LoadNextSegments(currentSegmentId, segmentType);
        }
    }
}
