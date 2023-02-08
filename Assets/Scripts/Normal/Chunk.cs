using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField]
    private Vector2 _pos;
    [SerializeField]
    private int _size;
    private int _seed;
    private float _noiseScale;
    private int _octaves;
    private List<Vector3> _vertices;
    private List<int> _indices;
    private Mesh _mesh;
    
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
    
    public Chunk(Vector2 pos, int size, int seed, float noiseScale, int octaves)
    {
        _pos = pos;
        _size = size;
        _seed = seed;
        _noiseScale = noiseScale;
        _octaves = octaves;
        _mesh = new Mesh();
    }
    
    Vector3[] newVertices = new Vector3[3]
    {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(0,1,0)
    };
    int[] newTriangles = new []{0,1,2};

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
    }
    
    public Vector2 GetPos()
    {
        return _pos;
    }
    
    public void SetChunk(Chunk chunk)
    {
        _pos = chunk._pos;
        _size = chunk._size;
        _seed = chunk._seed;
        _noiseScale = chunk._noiseScale;
        _octaves = chunk._octaves;
        _mesh = chunk._mesh;
        _mesh = new Mesh();
    }
    
    // Update is called once per frame
    public void GenerateData()
    {
        _mesh = new Mesh();
        _mesh.name = "Chunk " + _pos;
        
        _vertices = new List<Vector3>();
        _indices = new List<int>();
        
        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {
                float yNoise = (int)(Mathf.PerlinNoise(x, z) * 50);
                
                _vertices.Add(new Vector3(x, yNoise, z));
                _indices.Add(_vertices.Count - 1);
            }
        }
        
        _mesh.SetIndices(_indices, MeshTopology.Points, 0);
        _mesh.SetVertices(_vertices);        
        
        GetComponent<MeshFilter>().mesh = _mesh;
        
        _mesh.triangles = _indices.ToArray();
        _mesh.RecalculateNormals();
    }
    
}
