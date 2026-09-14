using System;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    public event Action OnAmmoObtained;

    [Header("Settings")]
    [SerializeField] private int ammo;

    public int Ammo => ammo;

    public void AddAmmo(int amount)
    {
        if (amount <= 0)
            return;

        ammo += amount;

        OnAmmoObtained?.Invoke();
    }

    public bool HasAmmo()
    {
        return ammo > 0;
    }

    public bool TryUseAmmo()
    {
        if (ammo <= 0)
            return false;

        ammo--;
        return true;
    }

}
