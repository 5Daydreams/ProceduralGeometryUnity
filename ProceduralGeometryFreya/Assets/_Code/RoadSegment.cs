using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class RoadSegment : MonoBehaviour
{
    [SerializeField] private Mesh2D _shape2D;
    [SerializeField] private MeshFilter _meshFilter;

    [Range(0, 1)] [SerializeField] private float _bezierLerpT = 0;
    [Range(2, 64)] [SerializeField] private int _edgeRingCount = 5;


    [SerializeField] private Transform[] _controlPoints = new Transform[4];
    public Vector3 GetPosition(int index) => _controlPoints[index].position;

    private Mesh _mesh;

    private void Awake()
    {
        _mesh = new Mesh();
        _mesh.name = "Segment";
        
        if (!_meshFilter)
            _meshFilter = GetComponent<MeshFilter>();
    }

    private void Update() => GenerateMesh();

    private void GenerateMesh()
    {
        _mesh.Clear();

        // Generating Vertices
        List<Vector3> verts = new List<Vector3>();

        for (int ring = 0; ring < _edgeRingCount + 1; ring++)
        {
            float t = ring / (_edgeRingCount - 1.0f);
            OrientedPoint op = GetBezierOrientedPoint(t);

            for (int i = 0; i < _shape2D.VertexCount; i++)
            {
                verts.Add(op.LocalToWorldPosition(_shape2D.vertices[i].point));
            }
        }

        // Generating Triangles
        List<int> triangleIndices = new List<int>();

        for (int ring = 0; ring < _edgeRingCount; ring++)
        {
            int rootIndex = ring * _shape2D.VertexCount;
            int rootIndexNext = (ring + 1) * _shape2D.VertexCount;

            for (int line = 0; line < _shape2D.LineCount; line+=2)
            {
                int lineIndexA = _shape2D.lineIndices[line];
                int lineIndexB = _shape2D.lineIndices[line+1];

                int currentA = rootIndex + lineIndexA;
                int currentB = rootIndex + lineIndexB;
                int nextA = rootIndexNext + lineIndexA;
                int nextB = rootIndexNext + lineIndexB;

                triangleIndices.Add(currentA);
                triangleIndices.Add(nextA);
                triangleIndices.Add(nextB);
                
                triangleIndices.Add(currentA);
                triangleIndices.Add(nextB);
                triangleIndices.Add(currentB);
            }
        }
        
        _mesh.SetVertices(verts);
        _mesh.SetTriangles(triangleIndices,0);
        _mesh.RecalculateNormals();
        
        _meshFilter.sharedMesh = _mesh;
    }

    private void OnDrawGizmos()
    {
        return;
        
        for (int i = 0; i < _controlPoints.Length; i++)
        {
            Gizmos.DrawSphere(GetPosition(i), 0.25f);
        }

        Handles.DrawBezier(
            GetPosition(0),
            GetPosition(3),
            GetPosition(1),
            GetPosition(2),
            Color.white,
            EditorGUIUtility.whiteTexture,
            0.5f
        );

        OrientedPoint testPoint = GetBezierOrientedPoint(_bezierLerpT);

        Gizmos.DrawSphere(testPoint.pos, 0.05f);

        Handles.PositionHandle(testPoint.pos, testPoint.rot);

        // void DrawPoint(Vector2 localPos) => Gizmos.DrawSphere(testPoint.LocalToWorldPosition(localPos), 0.15f);

        Vector3[] verts = _shape2D.vertices.Select(v => testPoint.LocalToWorldPosition(v.point)).ToArray();

        for (int i = 0; i < _shape2D.lineIndices.Length; i += 2)
        {
            Vector3 a = verts[_shape2D.lineIndices[i]];
            Vector3 b = verts[_shape2D.lineIndices[i + 1]];

            Gizmos.DrawLine(a, b);
        }
    }

    private OrientedPoint GetBezierOrientedPoint(float t)
    {
        Vector3 p0 = GetPosition(0);
        Vector3 p1 = GetPosition(1);
        Vector3 p2 = GetPosition(2);
        Vector3 p3 = GetPosition(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 pos = Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;

        return new OrientedPoint(pos, tangent);
    }
}

public struct OrientedPoint
{
    public Vector3 pos;
    public Quaternion rot;

    public OrientedPoint(Vector3 pos, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
    }

    public OrientedPoint(Vector3 pos, Vector3 forward)
    {
        this.pos = pos;
        this.rot = Quaternion.LookRotation(forward);
    }

    public Vector3 LocalToWorldPosition(Vector3 localSpacePosition)
    {
        var worldPosition = this.pos + this.rot * localSpacePosition;
        return worldPosition;
    }

    public Vector3 LocalToWorldVector(Vector3 localSpacePosition)
    {
        var worldPosition = this.rot * localSpacePosition;
        return worldPosition;
    }
}