using System;
using UnityEngine;

public abstract class Weapon : Loot
{
    public abstract int CurrentAmmo {  get; }
    public abstract int Damage { get; }
    public abstract void Shoot();
    public abstract void Reload(Action onReload);
    public abstract void Drop(Vector3 dropPosition);
}
