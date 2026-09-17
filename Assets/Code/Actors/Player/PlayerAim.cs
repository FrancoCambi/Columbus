using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundMask;

    private bool _isAiming;
    private Vector3 _aimPoint;

    public bool IsAiming => _isAiming;
    public Vector3 AimPoint => _aimPoint;
    private void Update()
    {
        _isAiming = Mouse.current != null && Mouse.current.rightButton.isPressed;
    }

    private void LateUpdate()
    {
        // Solo cambia el PlayerMovement cuando se activa el apuntado
        if (!_isAiming) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane aimPlane = new Plane(
            Vector3.up,
            new Vector3(0f, transform.position.y, 0f));

        if (aimPlane.Raycast(ray, out float distance))
        {
            _aimPoint = ray.GetPoint(distance);

            Vector3 direction = _aimPoint - transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {

                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
