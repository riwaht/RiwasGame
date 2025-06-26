using UnityEngine;
using System.Collections.Generic;

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

        [HideInInspector] public GameObject instance;
        public bool isLoaded => instance != null;
        public bool isPreloaded;
    }

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
    }

    void HandleSegmentUpdates(Dictionary<string, BackgroundSegment> segments)
    {
        foreach (var pair in segments)
        {
            var segment = pair.Value;
            float distance = Vector3.Distance(player.position, segment.position);

            if (!segment.isLoaded && distance <= loadDistance)
            {
                LoadSegment(segment);
            }
            else if (segment.isLoaded && distance >= unloadDistance)
            {
                UnloadSegment(segment);
            }
            else if (!segment.isPreloaded && distance <= preloadDistance)
            {
                PreloadSegment(segment);
            }
        }
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
        // Optionally use pooling here
        segment.instance = Instantiate(segment.prefab, segment.position, Quaternion.identity);
        segment.instance.SetActive(false); // Preload but keep hidden
        segment.isPreloaded = true;
    }

    public void ActivateSegment(string segmentId, SegmentType type)
    {
        var dict = type == SegmentType.Core ? coreSegments : memorySegments;
        if (dict.TryGetValue(segmentId, out var segment) && segment.instance != null)
        {
            segment.instance.SetActive(true); // Show preloaded content
        }
    }

    public void LoadNextSegments(string currentId, SegmentType type)
    {
        var dict = type == SegmentType.Core ? coreSegments : memorySegments;
        if (!dict.TryGetValue(currentId, out var current)) return;

        foreach (var id in current.nextSegments)
        {
            if (dict.TryGetValue(id, out var next) && !next.isLoaded)
            {
                LoadSegment(next);
            }
        }
    }
}
