using UnityEngine;
using System.Collections.Generic;
using RiwasGame.Narrative;

namespace RiwasGame.Environment
{
    public class BackgroundController : MonoBehaviour
    {
        public Transform player;
        public float loadDistance = 20f;
        public float unloadDistance = 30f;
        public float preloadDistance = 30f;

        [SerializeField] private List<BackgroundSegment> coreSegmentsList;
        [SerializeField] private List<BackgroundSegment> memorySegmentsList;

        private Dictionary<string, BackgroundSegment> coreSegments = new();
        private Dictionary<string, BackgroundSegment> memorySegments = new();

        public enum SegmentType { Core, Memory }

        [System.Serializable]
        public class BackgroundSegment
        {
            public string segmentId;
            public SegmentType type;
            public GameObject prefab;
            public Vector3 position;
            public List<string> nextSegments;

            // Optional flag to check before allowing load (for branching memories)
            public string requiredFlag; // leave empty if always loadable

            [HideInInspector] public GameObject instance;
            public bool isLoaded => instance != null;
            public bool isPreloaded;
        }

        private float preloadInterval = 0.5f;
        private float lastPreloadTime;

        void Awake()
        {
            foreach (var seg in coreSegmentsList)
                coreSegments[seg.segmentId] = seg;

            foreach (var seg in memorySegmentsList)
                memorySegments[seg.segmentId] = seg;
        }

        void Update()
        {
            HandleSegmentUpdates(coreSegments);
            HandleSegmentUpdates(memorySegments);

            if (Time.time - lastPreloadTime > preloadInterval)
            {
                lastPreloadTime = Time.time;
                StaggeredPreload(coreSegments);
                StaggeredPreload(memorySegments);
            }
        }

        void HandleSegmentUpdates(Dictionary<string, BackgroundSegment> segments)
        {
            foreach (var segment in segments.Values)
            {
                float distance = Vector3.Distance(player.position, segment.position);

                if (!segment.isLoaded && distance <= loadDistance)
                {
                    TryLoadSegment(segment);
                }
                else if (segment.isLoaded && distance >= unloadDistance)
                {
                    UnloadSegment(segment);
                }
            }
        }

        void StaggeredPreload(Dictionary<string, BackgroundSegment> segments)
        {
            List<(float, BackgroundSegment)> preloadCandidates = new();

            foreach (var segment in segments.Values)
            {
                if (!segment.isLoaded && !segment.isPreloaded)
                {
                    if (!string.IsNullOrEmpty(segment.requiredFlag) &&
                        !MemoryManager.Instance.GetFlag(segment.requiredFlag))
                        continue; // skip if flag not set

                    float distance = Vector3.Distance(player.position, segment.position);
                    if (distance <= preloadDistance)
                        preloadCandidates.Add((distance, segment));
                }
            }

            preloadCandidates.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            int count = Mathf.Min(preloadCandidates.Count, 2);

            for (int i = 0; i < count; i++)
            {
                PreloadSegment(preloadCandidates[i].Item2);
            }
        }

        void TryLoadSegment(BackgroundSegment segment)
        {
            if (!string.IsNullOrEmpty(segment.requiredFlag) &&
                !MemoryManager.Instance.GetFlag(segment.requiredFlag))
                return;

            LoadSegment(segment);
        }

        void LoadSegment(BackgroundSegment segment)
        {
            segment.instance = Instantiate(segment.prefab, segment.position, Quaternion.identity);
            segment.isPreloaded = true;
        }

        void UnloadSegment(BackgroundSegment segment)
        {
            if (segment.instance != null)
                Destroy(segment.instance);

            segment.instance = null;
            segment.isPreloaded = false;
        }

        void PreloadSegment(BackgroundSegment segment)
        {
            segment.instance = Instantiate(segment.prefab, segment.position, Quaternion.identity);
            segment.instance.SetActive(false);
            segment.isPreloaded = true;
        }

        public void ActivateSegment(string segmentId, SegmentType type)
        {
            var dict = type == SegmentType.Core ? coreSegments : memorySegments;
            if (dict.TryGetValue(segmentId, out var segment) && segment.instance != null)
            {
                segment.instance.SetActive(true);
            }
        }

        public void LoadNextSegments(string currentId, SegmentType type)
        {
            var dict = type == SegmentType.Core ? coreSegments : memorySegments;
            if (!dict.TryGetValue(currentId, out var current)) return;

            foreach (var id in current.nextSegments)
            {
                if (!dict.TryGetValue(id, out var next)) continue;

                // Check required flag
                if (!string.IsNullOrEmpty(next.requiredFlag) &&
                    !MemoryManager.Instance.GetFlag(next.requiredFlag))
                    continue;

                if (!next.isLoaded)
                    LoadSegment(next);
            }

            MemoryManager.Instance.MarkMemoryVisited(currentId);
        }
    }
}