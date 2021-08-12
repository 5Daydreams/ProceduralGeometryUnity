using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ProceduralGeometryFreya : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _meshFilter;

    private void OnEnable()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Quad";

        List<Vector3> points = new List<Vector3>()
        {
            new Vector3 (-1, 1),
            new Vector3 (1, 1),
            new Vector3 (-1, -1),
            new Vector3 (1, -1)
        };

        int[] triIndices = new int[]
        {
            2, 0, 1,
            2, 1, 3
        };

        List<Vector2> uvs = new List<Vector2>()
        {
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(0,0),
            new Vector2(1,0)
        };

        List<Vector3> normals = new List<Vector3>()
        {
            new Vector3 (0, 0, 1),
            new Vector3 (0, 0, 1),
            new Vector3 (0, 0, 1),
            new Vector3 (0, 0, 1)
        };

        mesh.SetVertices(points);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.triangles = triIndices;

        _meshFilter.sharedMesh = mesh;
    }
}