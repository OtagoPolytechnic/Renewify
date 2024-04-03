using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI textMeshProText; // Assign this in the inspector

    // Start is called before the first frame update
    void Start()
    {
        textMeshProText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshProText.text = GameManager.Instance.CalculateTotalScore(GridManager.Instance.tileStates, GridManager.Instance.tileBonus).ToString();
    }
}
