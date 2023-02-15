using System.Collections.Generic;
using UnityEngine;

namespace Normal
{
    public class ChunkManager : MonoBehaviour
    {
        private List<Chunk> _chunks;
        private int _seed;
        private float _noiseScale;
        private int _octaves;
        private int _chunkSize = 255;
        public GameObject chunkPrefab;
        public int renderDistance = 2;
        private Vector2 _playerPos;


        private void GenerateOne()
        {
            GameObject chunkGO = Instantiate(GameObject.Find("Chunk"), Vector2.one, Quaternion.identity);
            chunkGO.AddComponent<Chunk>();
            chunkGO.GetComponent<Chunk>().GenerateData(Vector2.one, _chunkSize, _seed, _noiseScale, _octaves);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
                GenerateChunks();
        }
        
        private void GenerateChunks()
        {
            for (int x = -((int)_playerPos.x + renderDistance) / 2; x < renderDistance / 2; x++)
            {
                for (int z = -((int)_playerPos.y+renderDistance) / 2; z < renderDistance / 2; z++)
                {
                    GameObject chunkGO = Instantiate(GameObject.Find("Chunk"), new Vector3(x * _chunkSize, 0, z * _chunkSize), Quaternion.identity);
                    chunkGO.AddComponent<Chunk>();
                    chunkGO.GetComponent<Chunk>().GenerateData(new Vector3(x, 0, z), _chunkSize, _seed, _noiseScale, _octaves);
                }
            }
        }
    }
}