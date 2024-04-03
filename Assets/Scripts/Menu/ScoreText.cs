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
        //This doesn't need to be called every frame, setScoreText should ideally only be called when the score changes
        //TODO: call setScoreText when placing building, instead of calling it here
        setScoreText(GameManager.Instance.CalculateTotalScore(GridManager.Instance.tileStates, GridManager.Instance.tileBonus))
    }

    void setScoreText(int score)
    {
        textMeshProText.text = "Score: " + score.ToString();
    }
}
