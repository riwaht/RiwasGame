using System.Collections.Generic;
using UnityEngine;

namespace RiwasGame.Narrative
{
    public class MemoryManager : MonoBehaviour
    {
        public static MemoryManager Instance { get; private set; }

        private HashSet<string> visitedMemories = new();
        private Dictionary<string, bool> memoryFlags = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // === Visited Memories ===

        public void MarkMemoryVisited(string memoryId)
        {
            visitedMemories.Add(memoryId);
        }

        public bool HasVisited(string memoryId)
        {
            return visitedMemories.Contains(memoryId);
        }

        // === Flags (Decisions, Unlocks, State) ===

        public void SetFlag(string flagKey, bool value)
        {
            memoryFlags[flagKey] = value;
        }

        public bool GetFlag(string flagKey)
        {
            return memoryFlags.TryGetValue(flagKey, out var value) && value;
        }

        public bool HasFlag(string flagKey)
        {
            return memoryFlags.ContainsKey(flagKey);
        }
    }
}
