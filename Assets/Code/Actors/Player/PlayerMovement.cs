using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerAim))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Move speed of the character in m/s")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("Move speed while aiming")]
    [SerializeField] private float aimingSpeed = 2.5f;

    [Tooltip("Sprint speed of the character in m/s")]
    [SerializeField] private float sprintSpeed = 10f;

    [Tooltip("How fast the character turns to face movement direction")]
    [SerializeField][Range(0.0f, 0.3f)] private float rotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    [SerializeField] private float speedChangeRate = 10.0f;

    [Header("Input References")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction;

    private CharacterController _controller;
    private PlayerAim _playerAim;
    private Transform _cameraTransform;

    private Vector2 _moveDirection;

    private bool _sprint;

    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerAim = GetComponent<PlayerAim>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        ReadInput();
        Move();
    }

    private void ReadInput()
    {
        _moveDirection = moveAction.action.ReadValue<Vector2>();
        _sprint = sprintAction.action.IsPressed();
    }

    private void Move()
    {
        bool isAiming = _playerAim.IsAiming;

        float targetSpeed = isAiming ? aimingSpeed : (_sprint ? sprintSpeed : moveSpeed);

        if (_moveDirection == Vector2.zero)
            targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(
            _controller.velocity.x,
            0.0f,
            _controller.velocity.z
        ).magnitude;

        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(
                currentHorizontalSpeed,
                targetSpeed,
                Time.deltaTime * speedChangeRate
            );

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        if (_moveDirection != Vector2.zero)
        {
            Vector3 cameraForward = _cameraTransform.forward;
            Vector3 cameraRight = _cameraTransform.right;

            cameraForward.y = 0.0f;
            cameraRight.y = 0.0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 targetDirection =
                cameraForward * _moveDirection.y +
                cameraRight * _moveDirection.x;

            _targetRotation = Mathf.Atan2(
                targetDirection.x,
                targetDirection.z
            ) * Mathf.Rad2Deg;

            float rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                _targetRotation,
                ref _rotationVelocity,
                rotationSmoothTime
            );

            transform.rotation = Quaternion.Euler(
                0.0f,
                rotation,
                0.0f
            );

            _controller.Move(
                targetDirection.normalized *
                (_speed * Time.deltaTime)
            );
        }
    }
}