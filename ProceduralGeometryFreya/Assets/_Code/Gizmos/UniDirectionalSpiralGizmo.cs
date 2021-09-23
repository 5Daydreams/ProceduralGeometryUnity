using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniDirectionalSpiralGizmo : MonoBehaviour
{
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _radius;
    [SerializeField] private float _coilLength;
    [SerializeField] private float _curlFactor;
    [SerializeField] private int _pointCount;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;

    private void OnDrawGizmos()
    {
        Vector3 localDirection = (this.transform.rotation * _direction).normalized;

        if (_pointCount <= 2)
            _pointCount = 3;
        
        // Gizmos.color = Color.red;
        // Gizmos.DrawRay(transform.position,localDirection*_coilLength);

        float angleStepSize = 360.0f*_curlFactor / _pointCount;
        float lengthStepSize = _coilLength / _pointCount;

        for (int i = 0; i < _pointCount; i++)
        {
            Quaternion rotationQuaternion1 = Quaternion.AngleAxis(angleStepSize * i, localDirection);
            Quaternion rotationQuaternion2 = Quaternion.AngleAxis(angleStepSize * (i + 1), localDirection);

            Vector3 point1 = rotationQuaternion1 * (transform.right * _radius) + (localDirection * (i)* lengthStepSize);
            Vector3 point2 = rotationQuaternion2 * (transform.right * _radius) + (localDirection * (i+1) *lengthStepSize);

            Gizmos.color = Color.Lerp(_startColor,_endColor,i*1.0f/_pointCount);
            Gizmos.DrawLine(point1, point2);
        }
    }
}