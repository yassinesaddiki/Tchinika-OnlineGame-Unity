using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class AmmoItem
{
    public AmmoType ammoType;
    public int maxAmmo;
    public int currentAmmo;
    public int magazineSize;

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }

    public void ReduceMaxAmmo(int amount)
    {
        // Reduce maxAmmo, but ensure it doesn't go below magazineSize
        maxAmmo = Mathf.Max(maxAmmo - amount, 0);
    }

    public bool UseAmmo(int amount)
    {
        if (currentAmmo >= amount)
        {
            currentAmmo -= amount;
            return true;
        }
        return false;
    }
}

public enum AmmoType
{
    Pistol,
    Rifle,
    // Add more ammo types as needed
}
