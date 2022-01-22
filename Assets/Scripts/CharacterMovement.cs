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
    [SerializeField] private Transform _groundCheck = null;

    [Header("Params")]
    [SerializeField] private float _walkSpeed = 1.7f;
    [SerializeField] private float _runSpeed = 3.0f;
    [SerializeField] private float _jumpVelocity = 10f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2f;
    [SerializeField] private float _accel = 0.5f;

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
        Debug.Assert(_groundCheck != null, "_groundCheck is not assigned!");
    }

    // Update is called once per frame
    private void Update()
    {
        _animator.SetFloat("vel", _moveVec.sqrMagnitude);
        if (_isDisabled)
        {
            _moveVec = Vector2.zero;
        }

    }

    public void OnMove(InputValue inInputValue)
    {
        Vector2 moveVec = inInputValue.Get<Vector2>();
        // Debug.Log(_playerInput.currentControlScheme + " " + moveVec);

        _moveVec = new Vector3(moveVec.x, 0, 0);

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
        HandleJumpingFalling();
    }

    private void Move()
    {
        // Debug.Log("Current moveVec: " + _moveVec);
        if (Mathf.Abs(_rb.velocity.x) < _walkSpeed)
        {
            _rb.velocity += _moveVec * _accel;
        }
    }

    private void HandleJumpingFalling()
    {
        Ray ray = new Ray(_groundCheck.transform.position, _groundCheck.transform.forward);
        RaycastHit hit;

        bool isGrounded = Physics.Raycast(ray, out hit, 0.1f) && Mathf.Abs(_rb.velocity.y) <= 0.1f;
        bool jumpPressed = Keyboard.current[Key.Space].IsPressed();

        if (isGrounded && jumpPressed)
        {
            _rb.velocity += Vector3.up * _jumpVelocity;
        }
    }
}
