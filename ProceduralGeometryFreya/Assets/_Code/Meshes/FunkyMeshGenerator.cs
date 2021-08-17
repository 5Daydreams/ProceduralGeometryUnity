using System;
using System.Collections.Generic;
using System.Linq;
using Submodules._Code.Extensions;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class FunkyMeshGenerator : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [Range(2, 64)] [SerializeField] private int _numberOfSegments = 3;
    [Range(1, 64)] [SerializeField] private int _numberOfStripes = 1;
    [Range(0, 360)] [SerializeField] private float _stripeAngularWidth = 90;
    [Range(0, 360)] [SerializeField] private float _curlAngle = 90;

    private Mesh _mesh;

    private void Awake()
    {
        _mesh = new Mesh();
        _mesh.name = "FunkyMesh";

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
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();

        float segmentSize = 1 / (float) _numberOfSegments;

        for (int i = 0; i < _numberOfSegments + 1; i++)
        {
            float tValueA = segmentSize * i;
            for (int j = 0; j < _numberOfStripes; j++)
            {
                float baseAngle = 360.0f / _numberOfStripes;

                Quaternion stripeRotation = Quaternion.AngleAxis(baseAngle * j+ i*_curlAngle/_numberOfSegments, Vector3.up);

                Vector3 a = GetPositionOnRegularCurve(tValueA).pos;
                a = stripeRotation * a;

                Quaternion rotation = Quaternion.AngleAxis(_stripeAngularWidth, Vector3.up);
                Vector3 b = rotation * a; 

                vertices.Add(a);
                normals.Add(a.normalized);
                
                vertices.Add(b);
                normals.Add(b.normalized);
            }
        }

        for (int i = 0; i < _numberOfSegments; i++)
        {
            for (int j = 0; j < _numberOfStripes; j++)
            {
                int rootIndex = _numberOfStripes * 2 * i + 2 * j;
                int nextIndex = _numberOfStripes * 2 * (i + 1) + 2 * j;

                triangles.Add(rootIndex);
                triangles.Add(nextIndex);
                triangles.Add(nextIndex + 1);

                triangles.Add(rootIndex);
                triangles.Add(nextIndex + 1);
                triangles.Add(rootIndex + 1);
            }
        }

        _mesh.SetVertices(vertices);
        _mesh.SetTriangles(triangles, 0);
        _mesh.SetVertices(normals);

        _meshFilter.sharedMesh = _mesh;
        return;
    }

    // private void OnDrawGizmos() => GenerateGizmo();

    private void GenerateGizmo()
    {
        float segment = 1 / (_numberOfSegments - 1.0f);
        for (int i = 0; i < _numberOfSegments - 1; i++)
        {
            float tValueA = segment * i;
            float tValueB = segment * (i + 1);
            Vector3 a = GetPositionOnFunkyCurve(tValueA).pos * transform.lossyScale.x + transform.position;
            Vector3 b = GetPositionOnFunkyCurve(tValueB).pos * transform.lossyScale.x + transform.position;

            Gizmos.DrawLine(a, b);

            Vector3 c = GetPositionOnRegularCurve(tValueA).pos * transform.lossyScale.x + transform.position;
            Vector3 d = GetPositionOnRegularCurve(tValueB).pos * transform.lossyScale.x + transform.position;

            Gizmos.DrawLine(c, d);
        }
    }

    private OrientedPoint GetPositionOnFunkyCurve(float t)
    {
        Vector3 pos = new Vector3(
            Mathf.Sin(Mathf.PI * t),
            Mathf.Cos(Mathf.PI * t),
            0.5f * Mathf.Sin(2 * Mathf.PI * t)
        );

        Quaternion rot = Quaternion.LookRotation(pos);

        OrientedPoint op = new OrientedPoint(pos, rot);

        return op;
    }

    private OrientedPoint GetPositionOnRegularCurve(float t)
    {
        Vector3 pos = new Vector3(
            Mathf.Sin(Mathf.PI * t),
            Mathf.Cos(Mathf.PI * t),
            0.0f
        );

        Quaternion rot = Quaternion.LookRotation(pos);

        OrientedPoint op = new OrientedPoint(pos, rot);

        return op;
    }
}