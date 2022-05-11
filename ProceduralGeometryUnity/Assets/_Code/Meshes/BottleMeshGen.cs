using System.Collections.Generic;
using _Code.Extensions;
using UnityEngine;

namespace _Code.Meshes
{
    [RequireComponent(typeof(MeshFilter))]
    public class BottleMeshGen : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _bottleCurve;
        [Range(1.0f, 10.0f)] [SerializeField] private float _height = 1.0f;
        [Range(1.0f, 10.0f)] [SerializeField] private float _radiusScaling = 1.0f;
        [Range(1, 32)] [SerializeField] private int _heightDivisions = 5;
        [Range(3, 32)] [SerializeField] private int _angleDivisions = 5;
        [Range(0, 6)] [SerializeField] private int _edgeDivisions;
        [SerializeField] private bool _filledBottom = false;
        [SerializeField] private bool _filledTop = false;
        private MeshFilter _meshFilter;

        private void Awake()
        {
            if (!_meshFilter)
            {
                _meshFilter = this.GetComponent<MeshFilter>();
            }
        }

        private void OnValidate()
        {
            if (!_meshFilter)
            {
                _meshFilter = this.GetComponent<MeshFilter>();
            }

            GenerateMesh();
        }

        private void GenerateMesh()
        {
            AnimationCurve curve = _bottleCurve.MultiplyCurve(_radiusScaling);
            
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvCoords = new List<Vector2>();
            List<int> tris = new List<int>();

            int indexStart = 0;

            float iterationAngleSubdivision = 1.0f / _angleDivisions;
            float iterationAngleStepSize = 360.0f * iterationAngleSubdivision;
            float iterationHeight = _height / _heightDivisions;

            // Fill in vertices -------------------------------------------------

            if (_filledBottom)
            {
                indexStart = 1;
                Vector3 startingPoint = new Vector3(0, -_height * 0.5f, 0);

                verts.Add(startingPoint);
                normals.Add(Vector3.down);
                uvCoords.Add(new Vector2(0.5f,0.0f));

                float radius = curve.Evaluate(0);
                Vector3 radialVec = Vector3.right * radius + Vector3.up * (-_height * 0.5f);

                float tangent = curve.GetDerivative(0);
                Vector3 baseNormal = (Vector3.right + Vector3.down * tangent).normalized;

                baseNormal = Quaternion.AngleAxis(-90.0f, Vector3.forward) * baseNormal;
                
                for (int j = 0; j < _angleDivisions+1; j++)
                {
                    Quaternion rotation = Quaternion.AngleAxis(iterationAngleStepSize * j, Vector3.down);

                    Vector3 iterationPosVector = rotation * radialVec;
                    Vector3 iterationNormalVector = rotation * baseNormal;
                    Vector2 iterationUVCoords = new Vector2(iterationAngleSubdivision * (j),0);
                    
                    verts.Add(iterationPosVector);
                    normals.Add(iterationNormalVector);
                    uvCoords.Add(iterationUVCoords);
                }
            }

            for (int i = indexStart; i < _heightDivisions + 1; i++)
            {
                float evalPosition = (1.0f * i) / _heightDivisions;
                
                float radius = curve.Evaluate(evalPosition);  
                Vector3 radialVec = Vector3.right * radius + Vector3.up * (-_height * 0.5f + i * iterationHeight);
                
                float tangent = curve.GetDerivative(evalPosition);
                Vector3 baseNormal = (Vector3.right + Vector3.down * tangent).normalized;

                for (int j = 0; j < _angleDivisions+1; j++)
                {
                    Quaternion rotation = Quaternion.AngleAxis(iterationAngleStepSize * j, Vector3.down);

                    Vector3 iterationVec = rotation * radialVec;
                    Vector3 iterationNormalVector = rotation * baseNormal;
                    Vector2 iterationUVCoords = new Vector2(iterationAngleSubdivision * (j),evalPosition);
                    
                    verts.Add(iterationVec);
                    normals.Add(iterationNormalVector);
                    uvCoords.Add(iterationUVCoords);
                }
            }

            if (_filledTop)
            {
                Vector3 topVector = Vector3.up * (+_height * 0.5f);
                verts.Add(topVector);
                normals.Add(Vector3.up);
                uvCoords.Add(Vector2.up);
            }

            // Vertices Filled --------------------------------------------------

            // Fill in tris -----------------------------------------------------

            if (_filledBottom)
            {
                for (int j = 0; j < _angleDivisions+1; j++)
                {
                    int index = (j + 1);
                    int thing = (j + 1) + 1;

                    tris.Add(0);
                    tris.Add(index);
                    tris.Add(thing);
                }
            }


            for (int i = 0; i < _heightDivisions; i++)
            {
                for (int j = 0; j < _angleDivisions + 1; j++)
                {
                    int index = i * _angleDivisions + j + indexStart;
                    int angletrick;
                    int angletrick2;

                    if ((j + 1) == 0)
                    {
                        angletrick = 0;
                        angletrick2 = -_angleDivisions;
                    }
                    else
                    {
                        angletrick = _angleDivisions;
                        angletrick2 = 0;
                    }

                    tris.Add(index);
                    tris.Add(index + _angleDivisions);
                    tris.Add(index + angletrick + 1);

                    tris.Add(index);
                    tris.Add(index + angletrick + 1);
                    tris.Add(index + angletrick2 + 1);
                }
            }


            if (_filledTop)
            {
                int lastIndex = (_heightDivisions + 1) * (_angleDivisions) + indexStart;

                for (int j = 0; j < _angleDivisions+1; j++)
                {
                    int index1 = _heightDivisions * _angleDivisions + indexStart + j;
                    int index2 = _heightDivisions * _angleDivisions + indexStart + (j + 1);

                    tris.Add(lastIndex);
                    tris.Add(index2);
                    tris.Add(index1);
                }
            }

            // Tris filled  -----------------------------------------------------

            Mesh mesh = new Mesh();

            mesh.SetVertices(verts);
            mesh.SetNormals(normals);
            mesh.SetTriangles(tris, 0);
            mesh.SetUVs(0, uvCoords);

            _meshFilter.sharedMesh = mesh;
        }
    }
}