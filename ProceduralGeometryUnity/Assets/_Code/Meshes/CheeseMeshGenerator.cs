using System.Collections.Generic;
using submodules.unity_spellbook._Code.ExtensionMethods;
using UnityEngine;

namespace _Code.Meshes
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class CheeseMeshGenerator : MonoBehaviour
    {
        [SerializeField] private float height;
        [SerializeField] private float angle;
        [Range(2,64)][SerializeField] private int divisions;
        [SerializeField] private MeshFilter _meshFilter;

        private Mesh _mesh;
    
        private void Awake()
        {
            _mesh = new Mesh();
            _mesh.name = "CheeseMesh";
        
            if (!_meshFilter)
                _meshFilter = GetComponent<MeshFilter>();
        }

        private void Update() => GenerateMesh();

        private void GenerateMesh()
        {
            _mesh.Clear();
        
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
        
            float segmentSize = 1 / (float)divisions;
        
            for (int i = 0; i < divisions+1; i++)
            {
                // Fill in the vertices list
            }
        
            for (int i = 0; i < divisions; i++)
            {
                // Fill in the triangles list
            }
        
            _mesh.SetVertices(vertices);
            _mesh.SetTriangles(triangles,0);

            _meshFilter.sharedMesh = _mesh;
        }

        private OrientedPoint GetPositionOnRegularCurve(float t)
        {
            // Might not need this...?
            Vector3 pos = new Vector3(
                Mathf.Sin(2*Mathf.PI*t),
                Mathf.Cos(2*Mathf.PI*t),
                0.0f
            );

            Quaternion rot = Quaternion.LookRotation(pos);
        
            OrientedPoint op = new OrientedPoint(pos,rot);

            return op;
        }
    }
}