using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShootingOnline : BaseWeaponOnline
{
    private bool fireContinuously;
    [SerializeField] private AmmoUI ammoUI;
    [SerializeField] private AmmoSystem ammoSystem;

    private void Start()
    {
        // Assuming you have a reference to AmmoUI
        if (ammoUI != null)
        {
            //We need to Change theAmmoUi Too
            //ammoUI.SetCurrentWeapon(this);
        }
    }


    private void OnFire(InputValue inputValue)
    {
        fireContinuously = inputValue.isPressed;

        if (fireContinuously)
        {
            float timeSinceLastFire = Time.time - lastTimeFired;
            if (timeSinceLastFire >= timeBetweenShots)
            {
                Fire();
                lastTimeFired = Time.time;
            }
        }
    }
    public override void Fire()
    {
        if(!IsOwner) return;
        if (ammoSystem.UseAmmo(ammoType, 1))
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<NetworkObject>().Spawn(true);
            projectile.GetComponent<ProjectilesOnline>().Speed = bulletSpeed;
            projectile.GetComponent<ProjectilesOnline>().Damage = bulletDamage;
        }
    }


    private void OnReload(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            Reload();
        }
    }

    // Add this method for reloading
    private void Reload()
    {
        int remainingAmmo = Mathf.Max(MagazineSize - ammoSystem.GetAmmoItem(ammoType).currentAmmo, 0);
        if (remainingAmmo > 0)
        {
            ammoSystem.AddAmmo(ammoType, remainingAmmo);
            ammoUI.UpdateAmmoInfo(); // Update UI after reloading
            // Optionally, you can add a sound effect or other feedback for reloading
        }
        else
        {
            Debug.LogWarning("No need to reload, magazine is already full.");
            // Optionally, provide feedback (e.g., sound, UI message) that the player doesn't need to reload
        }
    }
}
