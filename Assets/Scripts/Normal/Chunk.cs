using System.Collections;
using System.Collections.Generic;
using Normal;
using Unity.VisualScripting;
using UnityEngine;
using static Normal.FastNoiseLite;

public class Chunk : MonoBehaviour
{
    [SerializeField]
    private Vector2 _pos;
    [SerializeField]
    private int _size;
    private int _seed;
    private Vector3[] _vertices;
    private List<int> _indices;
    private Mesh _mesh;
    private FastNoiseLite noise;
    
    //private Vector3[] _corners = new Vector3[8]
    //{
    //    new Vector3(0,0,0),
    //    new Vector3(1,0,0),
    //    new Vector3(0,1,0),
    //    new Vector3(0,0,1),
    //    new Vector3(1,0,1),
    //    new Vector3(1,1,0),
    //    new Vector3(0,1,1),
    //    new Vector3(1,1,1)
    //};
    
    public Vector2 GetPos()
    {
        return _pos;
    }
    
    // Update is called once per frame
    public void GenerateData(Vector3 pos, int size,int seed, float noiseScale,int octaves)
    {
        ChunkData chunkData = new ChunkData(pos, size);
        _mesh = new Mesh();
        _mesh.name = "Chunk " + _pos;
        noise = new FastNoiseLite(_seed);

        int _verticesCount = 0;

        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {
                float yNoise = Mathf.Abs(noise.GetNoise(x, z))* 10;
                
                chunkData.AddVertice(new Vector3(x, yNoise, z));

                if (x < _size - 1 && z < _size - 1)
                {
                    chunkData.AddIndices(_verticesCount, _verticesCount + _size + 1, _verticesCount + _size);
                    chunkData.AddIndices(_verticesCount, _verticesCount + 1, _verticesCount + _size + 1);
                }
                _verticesCount++;
            }
        }
        chunkData.Render(GetComponent<MeshFilter>());
    }
}

public class ChunkData
{
    private Vector2 pos;
    private int size;
    
    private Vector3[] _vertices;
    private int[] _indices;

    private int _indiceCount = 0;
    private int _verticeCount = 0;
    
    public ChunkData(Vector2 pos, int size)
    {
        this.pos = pos;
        this.size = size;
        
        _vertices = new Vector3[size * size];
        _indices = new int[size * size];
    }
    
    public Vector2 GetPos()
    {
        return pos;
    }
    
    public void AddIndices(int a,int b,int c)
    {
        _indices[_indiceCount++] = a;
        _indices[_indiceCount++] = b;
        _indices[_indiceCount++] = c;
    }
    
    public void AddVertice(Vector3 vertice)
    {
        _vertices[_verticeCount++] = vertice;
    }
    
    public Vector3[] GetVertices()
    {
        return _vertices;
    }
    
    public int[] GetIndices()
    {
        return _indices;
    }
    
    public void Render(MeshFilter meshFilter)
    {
        Mesh _mesh = new Mesh();
        _mesh.SetVertices(_vertices);        
        _mesh.SetIndices(_indices, MeshTopology.LineStrip, 0);
        
        _mesh.RecalculateNormals();
        
        meshFilter.mesh = _mesh;
    }
}
