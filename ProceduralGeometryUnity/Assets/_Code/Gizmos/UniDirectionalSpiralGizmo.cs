using UnityEngine;

namespace _Code.Gizmos
{
    public class UniDirectionalSpiralGizmo : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _radius;
        [SerializeField] private float _coilLength;
        [SerializeField] private float _curlFactor;
        [SerializeField] private int _pointCount;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;
        [SerializeField] private bool _drawAxis;

        private void OnDrawGizmos()
        {
            Vector3 axisDir = (this.transform.rotation * _direction).normalized;

            if (_pointCount <= 2)
                _pointCount = 3;

            if (_drawAxis)
            {
                UnityEngine.Gizmos.color = Color.red;
                UnityEngine.Gizmos.DrawRay(transform.position, axisDir * _coilLength);
            }

            float angleStepSize = 360.0f * _curlFactor / _pointCount;
            float lengthStepSize = _coilLength / _pointCount;

            for (int i = 0; i < _pointCount; i++)
            {
                Quaternion rotationQuaternion1 = Quaternion.AngleAxis(angleStepSize * i, axisDir);
                Quaternion rotationQuaternion2 = Quaternion.AngleAxis(angleStepSize * (i + 1), axisDir);

                Vector3 point1 = rotationQuaternion1 * (transform.right * _radius) + (axisDir * (i) * lengthStepSize);
                Vector3 point2 = rotationQuaternion2 * (transform.right * _radius) + (axisDir * (i + 1) * lengthStepSize);

                UnityEngine.Gizmos.color = Color.Lerp(_startColor, _endColor, i * 1.0f / _pointCount);
                UnityEngine.Gizmos.DrawLine(point1, point2);
            }
        }
    }
}