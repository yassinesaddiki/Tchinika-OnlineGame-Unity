using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoInfoText;

    [SerializeField] private AmmoSystem ammoSystem;
    [SerializeField] private BaseWeapon currentWeapon;

    private void Start()
    {
        UpdateAmmoInfo();
    }
    public void SetCurrentWeapon(BaseWeapon weapon)
    {
        currentWeapon = weapon;
        UpdateAmmoInfo();
    }

    private void Update()
    {
        if (currentWeapon != null)
        {
            UpdateAmmoInfo();
        }
    }


    public void UpdateAmmoInfo()
    {
        if (currentWeapon != null && ammoSystem != null)
        {
            AmmoItem ammoItem = ammoSystem.GetAmmoItem(currentWeapon.GetAmmoType());

            if (ammoItem != null)
            {
                ammoInfoText.text = $"{ammoItem.currentAmmo}/{ammoItem.maxAmmo}";
            }
        }
    }
}
