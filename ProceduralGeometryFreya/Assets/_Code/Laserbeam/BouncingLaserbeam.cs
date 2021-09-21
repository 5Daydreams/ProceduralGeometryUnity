using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingLaserbeam : MonoBehaviour
{
    [SerializeField] private Transform _targetDirTransform;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _laserRange;
    private float _remainingLaserTravel;
    private List<Vector3> _points = new List<Vector3>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        if (_targetDirTransform == null || _line == null)
        {
            return;
        }

        bool tooClose = Vector3.Distance(
            this.transform.position, 
            _targetDirTransform.transform.position) 
                        < Single.Epsilon;
        if (tooClose)
        {
            return;
        }

        Vector3 lastPos = this.transform.position;
        Vector3 direction = (_targetDirTransform.position - lastPos).normalized;
        _remainingLaserTravel = _laserRange;
        _points.Clear();
        _points.Add(lastPos);

        while (_remainingLaserTravel > 0)
        {
            Ray ray = new Ray(lastPos, direction);
            bool hitSomething = Physics.Raycast(ray, out RaycastHit raycastInfo, _remainingLaserTravel);

            if (hitSomething)
            {
                Vector3 hitPosition = raycastInfo.point;
                float distanceTravelled = Vector3.Distance(lastPos, hitPosition);
                // Gizmos.DrawLine(lastPos, lastPos + direction * distanceTravelled);
                _points.Add(lastPos + direction * distanceTravelled);
             
                float bounceFactor = Vector3.Dot(direction, raycastInfo.normal.normalized);
                direction = direction - 2 * (bounceFactor) * raycastInfo.normal.normalized;
                direction.Normalize();
                lastPos = hitPosition;
                _remainingLaserTravel -= distanceTravelled;
                continue;
            }

            break;
        }

        // Gizmos.DrawLine(lastPos, lastPos + direction * _remainingLaserTravel);
        _points.Add(lastPos + direction * _remainingLaserTravel);
        
        _line.positionCount = _points.Count;
        _line.SetPositions(_points.ToArray());
    }
}
