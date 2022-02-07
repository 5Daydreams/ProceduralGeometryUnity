using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        if (_filledBottom)
        {
            Vector3 startingPoint = new Vector3(0, -_height * 0.5f, 0);

            verts.Add(startingPoint);
        }

        float iterationAngle = 360.0f / _angleDivisions;
        float iterationHeight = _height / _heightDivisions;

        for (int i = 0; i < _heightDivisions + 1; i++)
        {
            float evalPosition = (1.0f*i) / _heightDivisions;
            float radius = _bottleCurve.Evaluate(evalPosition) * _radiusScaling;
            Vector3 radialVec = Vector3.right * radius + Vector3.up * (-_height * 0.5f + i * iterationHeight);

            for (int j = 0; j < _angleDivisions; j++)
            {
                Quaternion rotation = Quaternion.AngleAxis(iterationAngle * j, Vector3.down);

                Vector3 iterationVec = rotation * radialVec;
                verts.Add(iterationVec);
            }
        }

        if (_filledBottom)
        {
            // some stuff needs to happen, bc there's tris to fill before going into the strips
        }

        for (int i = 0; i < _heightDivisions; i++)
        {
            for (int j = 0; j < _angleDivisions; j++)
            {
                int index = i * _angleDivisions + j;
                int angletrick;
                int angletrick2;

                if ((j + 1) % _angleDivisions == 0)
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

        Mesh mesh = new Mesh();

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);

        _meshFilter.sharedMesh = mesh;
    }
}