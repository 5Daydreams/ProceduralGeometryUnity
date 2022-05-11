using UnityEngine;

namespace _Code.Gizmos
{
    public class DotProductVisual : MonoBehaviour
    {
        [SerializeField] private Transform _vectorOrigin;
        [SerializeField] private Transform _vec1;
        [SerializeField] private Transform _vec2;
        [SerializeField] private Transform _resultCube;
        [SerializeField] private float _cubeScaling;
        [SerializeField] private bool _useNormalizedVectors;
    
        private void OnDrawGizmos()
        {
            if (_vec1 == null || _vec2 == null || _resultCube == null)
            {
                return;
            }
            var originPos = _vectorOrigin.position;

            Vector3 relativePos1 = _vec1.position - originPos;
            Vector3 relativePos2 = _vec2.position - originPos;
        
            UnityEngine.Gizmos.color = Color.green; 
            UnityEngine.Gizmos.DrawLine(originPos,originPos+relativePos1);
            UnityEngine.Gizmos.color = Color.red;
            UnityEngine.Gizmos.DrawLine(originPos,originPos+relativePos2);

            float result;
            if (_useNormalizedVectors)
            {
                _cubeScaling = 1;
                result = Vector3.Dot(relativePos1.normalized, relativePos2.normalized);
            }
            else
            {
                result = Vector3.Dot(relativePos1, relativePos2);
            }
        
            _resultCube.localScale = Vector3.up * result * _cubeScaling + Vector3.forward + Vector3.right;
            Vector3 mantainedXZPosition = _resultCube.localPosition.x * Vector3.right + Vector3.up * (result * _cubeScaling/2) + _resultCube.localPosition.z * Vector3.forward;
            _resultCube.localPosition = mantainedXZPosition;
        }
    }
}
