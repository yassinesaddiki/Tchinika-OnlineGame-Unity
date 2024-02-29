using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected int bulletDamage;
    [SerializeField] protected float timeBetweenShots;
    [SerializeField] protected int MagazineSize;
    [SerializeField] protected AmmoType ammoType;

    protected float lastTimeFired;
    protected AmmoSystem ammoSystem;
    public int GetMagazineSize()
    {
        return MagazineSize;
    }
    public AmmoType GetAmmoType()
    {
        return ammoType;
    }
    public void Reload()
    {
        ammoSystem.ReloadAmmo(ammoType);
    }

    public abstract void Fire();

    protected virtual void Start()
    {
        ammoSystem = FindObjectOfType<AmmoSystem>();
    }
}
