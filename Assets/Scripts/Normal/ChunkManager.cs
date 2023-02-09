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
        private int _chunkSize = 256;
        public GameObject chunkPrefab;


        private void GenerateOne()
        {
            GameObject chunkGO = Instantiate(GameObject.Find("Chunk"), Vector2.one, Quaternion.identity);
            chunkGO.AddComponent<Chunk>();
            chunkGO.GetComponent<Chunk>().GenerateData(Vector2.one, _chunkSize, _seed, _noiseScale, _octaves);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                GenerateOne();
        }
    }
}