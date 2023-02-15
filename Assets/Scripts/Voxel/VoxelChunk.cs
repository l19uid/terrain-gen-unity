using UnityEngine;

namespace Voxel
{
    public class VoxelChunk : MonoBehaviour
    {
        
    }
    public class ChunkData
    {
        private Vector2 _pos;
        private int _size;
    
        private List<Vector3> _vertices;
        private List<int> _indices;

        private int _indiceCount = 0;
        private int _verticeCount = 0;
    
        public ChunkData(Vector2 pos, int size)
        {
            this.pos = pos;
            this.size = size;

            _vertices = new Vector3[size * size];
            _indices = new int[(size - 1) * (size - 1) * 6];
        }
    
        public Vector2 GetPos()
        {
            return pos;
        }
    
        public void AddIndices(int a,int b,int c)
        {
            _indices[_indiceCount] = a;
            _indices[_indiceCount+1] = b;
            _indices[_indiceCount+2] = c;
            _indiceCount += 3;
        }
    
        public void AddVertice(Vector3 vertice)
        {
            _vertices[_verticeCount] = vertice;
            _verticeCount++;
        }
    
        public Vector3[] GetVertices()
        {
            return _vertices;
        }
    
        public int[] GetIndices()
        {
            return _indices;
        }
    }

}