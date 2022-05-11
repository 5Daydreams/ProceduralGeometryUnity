using System;
using UnityEngine;

namespace _Code
{
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

        public float CalculateUSpan()
        {
            float dist = 0;
            for (int i = 0; i < LineCount; i+=2)
            {
                Vector2 a = vertices[lineIndices[i]].point;
                Vector2 b = vertices[lineIndices[i+1]].point;

                dist += Vector2.Distance(b,a);
            }

            return dist;
        }
    }
}