using System;
using UnityEngine;


[CreateAssetMenu()]
public class Mesh2D : ScriptableObject
{
    [Serializable] 
    public class Vertex
    {
        public Vector2 point;
        public Vector2 normal;
        public float u;
    }

    public Vertex[] vertices;
    public int[] lineIndices;

    public int VertexCount => vertices.Length;
    public int LineCount => lineIndices.Length;
}