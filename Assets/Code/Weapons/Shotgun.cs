using UnityEngine;
public class Shotgun : Weapon
{
    [Header("References")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Settings")]
    [SerializeField] private int pellets = 6;
    [SerializeField] private float spread = 8f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float fireRate = 0.8f;
    [SerializeField] private int damage = 20;

    [Header("Ammo")]
    [SerializeField] private int magazineCapacity = 8;
    [SerializeField] private int currentAmmo = 8;
    [SerializeField] private float reloadStartDelay = 0.5f;
    [SerializeField] private float reloadInterval = 0.5f;
    public override int CurrentAmmo => currentAmmo;
    public override int Damage => damage;

    private bool _isReloading;
    private Coroutine _reloadCoroutine;

    private bool _pickedUp;
    private float nextFireTime;
    public override void Pickup()
    {
        if (_pickedUp)
            return;

        _pickedUp = true;

        transform.SetParent(weaponHolder);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        Debug.Log("Groovy");
    }

    public override void Shoot()
    {
        if (_isReloading)
        {
            StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
            _isReloading = false;

            Debug.Log("Reload interrupted");
        }

        if (Time.time < nextFireTime)
            return; 

        if (currentAmmo <= 0)
        {
            Debug.Log("No ammo in magazine");
            return;
        }
        
        nextFireTime = Time.time + fireRate;

        currentAmmo--;

        Debug.Log("Shoot - Ammo left: " + currentAmmo);

        for (int i = 0; i < pellets; i++)
        {
            Quaternion rotation = muzzle.rotation;

            float randomX = Random.Range(-spread, spread);
            float randomY = Random.Range(-spread, spread);

            rotation *= Quaternion.Euler(randomX, randomY, 0f);

            GameObject projectile = Instantiate(
                projectilePrefab,
                muzzle.position,
                rotation
                );    

            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.Initialize(Damage);
            }

            Rigidbody rb = projectile.GetComponentInChildren<Rigidbody>();

            if (rb == null)
            {
                Destroy(projectile);
                return;
            }

            rb.linearVelocity = rotation * Vector3.forward * projectileSpeed;
        }
    }

    public override void Reload()
    {
        if (_isReloading)
            return;

        if (currentAmmo >= magazineCapacity)
            return;

        AmmoInventory ammoInventory = GetComponentInParent<AmmoInventory>();

        if (ammoInventory == null)
        {
            Debug.LogError("AmmoInventory not found in Player");
            return;
        }

        if (!ammoInventory.HasAmmo())
        {
            Debug.Log("No more ammo on reserve");
            return;
        }

        _reloadCoroutine = StartCoroutine(ReloadOneByOne(ammoInventory));
    }

    private System.Collections.IEnumerator ReloadOneByOne(AmmoInventory ammoInventory)
    {
        _isReloading = true;

        yield return new WaitForSeconds(reloadStartDelay);

        while (currentAmmo < magazineCapacity && ammoInventory.HasAmmo())
        {
            currentAmmo++;
            ammoInventory.TryUseAmmo();

            Debug.Log("Reloading... " + currentAmmo + "/" + magazineCapacity);

            yield return new WaitForSeconds(reloadInterval);
        }

        _isReloading = false;
        _reloadCoroutine = null;

        Debug.Log("Reload complete");
    }

}
