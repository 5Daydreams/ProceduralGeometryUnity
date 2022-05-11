using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Code.Meshes
{
    [RequireComponent(typeof(MeshFilter))]
    [ExecuteInEditMode]
    public class TorusMeshGenerator : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [Range(3, 64)] [SerializeField] private int _numberOfInnerRings = 3;
        [Range(3, 32)] [SerializeField] private int _ringVertexCount = 1;
        [Range(0.01f, 10)] [SerializeField] private float _torusThickness = 1;
        [Range(0.01f, 10)] [SerializeField] private float _torusRadius = 1;

        private Mesh _mesh;

        private void Awake()
        {
            _mesh = new Mesh();
            _mesh.name = "TorusMesh";

            if (!_meshFilter)
                _meshFilter = GetComponent<MeshFilter>();
        }

        private void Update()
        {
            GenerateMesh();
        }


        private void GenerateMesh()
        {
            _mesh.Clear();

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector3> normals = new List<Vector3>();

            float largerRingSegmentCount = 1.0f / (_numberOfInnerRings);
            float innerRingSegmentCount = 1.0f / (_ringVertexCount);

            for (int i = 0; i < _numberOfInnerRings +1 ; i++)
            {
                Quaternion iterationRot = Quaternion.AngleAxis(360.0f * i * largerRingSegmentCount, Vector3.up);

                Vector3 displacement = SpinAroundAxis(360.0f * i * largerRingSegmentCount, Vector3.right * _torusRadius,
                    Vector3.up);

                for (int j = 0; j < _ringVertexCount; j++)
                {
                    Vector3 innerDisplacement1 = SpinAroundAxis(360.0f * j * innerRingSegmentCount,
                        Vector3.left * _torusThickness, Vector3.forward);
                    Vector3 innerDisplacement2 = SpinAroundAxis(360.0f * (j + 1) * innerRingSegmentCount,
                        Vector3.left * _torusThickness, Vector3.forward);

                    innerDisplacement1 = iterationRot * innerDisplacement1;
                    innerDisplacement2 = iterationRot * innerDisplacement2;

                    Vector3 a = displacement + innerDisplacement1;
                    Vector3 b = displacement + innerDisplacement2;

                    vertices.Add(a);
                    vertices.Add(b);
                    normals.Add(innerDisplacement1.normalized);
                    normals.Add(innerDisplacement2.normalized);
                }
            }

            for (int i = 0; i < _numberOfInnerRings; i++)
            {
                for (int j = 0; j < _ringVertexCount; j++)
                {
                    int rootIndex = 2*i * _ringVertexCount + 2*j;
                    int nextRingIndex = 2*(i + 1) * _ringVertexCount + 2*j;

                    triangles.Add(rootIndex);
                    triangles.Add(nextRingIndex + 1);
                    triangles.Add(rootIndex + 1);

                    triangles.Add(rootIndex);
                    triangles.Add(nextRingIndex);
                    triangles.Add(nextRingIndex + 1);
                }
            }

            _mesh.SetVertices(vertices);
            _mesh.SetTriangles(triangles,0);
            _mesh.SetNormals(normals);
            // _mesh.RecalculateNormals();
        
            _meshFilter.sharedMesh = _mesh;
        }

        // private void OnDrawGizmos() => GenerateGizmo();

        private void GenerateGizmo()
        {
            float largerRingSegmentCount = 1.0f / (_numberOfInnerRings);
            float innerRingSegmentCount = 1.0f / (_ringVertexCount);

            for (int i = 0; i < _numberOfInnerRings - 1; i++)
            {
                Quaternion iterationRot = Quaternion.AngleAxis(360.0f * i * largerRingSegmentCount, Vector3.up);

                // Vector3 displacement1 = Quaternion.AngleAxis(360.0f * i * largerRingSegmentCount, Vector3.up) * Vector3.right * _torusRadius;
                Vector3 displacement1 = SpinAroundAxis(360.0f * i * largerRingSegmentCount, Vector3.right * _torusRadius,
                    Vector3.up);
                // Vector3 displacement2 = Quaternion.AngleAxis(360.0f *(i+1) * largerRingSegmentCount, Vector3.up) * Vector3.right * _torusRadius;
                Vector3 displacement2 = SpinAroundAxis(360.0f * (i + 1) * largerRingSegmentCount,
                    Vector3.right * _torusRadius, Vector3.up);

                Handles.DrawLine(displacement1, displacement2);

                for (int j = 0; j < _ringVertexCount - 1; j++)
                {
                    // Vector3 innerDisplacement1 = Quaternion.AngleAxis(360.0f * j * innerRingSegmentCount, Vector3.forward) * Vector3.right * _torusThickness;
                    Vector3 innerDisplacement1 = SpinAroundAxis(360.0f * j * innerRingSegmentCount,
                        Vector3.right * _torusThickness, Vector3.forward);
                    // Vector3 innerDisplacement2 = Quaternion.AngleAxis(360.0f *(j+1) * innerRingSegmentCount, Vector3.forward) * Vector3.right * _torusThickness;
                    Vector3 innerDisplacement2 = SpinAroundAxis(360.0f * (j + 1) * innerRingSegmentCount,
                        Vector3.right * _torusThickness, Vector3.forward);

                    innerDisplacement1 = iterationRot * innerDisplacement1;
                    innerDisplacement2 = iterationRot * innerDisplacement2;

                    Handles.DrawLine(displacement1 + innerDisplacement1,
                        displacement1 + innerDisplacement2);
                }

                // Vector3 lastInnerdisplacement =
                //     Quaternion.AngleAxis(360.0f * (1 - innerRingSegmentCount), Vector3.forward) * Vector3.right * _torusThickness;

                Vector3 lastInnerdisplacement =
                    SpinAroundAxis(360.0f * (1 - innerRingSegmentCount), _torusThickness * Vector3.right, Vector3.forward);

                Handles.DrawLine(displacement1 + iterationRot * lastInnerdisplacement,
                    displacement1 + iterationRot * Vector3.right * _torusThickness);
            }

            // Vector3 displacementFinal = Quaternion.AngleAxis(360.0f * (1- largerRingSegmentCount), Vector3.up) * Vector3.right * _torusRadius;
            Vector3 displacementFinal =
                SpinAroundAxis(360.0f * (1 - largerRingSegmentCount), Vector3.right * _torusRadius, Vector3.up);
            Vector3 displacementZero = Vector3.right * _torusRadius;

            Handles.DrawLine(displacementFinal, displacementZero);

            Quaternion finalIterationRot = Quaternion.AngleAxis(360.0f * (1 - largerRingSegmentCount), Vector3.up);

            for (int j = 0; j < _ringVertexCount - 1; j++)
            {
                // Vector3 innerDisplacement1 = Quaternion.AngleAxis(360.0f * j * innerRingSegmentCount, Vector3.forward) * Vector3.right * _torusThickness;
                Vector3 innerDisplacement1 = SpinAroundAxis(360.0f * j * innerRingSegmentCount,
                    Vector3.right * _torusThickness, Vector3.forward);
                // Vector3 innerDisplacement2 = Quaternion.AngleAxis(360.0f *(j+1) * innerRingSegmentCount, Vector3.forward) * Vector3.right * _torusThickness;
                Vector3 innerDisplacement2 = SpinAroundAxis(360.0f * (j + 1) * innerRingSegmentCount,
                    Vector3.right * _torusThickness, Vector3.forward);

                Handles.DrawLine(displacementFinal + finalIterationRot * innerDisplacement1,
                    displacementFinal + finalIterationRot * innerDisplacement2);
            }

            // Vector3 lastInnerdisplacement2 =
            //     Quaternion.AngleAxis(360.0f * (1 - innerRingSegmentCount), Vector3.forward) * Vector3.right * _torusThickness;
            Vector3 lastInnerdisplacement2 = SpinAroundAxis(360.0f * (1 - innerRingSegmentCount),
                Vector3.right * _torusThickness, Vector3.forward);

            Handles.DrawLine(displacementFinal + finalIterationRot * lastInnerdisplacement2,
                displacementFinal + finalIterationRot * Vector3.right * _torusThickness);
        }

        private Vector3 SpinAroundAxis(float angle, Vector3 radialVector, Vector3 axis)
        {
            return Quaternion.AngleAxis(angle, axis) * radialVector;
        }
    }
}