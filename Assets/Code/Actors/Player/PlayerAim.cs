using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundMask;

    private bool _isAiming;

    public bool IsAiming => _isAiming;

    private void Update()
    {
        _isAiming = Mouse.current != null && Mouse.current.rightButton.isPressed;
    }

    private void LateUpdate()
    {
        // Solo cambia el PlayerMovement cuando se activa el apuntado
        if (!_isAiming) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 direction = hit.point - transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {

                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
