using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyPath _enemyPath = null;
    [SerializeField] private Transform[] _segments = null;
    [SerializeField] private Rigidbody _headSeg = null;

    [SerializeField] private float _separation = 0.2f;
    [SerializeField] private float _smoothSpeed = 0.3f;
    [SerializeField] private float _topSpeed = 2f;
    [SerializeField] private float _accel = 0.5f;
    [SerializeField] private float _decel = 0.5f;
    [SerializeField] private float _reachThreshold = 0.2f;
    [SerializeField] private bool _loopWaypoints = false;

    private Vector3 _currVel;
    private int _currentWayPointIndex = 0;
    private bool _isUpdating = true;
    private Transform[] _wayPoints = null;

    public void SetPath(EnemyPath enemyPath)
    {
        _wayPoints = enemyPath.WayPoints;
    }

    public void SetSegments(Transform[] segments)
    {
        _segments = segments;
        if (_segments.Length <= 1) { return; }
        _headSeg = segments[0].GetComponent<Rigidbody>();
    }

    public void SetIsUpdating(bool isUpdating)
    {
        _isUpdating = isUpdating;
    }

    private void Start()
    {
        if (_enemyPath != null)
        {
            _wayPoints = _enemyPath.WayPoints;
        }
    }

    private void Update()
    {
        if (!_isUpdating)
        {
            _currVel = Vector3.zero;
            return;
        }

        FollowWayPoints();
        UpdateSegments();
    }

    private void FollowWayPoints()
    {
        if (_currentWayPointIndex >= _wayPoints.Length || _segments.Length <= 0)
        {
            _currVel = Vector3.zero;

            if (_loopWaypoints)
            {
                _currentWayPointIndex = 0;
            }
            return;
        }

        Transform currWayPoint = _wayPoints[_currentWayPointIndex];
        Vector3 destinationPos = currWayPoint.position;
        Transform headSeg = _segments[0];
        Vector3 headSegPos = _segments[0].position;
        Vector3 targetDir = (destinationPos - headSegPos).normalized;

        _currVel += targetDir * _accel;
        _currVel = Vector3.ClampMagnitude(_currVel, _topSpeed);

        if (Vector3.Distance(headSegPos, destinationPos) <= _reachThreshold)
        {
            _currentWayPointIndex++;
        }
    }

    private void FixedUpdate()
    {
        if (_segments.Length <= 0) return;
        _headSeg.velocity = _currVel;
    }

    private void UpdateSegments()
    {
        for (int i = 1; i < _segments.Length; i++)
        {
            Transform previousSeg = _segments[i - 1];
            Transform segment = _segments[i];

            Vector3 prevSegPos = previousSeg.position;
            Vector3 segPos = segment.position;
            Vector3 targetPos = prevSegPos + (segPos - prevSegPos).normalized * _separation;

            segment.transform.position = Vector3.Slerp(segPos, targetPos, _smoothSpeed);
        }
    }
}
