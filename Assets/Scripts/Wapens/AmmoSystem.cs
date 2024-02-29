using System.Collections.Generic;
using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    [SerializeField] private List<AmmoItem> ammoItems = new List<AmmoItem>();

    // Make GetAmmoItem public
    public AmmoItem GetAmmoItem(AmmoType ammoType)
    {
        return ammoItems.Find(item => item.ammoType == ammoType);
    }

    public void AddAmmo(AmmoType ammoType, int amount)
    {
        AmmoItem item = GetAmmoItem(ammoType);
        if (item != null)
        {
            item.AddAmmo(amount);
            item.ReduceMaxAmmo(amount);
        }
        else
        {
            Debug.LogError("Ammo type not found: " + ammoType);
        }
    }

    public bool UseAmmo(AmmoType ammoType, int amount)
    {
        AmmoItem item = GetAmmoItem(ammoType);
        if (item != null && item.UseAmmo(amount))
        {
            return true;
        }

        Debug.LogWarning("Not enough ammo for type: " + ammoType);
        return false;
    }

    public bool CanReload(AmmoType ammoType)
    {
        AmmoItem item = GetAmmoItem(ammoType);
        return item != null && item.currentAmmo < item.maxAmmo;
    }

    public void ReloadAmmo(AmmoType ammoType)
    {
        AmmoItem item = GetAmmoItem(ammoType);
        if (item != null)
        {
            int remainingAmmo = Mathf.Max(item.magazineSize - item.currentAmmo, 0);
            if (remainingAmmo > 0)
            {
                item.AddAmmo(remainingAmmo);
               
            }
            else
            {
                Debug.LogWarning("No need to reload, magazine is already full.");
                // Optionally, provide feedback (e.g., sound, UI message) that the player doesn't need to reload
            }
        }
        else
        {
            Debug.LogError("Ammo type not found: " + ammoType);
        }
    }
}
