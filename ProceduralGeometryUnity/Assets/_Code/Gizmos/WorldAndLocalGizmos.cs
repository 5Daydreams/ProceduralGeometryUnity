using UnityEngine;

namespace _Code.Gizmos
{
    public class WorldAndLocalGizmos : MonoBehaviour
    {
        [SerializeField] private Transform _initialRef;
        [SerializeField] private Transform _newRef;
        [SerializeField] private Transform _point;
        [SerializeField] private Transform _testPointW2L;
        [SerializeField] private Transform _testPointL2W;
        [SerializeField] private bool _enableRefFrames = false;
        [SerializeField] private bool _enableL2W = false;
        [SerializeField] private bool _enableW2L = false;
        private Vector3 _initialRefPos => _initialRef.position;
        private Vector3 _newRefPos => _newRef.position;
        private Vector3 _pointPos => _point.position;


        private void OnDrawGizmos()
        {
            if (_initialRef == null || _newRef == null || _point == null)
            {
                return; // need all three points, otherwise you're a big cheater :v
            }

            if(_enableRefFrames)
            {
                DrawNewReferenceFrames();
            }

            if (_enableL2W)
            {
                WorldToLocal();
            }

            if (_enableW2L)
            {
                LocalToWorld();
            }
        }

        private void LocalToWorld()
        {
            Vector3 relativePos = _pointPos - _initialRefPos;

            float zPos = Vector3.Dot(_initialRef.forward, relativePos);
            float yPos = Vector3.Dot(_initialRef.up, relativePos);
            float xPos = Vector3.Dot(_initialRef.right, relativePos);

            Vector3 resultingVector = new Vector3(xPos, yPos, zPos);

            _testPointL2W.position = _newRefPos + _newRef.rotation * resultingVector;

            UnityEngine.Gizmos.color = Color.magenta;
            UnityEngine.Gizmos.DrawLine(_newRefPos,_testPointL2W.position);
            UnityEngine.Gizmos.DrawLine(_initialRefPos,_pointPos);
        }

        private void WorldToLocal()
        {
            Vector3 relativePos = _pointPos - _newRefPos;

            float zPos = Vector3.Dot(_newRef.forward, relativePos);
            float yPos = Vector3.Dot(_newRef.up, relativePos);
            float xPos = Vector3.Dot(_newRef.right, relativePos);

            Vector3 resultingVector = new Vector3(xPos, yPos, zPos);

            _testPointW2L.position = _initialRef.rotation * resultingVector;
        
            UnityEngine.Gizmos.color = Color.yellow;
            UnityEngine.Gizmos.DrawLine(_initialRefPos, _testPointW2L.position);
            UnityEngine.Gizmos.DrawLine(_newRefPos, _pointPos);
        }
    
        private void DrawNewReferenceFrames()
        {
            Quaternion newRefRotation = _newRef.rotation;
            Quaternion initRefRotation = _initialRef.rotation;

            UnityEngine.Gizmos.color = Color.red;
            UnityEngine.Gizmos.DrawLine(_newRefPos, _newRefPos + newRefRotation * Vector3.right);
            UnityEngine.Gizmos.DrawLine(_initialRefPos, _initialRefPos + initRefRotation * Vector3.right);

            UnityEngine.Gizmos.color = Color.green;
            UnityEngine.Gizmos.DrawLine(_newRefPos, _newRefPos + newRefRotation * Vector3.up);        
            UnityEngine.Gizmos.DrawLine(_initialRefPos, _initialRefPos + initRefRotation * Vector3.up);        
        
            UnityEngine.Gizmos.color = Color.blue;
            UnityEngine.Gizmos.DrawLine(_newRefPos, _newRefPos + newRefRotation * Vector3.forward);
            UnityEngine.Gizmos.DrawLine(_initialRefPos, _initialRefPos + initRefRotation * Vector3.forward);
        }
    }
}