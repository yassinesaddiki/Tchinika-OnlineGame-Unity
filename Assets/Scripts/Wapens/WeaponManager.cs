using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private BaseWeapon[] weapons;
    private int currentWeaponIndex;
    private AmmoSystem ammoSystem;

    private void Start()
    {
        ammoSystem = FindObjectOfType<AmmoSystem>();
        SwitchWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        // Handle switching weapons, reloading, etc.
        // ...
    }

    private void SwitchWeapon(int index)
    {
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        weapons[index].gameObject.SetActive(true);
        currentWeaponIndex = index;
    }
}
