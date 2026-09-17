using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference interactAction;

    [Header("Settings")]
    [SerializeField] private float interactionRange;

    private void Update()
    {
        Interactable closestInteractable = FindClosestInteractable();

        if (closestInteractable != null && interactAction.action.WasPressedThisFrame())
        {
            closestInteractable.Interact();
        }
    }

    private Interactable FindClosestInteractable()
    {
        Interactable closestInteractable = null;
        float closestDistance = interactionRange;

        Interactable[] interactables = FindObjectsByType<Interactable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(interactable.transform.position, transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        return closestInteractable;
    }
}