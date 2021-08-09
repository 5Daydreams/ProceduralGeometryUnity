using System;
using Submodules._Code.Extensions;
using UnityEngine;

namespace _Code
{
    public class QuadRing : MonoBehaviour
    {
        [Range(0.01f, 2)] [SerializeField] private float _innerRadius;
        [Range(0.01f, 2)] [SerializeField] private float _thickness;

        private float _outerRadius => _innerRadius + _thickness;

        [Range(3, 32)] [SerializeField] private int _detailLevel;


        private void OnDrawGizmosSelected()
        {
            // Gizmos.DrawSphere(transform.position, _innerRadius);
            // Gizmos.DrawSphere(transform.position, _outerRadius);

            GizmoExtensions.DrawWireCircle(transform.position, transform.rotation, _innerRadius, _detailLevel);
            GizmoExtensions.DrawWireCircle(transform.position, transform.rotation, _outerRadius, _detailLevel);
        }
    }
}