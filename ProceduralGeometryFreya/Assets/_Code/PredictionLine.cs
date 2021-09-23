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
    [SerializeField] private float _launchSpeed;
    [Range(0.001f,0.01f)][SerializeField] private float _stepSize;
    [SerializeField] private int _iterationCount;
    private Vector3 _launchDirection => (_directionTransform.position - this.transform.position).normalized * _launchSpeed;


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
        
        Gizmos.color = Color.white;

        for (int i = 0; i < _iterationCount; i++)
        {
            float stepSize = i* _stepSize;
            float stepSize2 = stepSize * stepSize;
            
            Vector3 nextPos = pos + speed * stepSize + gravity * 0.5f * stepSize2;
            Gizmos.DrawLine(pos, nextPos);
            pos = nextPos;   
        }
    }
}