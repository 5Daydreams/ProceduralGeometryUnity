using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Quaternion initRefRotation = _initialRef.rotation;
        Vector3 relativePos = _pointPos - _initialRefPos;

        Vector3 rightVec = initRefRotation * Vector3.right;
        Vector3 upVec = initRefRotation * Vector3.up;
        Vector3 frontVec = initRefRotation * Vector3.forward;

        float zPos = Vector3.Dot(frontVec, relativePos);
        float yPos = Vector3.Dot(upVec, relativePos);
        float xPos = Vector3.Dot(rightVec, relativePos);

        Vector3 resultingVector = new Vector3(xPos, yPos, zPos);

        _testPointL2W.position = _newRefPos + _newRef.rotation * resultingVector;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_newRefPos,_testPointL2W.position);
        Gizmos.DrawLine(_initialRefPos,_pointPos);
    }

    private void WorldToLocal()
    {
        Quaternion newRefRotation = _newRef.rotation;
        Vector3 relativePos = _pointPos - _newRefPos;

        Vector3 rightVec = newRefRotation * Vector3.right;
        Vector3 upVec = newRefRotation * Vector3.up;
        Vector3 frontVec = newRefRotation * Vector3.forward;

        float zPos = Vector3.Dot(frontVec, relativePos);
        float yPos = Vector3.Dot(upVec, relativePos);
        float xPos = Vector3.Dot(rightVec, relativePos);

        Vector3 resultingVector = new Vector3(xPos, yPos, zPos);

        _testPointW2L.position = _initialRef.rotation * resultingVector;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_initialRefPos, _testPointW2L.position);
        Gizmos.DrawLine(_newRefPos, _pointPos);
    }

    private void DrawFromWorldToPoint()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_initialRefPos, _pointPos);
    }

    private void DrawFromLocalToPoint()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_newRefPos, _pointPos);
    }

    private void DrawNewReferenceFrames()
    {
        Quaternion newRefRotation = _newRef.rotation;
        Quaternion initRefRotation = _initialRef.rotation;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_newRefPos, _newRefPos + newRefRotation * Vector3.right);
        Gizmos.DrawLine(_initialRefPos, _initialRefPos + initRefRotation * Vector3.right);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_newRefPos, _newRefPos + newRefRotation * Vector3.up);        
        Gizmos.DrawLine(_initialRefPos, _initialRefPos + initRefRotation * Vector3.up);        
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_newRefPos, _newRefPos + newRefRotation * Vector3.forward);
        Gizmos.DrawLine(_initialRefPos, _initialRefPos + initRefRotation * Vector3.forward);
    }
}