using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLoot : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference pickupAction;

    [Header("Settings")]
    [SerializeField] private float pickupRange;

    private void Update()
    {
        Loot closestLoot = FindClosestLoot();

        if (closestLoot != null && pickupAction.action.WasPressedThisFrame())
            closestLoot.Pickup();
    }

    private Loot FindClosestLoot()
    {
        Loot closestLoot = null;
        float closestDistance = pickupRange;

        foreach (Loot loot in LootRegistry.Loots)
        {
            if (loot.transform.IsChildOf(transform))
                continue;

            float distance = Mathf.Sqrt((loot.transform.position - transform.position).sqrMagnitude);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestLoot = loot;
            }
        }

        return closestLoot;
        
    }
}