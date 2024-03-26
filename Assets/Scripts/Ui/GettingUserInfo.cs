using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GettingUserInfo : MonoBehaviour
{
    public TextMeshProUGUI UserName;
    private void Start()
    {
        UserName.text = $"Guest";
        if (PlayerPrefs.GetString("PlayerName", "Player") != null)
        {
            string playername = PlayerPrefs.GetString("PlayerName", "Player");
            UserName.text = playername;
        }
        
       
    }
}
