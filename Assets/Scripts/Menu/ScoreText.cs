using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script changes the gameObject text to the score value from game manager.
/// </summary>
/// <remarks>
/// This script should be added to a game object with a textmeshpro component
/// </remarks>
[RequireComponent(typeof(TextMeshProUGUI))] //This will add the TextMeshPro component to the game object if it doesn't already have one
public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI textMeshProText; //Text component

    void Awake()
    {
        textMeshProText = gameObject.GetComponent<TextMeshProUGUI>(); //searches for text component on the game object this script is attached to
    }

    void Update()
    {
        //This doesn't need to be called every frame, setScoreText should ideally only be called when the score changes
        //TODO: call setScoreText when placing building, instead of calling it here
        setScoreText(GameManager.Instance.CalculateTotalScore(GridManager.Instance.tileStates));
    }

    /// <summary>
    /// Updates the score textmeshpro text to the int being passed.
    /// </summary>
    /// <param name="score"></param>
    void setScoreText(int score)
    {
        if (textMeshProText == null)
        {
            Debug.LogError("Error: Missing TextMeshPro component. Please assign it in the inspector.", this);
            return; //exits the function since textmeshpro component is missing
        }

        textMeshProText.text = "Score: " + score.ToString();
    }
}
