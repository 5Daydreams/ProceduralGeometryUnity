using System.Collections.Generic;
using Submodules._Code.Extensions;
using UnityEngine;

namespace _Code
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public class QuadRing : MonoBehaviour
    {
        public enum UVProjectionType
        {
            AngularRadial,
            ProjectionZ
        }

        [Range(0.01f, 2)] [SerializeField] private float _innerRadius;
        [Range(0.01f, 2)] [SerializeField] private float _thickness;
        private float _outerRadius => _innerRadius + _thickness;

        [Range(3, 32)] [SerializeField] private int _angularSegmentCount = 3;
        private Mesh _mesh;
        private int _vertexCount => _angularSegmentCount * 2;
        [SerializeField] private MeshFilter _meshFilter;

        [SerializeField] private UVProjectionType _uvProjection = UVProjectionType.AngularRadial;


        private void OnDrawGizmosSelected()
        {
            GizmoExtensions.DrawWireCircle(transform.position, transform.rotation, _innerRadius, _angularSegmentCount);
            GizmoExtensions.DrawWireCircle(transform.position, transform.rotation, _outerRadius, _angularSegmentCount);
        }

        private void Awake()
        {
            _mesh = new Mesh();
            _mesh.name = "QuadRing";

            _meshFilter.sharedMesh = _mesh;
        }

        private void Update() => GenerateMesh();

        private void GenerateMesh()
        {
            _mesh.Clear();

            int vertexCount = _vertexCount;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();


            for (int i = 0; i < _angularSegmentCount + 1; i++)
            {
                float t = i / (float) _angularSegmentCount;
                float angleRadians = t * 2 * Mathf.PI;
                Vector2 direction = VectorExtension.GetVectorByAngle(1, angleRadians);

                // Vector3 zOffset = Vector3.forward * Mathf.Cos(angleRadians * 4);

                vertices.Add((Vector3) (direction * _outerRadius)); // + zOffset);
                vertices.Add((Vector3) (direction * _innerRadius)); // + zOffset);

                normals.Add(Vector3.forward);
                normals.Add(Vector3.forward);

                switch (_uvProjection)
                {
                    case UVProjectionType.AngularRadial:
                    {
                        uvs.Add(new Vector2(t, 1));
                        uvs.Add(new Vector2(t, 0));
                        break;
                    }
                    case UVProjectionType.ProjectionZ:
                    {
                        uvs.Add((direction + Vector2.one)/2.0f);
                        uvs.Add((direction*(_innerRadius/_outerRadius) + Vector2.one)/2.0f);
                        break;
                    }
                }
            }

            List<int> triangleIndices = new List<int>();

            for (int i = 0; i < _angularSegmentCount; i++)
            {
                int indexRoot = i * 2;
                int indexInnerRoot = indexRoot + 1;
                int indexOuterNext = indexRoot + 2;
                int indexInnerNext = indexRoot + 3;
    
                triangleIndices.Add(indexRoot);
                triangleIndices.Add(indexInnerNext);
                triangleIndices.Add(indexOuterNext);

                triangleIndices.Add(indexRoot);
                triangleIndices.Add(indexInnerRoot);
                triangleIndices.Add(indexInnerNext);
            }

            _mesh.SetVertices(vertices);
            _mesh.SetTriangles(triangleIndices, 0);
            _mesh.SetNormals(normals);
            _mesh.SetUVs(0, uvs);
        }
    }
}