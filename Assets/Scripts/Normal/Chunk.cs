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
    private FastNoiseLite primaryNoise;
    private FastNoiseLite secondaryNoise;
    
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
        Debug.Log(size);
        ChunkData chunkData = new ChunkData(pos, size);
        _mesh = new Mesh();
        _mesh.name = "Chunk " + _pos;
        primaryNoise = new FastNoiseLite(_seed);
        primaryNoise.SetFrequency(.075f);
        secondaryNoise = new FastNoiseLite(_seed+3);
        primaryNoise.SetFrequency(.05f);

        int _verticesCount = 0;

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                float noiseOne = (primaryNoise.GetNoise(x, z))* 20;
                float noiseTwo = (secondaryNoise.GetNoise(x / 2, z / 2))* 10;
                float yNoise = Mathf.Lerp(noiseOne, noiseTwo, .5f);
                
                chunkData.AddVertice(new Vector3(x, yNoise, z));

                if (x < size - 1 && z < size - 1)
                {
                    chunkData.AddIndices(_verticesCount, _verticesCount + size + 1, _verticesCount + size);
                    chunkData.AddIndices(_verticesCount, _verticesCount + 1, _verticesCount + size + 1);
                }
                _verticesCount++;
            }
        }
        _mesh.SetVertices(chunkData.GetVertices());        
        Debug.Log(chunkData.GetIndices().Length);
        _mesh.SetIndices(chunkData.GetIndices(), MeshTopology.Triangles, 0);
        
        _mesh.RecalculateNormals();
        
        GetComponent<MeshFilter>().mesh = _mesh;
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
        
        _vertices = new Vector3[this.size * this.size];
        _indices = new int[this.size * this.size * this.size];
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
