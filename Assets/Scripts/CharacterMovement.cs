using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    private const string ANIM_RUNNING_BOOL = "isRunning";

    private float GRAVITY = 9.81f;

    [Header("References")]
    [SerializeField] private Rigidbody _rb = null;
    [SerializeField] private Animator _animator = null;

    [Header("Params")]
    [SerializeField] private float _walkSpeed = 1.7f;
    [SerializeField] private float _runSpeed = 3.0f;
    [SerializeField] private float _jumpForce = 10.0f;

    private Vector3 _moveVec = Vector3.zero;
    private bool _isRunning = false;
    private bool _isDisabled = false;

    public void SetDisabled(bool isDisabled)
    {
        _isDisabled = isDisabled;
    }

    private void Awake()
    {
        Debug.Assert(_rb != null, "_rb is not assigned!");
        Debug.Assert(_animator != null, "_animator is not assigned!");
    }

    // Update is called once per frame
    private void Update()
    {
        // _animator.SetBool(ANIM_RUNNING_BOOL, _moveVec.sqrMagnitude > 0);
        if (_isDisabled)
        {
            _moveVec = Vector2.zero;
        }
    }

    public void OnMove(InputValue inInputValue)
    {
        Vector2 moveVec = inInputValue.Get<Vector2>();
        // Debug.Log(_playerInput.currentControlScheme + " " + moveVec);

        _moveVec = new Vector3(moveVec.x, 0, moveVec.y);

        if (_isDisabled)
        {
            _moveVec = Vector2.zero;
        }

        FaceMoveVec();
    }

    private void FaceMoveVec()
    {
        if (_moveVec.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (_moveVec.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Debug.Log("Current moveVec: " + _moveVec);
        _rb.velocity = _moveVec.normalized * (_isRunning ? _runSpeed : _walkSpeed);
    }

    public void Jump(float inJumpForce)
    {
        _rb.AddForce(Vector3.up * inJumpForce);
    }
}
