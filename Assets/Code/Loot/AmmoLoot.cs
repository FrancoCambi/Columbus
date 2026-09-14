using System;
using UnityEngine;

public class AmmoLoot : Loot
{
    [Header("Settings")]
    [SerializeField] private int ammoAmount = 8;

    public override void Pickup()
    {
        AmmoInventory ammoInventory = FindFirstObjectByType<AmmoInventory>();
        
        if (ammoInventory == null)
        {
            Debug.LogError("AmmoInventory not found in Player");
            return;
        }

        ammoInventory.AddAmmo(ammoAmount);

        Destroy(gameObject);
    }
}
