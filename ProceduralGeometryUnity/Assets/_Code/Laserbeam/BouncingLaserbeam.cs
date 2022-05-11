using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Code.Laserbeam
{
    public class BouncingLaserbeam : MonoBehaviour
    {
        [SerializeField] private Transform _targetDirTransform;
        [SerializeField] private LineRenderer _line;
        [SerializeField] private float _laserRange;
        private float _remainingLaserTravel;
        private List<Vector3> _points = new List<Vector3>();

        private void OnDrawGizmos()
        {
            UnityEngine.Gizmos.color = Color.red;

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
                    _points.Add(lastPos + direction * distanceTravelled);
             
                    raycastInfo.normal.Normalize();
                    float bounceFactor = Vector3.Dot(direction, raycastInfo.normal);
                    direction = direction - 2 * (bounceFactor) * raycastInfo.normal;
                
                    direction.Normalize();
                    lastPos = hitPosition;
                    _remainingLaserTravel -= distanceTravelled;
                    continue;
                }

                break;
            }

            _points.Add(lastPos + direction * _remainingLaserTravel);
        
            _line.positionCount = _points.Count;
            _line.SetPositions(_points.ToArray());
        }
    }
}
