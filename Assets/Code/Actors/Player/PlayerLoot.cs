using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLoot : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference pickupAction;

    [Header("Settings")]
    [SerializeField] private float pickupRange;

    private PlayerFire _playerFire;

    private void Start()
    {
        _playerFire = GetComponent<PlayerFire>();
    }
    private void Update()
    {
        Loot closestLoot = FindClosestLoot();

        //PICKUP CON E
        if (closestLoot != null && pickupAction.action.WasPressedThisFrame())
        {
            if (closestLoot is Weapon newWeapon)
            {
                Weapon currentWeapon = _playerFire.GetCurrentWeapon();

                //SE SUELTA EN EL PISO ARMA ACTUAL Y SE AGARRA OTRA
                if (currentWeapon != null)
                {
                    currentWeapon.Drop(transform.position);
                }
                newWeapon.Pickup();
                _playerFire.SetWeapon(newWeapon);
            }
            else
            {
                closestLoot.Pickup();
            }
        }
        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            DropCurrentWeapon();
        }
    }

    private void DropCurrentWeapon()
    {
        if (_playerFire == null)
            return;

        Weapon currentWeapon = _playerFire.GetCurrentWeapon();

        if (currentWeapon == null)
            return;
        
        currentWeapon.Drop(transform.position);

        _playerFire.SetWeapon(null);
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