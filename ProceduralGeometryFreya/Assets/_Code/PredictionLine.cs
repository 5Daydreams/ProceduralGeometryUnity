using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO.IsolatedStorage;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

[ExecuteAlways]
public class PredictionLine : MonoBehaviour
{
    [SerializeField] private Transform _directionTransform;

    [Range(0.001f, 0.01f)] [SerializeField]
    private float _stepSize;

    [SerializeField] private float _launchSpeed;
    [SerializeField] private int _pointCount;
    [SerializeField] private float _coilRadius;
    [SerializeField] private float _curlFactor;
    [SerializeField] private Color _spiralColor;
    [SerializeField] private Color _trajColor;

    private Vector3 _launchDirection =>
        (_directionTransform.position - this.transform.position).normalized * _launchSpeed;


    private void OnDrawGizmos()
    {
        if (_directionTransform == null || _stepSize < 0.0001f)
        {
            return;
        }

        Vector3 gravity = Physics.gravity;
        Vector3 speed = _launchDirection;

        Vector3 pos = this.transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, _directionTransform.position);

        float angleStepSize = 360.0f * _curlFactor / _pointCount;

        if (_pointCount <= 2)
            _pointCount = 3;

        for (int i = 0; i < _pointCount; i++)
        {
            float stepSize = i * _stepSize;
            float stepSize2 = stepSize * stepSize;

            Vector3 nextPos = pos + speed * stepSize + gravity * 0.5f * stepSize2;

            Vector3 iterationAxis = nextPos - pos;

            Gizmos.color = _trajColor;
            Gizmos.DrawLine(pos, nextPos);

            Vector3 axisOffsetDir = Vector3.Cross(nextPos - pos, Vector3.up).normalized * _coilRadius;

            Quaternion rotationQuaternion1 = Quaternion.AngleAxis(angleStepSize * i, iterationAxis);
            Quaternion rotationQuaternion2 = Quaternion.AngleAxis(angleStepSize * (i + 0.5f), iterationAxis);
            Quaternion rotationQuaternion3 = Quaternion.AngleAxis(angleStepSize * (i + 1), iterationAxis);

            Vector3 point1 = rotationQuaternion1 * axisOffsetDir + (pos);
            Vector3 point2 = rotationQuaternion2 * axisOffsetDir + (pos + nextPos) / 2;
            Vector3 point3 = rotationQuaternion3 * axisOffsetDir + nextPos;

            pos = nextPos;

            Gizmos.color = _spiralColor;
            Gizmos.DrawLine(point1, point2);
            Gizmos.DrawLine(point2, point3);
        }
    }
}