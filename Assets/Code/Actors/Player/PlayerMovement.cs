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

    [Header("Gravity")]
    [Tooltip("Fuerza de la gravedad aplicada al jugador")]
    [SerializeField] private float gravity = -9.81f;

    [Header("Input References")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction;

    private CharacterController _controller;
    private PlayerAim _playerAim;

    private Vector2 _moveDirection;
    private float _verticalVelocity; // Controla la caída acumulada

    private bool _sprint;

    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerAim = GetComponent<PlayerAim>();
    }

    private void Update()
    {
        ReadInput();
        ApplyGravity();
        Move();
    }

    private void ReadInput()
    {
        _moveDirection = moveAction.action.ReadValue<Vector2>();
        _sprint = sprintAction.action.IsPressed();
    }

    private void ApplyGravity()
    {
        // Si toca el suelo, reseteamos la velocidad vertical a un valor negativo pequeño para mantenerlo pegado
        if (_controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            // Acumula la aceleración por gravedad en cada frame
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        bool isAiming = _playerAim.IsAiming;

        float targetSpeed = isAiming ? aimingSpeed : (_sprint ? sprintSpeed : moveSpeed);

        if (_moveDirection == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        Vector3 inputDirection = new Vector3(_moveDirection.x, 0.0f, _moveDirection.y).normalized;

        if (_moveDirection != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // Combinamos la velocidad de movimiento horizontal con la velocidad de caída vertical (Eje Y)
        Vector3 motion = (targetDirection.normalized * _speed) + new Vector3(0.0f, _verticalVelocity, 0.0f);

        _controller.Move(motion * Time.deltaTime);
    }
}