using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class BaseWeaponOnline : NetworkBehaviour
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

    public abstract void Fire(GameObject _projectilePrefab, Vector3 _position, Quaternion _rotation);

    protected virtual void Start()
    {
        ammoSystem = FindObjectOfType<AmmoSystem>();
    }
}
