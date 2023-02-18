using System.Collections.Generic;
using UnityEngine;

namespace Voxel
{
    public class VoxelChunkManager : MonoBehaviour
    {
        List<VoxelChunk> _chunks;
        private int _seed;
        private float _noiseScale;
        private int _octaves;
        private Vector3 _chunkSize = new Vector3(16, 256, 16);
        public Vector3 _playerPos;
        public int renderDistance = 4;
        
        
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
                    GameObject chunkGO = Instantiate(GameObject.Find("VoxelChunk"), new Vector3(x * _chunkSize.x, 0, z * _chunkSize.z), Quaternion.identity);
                    chunkGO.AddComponent<VoxelChunk>();
                    chunkGO.GetComponent<VoxelChunk>().GenerateData(new Vector3(x, 0, z), _chunkSize, _seed, _noiseScale, _octaves);
                }
            }
        }
        
    }
}