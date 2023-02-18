using System.Collections.Generic;
using Normal;
using UnityEngine;

namespace Voxel
{
    public class VoxelChunk : MonoBehaviour
    {
        public enum BlockType
        {
            Air,
            Grass,
            Dirt,
            Stone,
            Water,
            Sand,
            Snow,
            Wood,
            Bedrock,
        }
        
        [SerializeField]
        private Vector2 _pos;
        [SerializeField]
        private Vector3 _size;
        private int _seed;
        private Vector3[] _vertices;
        private int[] _indices;
        private Mesh _mesh;
        private FastNoiseLite primaryNoise;
        private FastNoiseLite secondaryNoise;
        private FastNoiseLite highriseNoise;
    
        
        
        public Vector2 GetPos()
        {
            return _pos;
        }
    
        // Update is called once per frame
        public void GenerateData(Vector3 pos, Vector3 size,int seed, float noiseScale,int octaves)
        {
            _seed = seed;
            ChunkData chunkData = new ChunkData(pos, size);
            _mesh = new Mesh();
            _mesh.name = "Chunk " + _pos;
            primaryNoise = new FastNoiseLite(_seed);
            primaryNoise.SetFrequency(.015f);
            secondaryNoise = new FastNoiseLite(_seed);
            primaryNoise.SetFrequency(.025f);
            highriseNoise = new FastNoiseLite(_seed);
            highriseNoise.SetFrequency(.005f);

            int _verticesCount = 0;

            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    int y = GenerateHeight(x, z);

                    chunkData.SetBlock(x, 0, z, BlockType.Bedrock);
                    chunkData.SetBlock(x, y, z, BlockType.Grass);
                    for(int i = 1; i < y - 2; i++)
                    {
                        chunkData.SetBlock(x, i, z, BlockType.Dirt);
                    }
                    
                    _verticesCount++;
                }
            }

            chunkData.GenerateData();
            
            _mesh.SetVertices(chunkData.GetVertices());        
            _mesh.SetIndices(chunkData.GetIndices(), MeshTopology.Lines, 0);
        
        
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        private int GenerateHeight(int x, int z)
        {
            float noiseOne = (primaryNoise.GetNoise(x +(_pos.x*_size.x), z+(_pos.y*_size.z))* 10);
            float noiseTwo = (secondaryNoise.GetNoise(x +(_pos.x*_size.x), z+(_pos.y*_size.z))* 30);
            float noiseThree = (highriseNoise.GetNoise(x +(_pos.x*_size.x), z+(_pos.y*_size.z))* 50);
            float yNoise = Mathf.Lerp(noiseOne, noiseTwo, .5f);
            return (int)Mathf.Lerp(yNoise, noiseThree, .5f);
        }
    }
    
    public class ChunkData
    {
        public Vector3[,] sides = new Vector3[6, 4]
        {
            {new Vector3(1,0,0),new Vector3(1,0,1),new Vector3(1,1,1),new Vector3(1,1,0)},
            {new Vector3(0,0,0),new Vector3(0,0,1),new Vector3(0,1,1),new Vector3(0,1,0)},
            {new Vector3(0,1,0),new Vector3(1,1,0),new Vector3(1,1,1),new Vector3(0,1,1)},
            {new Vector3(0,0,0),new Vector3(1,0,0),new Vector3(1,0,1),new Vector3(0,0,1)},
            {new Vector3(0,0,1),new Vector3(1,0,1),new Vector3(1,1,1),new Vector3(0,1,1)},
            {new Vector3(0,0,0),new Vector3(1,0,0),new Vector3(1,1,0),new Vector3(0,1,0)}
        };
        
        private Vector2 _pos;
        private Vector3 _size;
    
        private List<Vector3> _vertices;
        private List<int> _indices;
        private VoxelChunk.BlockType[,,] _blocks;
        

        public ChunkData(Vector2 pos, Vector3 size)
        {
            this._pos = pos;
            this._size = size;

            _vertices = new List<Vector3>();
            _indices = new List<int>();
            _blocks = new VoxelChunk.BlockType[(int)size.x,(int)size.y,(int)size.z];
        }
        
        public VoxelChunk.BlockType[,,] GetBlocks()
        {
            return _blocks;
        }
        
        public void SetBlock(int x, int y, int z, VoxelChunk.BlockType blockType)
        {
            _blocks[x, y, z] = blockType;
        }
        
        public void GenerateData()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    for (int z = 0; z < _size.z; z++)
                    {
                        if (_blocks[x, y, z] != VoxelChunk.BlockType.Air)
                        {
                            GenerateBlock(x, y, z);
                        }
                    }
                }
            }
        }
        
        private void GenerateBlock(int x, int y, int z)
        {
            if(x + 1 > _size.x || _blocks[x + 1,y,z] == VoxelChunk.BlockType.Air)
                GenerateFace(x,y,z,0);
            else if(x - 1 <= 0 || _blocks[x - 1,y,z] == VoxelChunk.BlockType.Air)
                GenerateFace(x,y,z,1);
            else if(y + 1 < _size.y && _blocks[x,y + 1,z] == VoxelChunk.BlockType.Air)
                GenerateFace(x,y,z,2);
            else if(y - 1 >= 0 && _blocks[x,y - 1,z] == VoxelChunk.BlockType.Air)
                GenerateFace(x,y,z,3);
            else if(z + 1 > _size.z || _blocks[x,y,z + 1] == VoxelChunk.BlockType.Air)
                GenerateFace(x,y,z,4);
            else if(z - 1 <= 0 || _blocks[x,y,z - 1] == VoxelChunk.BlockType.Air)
                GenerateFace(x,y,z,5);
        }
        
        private void GenerateFace(int x, int y, int z, int faceIndex)
        {
            int index = _vertices.Count;
            for (int i = 0; i < 4; i++)
            {
                _vertices.Add(sides[faceIndex,i] + new Vector3(x,y,z));
            }
            AddIndices(index,index+1,index+2);
            AddIndices(index,index+2,index+3);
        }
    
        public Vector2 GetPos()
        {
            return _pos;
        }
        
        public void AddIndices(int a,int b,int c)
        {
            _indices.Add(a);
            _indices.Add(b);
            _indices.Add(c);
        }
    
        public void AddVertice(Vector3 vertice)
        {
            _vertices.Add(vertice);
        }
    
        public List<Vector3> GetVertices()
        {
            return _vertices;
        }
        
        public List<int> GetIndices()
        {
            return _indices;
        }
    }
}