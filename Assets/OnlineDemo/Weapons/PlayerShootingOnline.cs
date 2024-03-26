using System;
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
    public AnimatedTexture muzzleFlashAnimation;

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
        if (!IsOwner)
        {
            Debug.Log("Player is not the owner. Cannot shoot.");
            return;
        }

        fireContinuously = inputValue.isPressed;

        if (fireContinuously)
        {
            float timeSinceLastFire = Time.time - lastTimeFired;
            if (timeSinceLastFire >= timeBetweenShots)
            {
                if(IsServer)
                {
                    SpawnTheProjectile();
                }
                else
                {
                    SpawnTheProjectileServerRPC();
                }
                lastTimeFired = Time.time;
            }
            else
            {
                Debug.Log("Fire rate limit reached. Cannot shoot.");
            }
        }
    }

    [ServerRpc(RequireOwnership =false)]
    private void SpawnTheProjectileServerRPC()
    {
        SpawnTheProjectile();
    }

    private void SpawnTheProjectile()
    {
        Fire(projectilePrefab, firePoint.position, firePoint.rotation); 
    }

    public override void Fire(GameObject _projectilePrefab, Vector3 _position, Quaternion _rotation)
    {
        //Here Where I desactivate Consuming Ammo
        if (ammoSystem.UseAmmo(ammoType, 0))
        {
            GameObject projectile = Instantiate(_projectilePrefab, _position, _rotation);
            projectile.GetComponent<ProjectilesOnline>().Speed = bulletSpeed;
            projectile.GetComponent<ProjectilesOnline>().Damage = bulletDamage;
            projectile.GetComponent<NetworkObject>().Spawn(true);
            Debug.Log("Bullet spawned locally on client.");

            // Play muzzle flash animation
            if (muzzleFlashAnimation != null)
            {
                muzzleFlashAnimation.ResetAnimation(); // Reset to first frame
                muzzleFlashAnimation.Play();
            }
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
