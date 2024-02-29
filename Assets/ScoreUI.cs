using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ScoreInfo;
    private int score { get; set; }

    private void Start()
    {
        score = 0;
    }
    public void AddScore(int amount)
    {
        score+= amount;
    }
    void UpdateScore()
    {
        ScoreInfo.text = $"{score}";
    }
    private void Update()
    {
        UpdateScore();
    }


}
