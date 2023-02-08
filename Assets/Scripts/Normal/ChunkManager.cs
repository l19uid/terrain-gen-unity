using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Normal
{
    public class ChunkManager : MonoBehaviour
    {
        private List<Chunk> _chunks;
        private int _seed;
        private float _noiseScale;
        private int _octaves;
        private int _chunkSize;
        public GameObject chunkPrefab;


        private void GenerateOne()
        {
            Chunk chunk = new Chunk(new Vector2(0, 0), _chunkSize, _seed, _noiseScale, _octaves);
            GameObject chunkGO = Instantiate(chunkPrefab, Vector2.zero, Quaternion.identity);
            chunkGO.AddComponent<Chunk>();
            chunkGO.GetComponent<Chunk>().SetChunk(chunk);
            
            chunkGO.GetComponent<Chunk>().GenerateData();
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                GenerateOne();
        }
    }
}