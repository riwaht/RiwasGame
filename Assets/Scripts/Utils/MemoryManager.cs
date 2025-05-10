using System.Collections.Generic;
using UnityEngine;

namespace RiwasGame.Utils
{
    public class MemoryManager : MonoBehaviour
    {
        public static MemoryManager Instance { get; private set; }

        private HashSet<string> visitedMemories = new HashSet<string>();

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

        public void MarkMemoryVisited(string memoryId)
        {
            visitedMemories.Add(memoryId);
        }

        public bool HasVisited(string memoryId)
        {
            return visitedMemories.Contains(memoryId);
        }
    }
}
