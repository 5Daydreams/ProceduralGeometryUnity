using System.Collections;
using System.Collections.Generic;
using Submodules._Code.Extensions;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class MeshStripGenerator : MonoBehaviour
{
    [Range(2,64)][SerializeField] private int _numberOfLayers;
    [SerializeField] private MeshFilter _meshFilter;

    private Mesh _mesh;
    
    private void Awake()
    {
        _mesh = new Mesh();
        _mesh.name = "FunkyMesh";
        
        if (!_meshFilter)
            _meshFilter = GetComponent<MeshFilter>();
    }

    private void Update() => GenerateMesh();

    private void GenerateMesh()
    {
        _mesh.Clear();
        
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        float segmentSize = 1 / (float)_numberOfLayers;
        
        for (int i = 0; i < _numberOfLayers+1; i++)
        {
            float tValueA = segmentSize * i;
            
            Vector3 a = GetPositionOnRegularCurve(tValueA).pos;
            // Quaternion rotation = Quaternion.AngleAxis(45.0f*i/_numberOfLayers, Vector3.up);
            Vector3 b = Vector3.forward + a;
            
            vertices.Add(a);
            vertices.Add(b);
        }
        
        for (int i = 0; i < _numberOfLayers; i++)
        {
            int rootIndex = 2 * i;
            
            triangles.Add(rootIndex);
            triangles.Add(rootIndex+1);
            triangles.Add(rootIndex+2);
            
            triangles.Add(rootIndex+1);
            triangles.Add(rootIndex+3);
            triangles.Add(rootIndex+2);
        }
        
        _mesh.SetVertices(vertices);
        _mesh.SetTriangles(triangles,0);

        _meshFilter.sharedMesh = _mesh;
        return;
    }

    private OrientedPoint GetPositionOnRegularCurve(float t)
    {
        Vector3 pos = new Vector3(
            Mathf.Sin(Mathf.PI*t),
            Mathf.Cos(Mathf.PI*t),
            0.0f
        );

        Quaternion rot = Quaternion.LookRotation(pos);
        
        OrientedPoint op = new OrientedPoint(pos,rot);

        return op;
    }

}
