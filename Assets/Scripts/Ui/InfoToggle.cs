using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoToggle : MonoBehaviour
{
    public GameObject InfoCard;


    private void Start()
    {
        InfoCard.SetActive(false);
    }
    public void OpenCard()
    {
        InfoCard.SetActive(true);
    }

    public void CloseCard()
    { InfoCard.SetActive(false); }

}
