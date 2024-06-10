using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int goalSlots = 0; //This is the number of slots that need to be powered to win
    public const int DIAGSCORE = 1; //This is the score for a diagonal connection
    public const int ADJSCORE = 2; //This is the score for an adjacent connection this will be the default for calclating the min score
    public const int BONUSSCORE = 3; //This is the score for a bonus tile
    public GameObject scoreText;
    public GameObject winOverlay;
    private int currentScore = 0;
    public int CurrentScore
    {
        get { return currentScore; }
        set
        {
            currentScore = value;
            scoreText.GetComponent<ScoreText>().setScoreText(currentScore);
            CheckWinCondition(currentScore);

        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// This function will calculate the score based on how many correct buildings the player has placed
    /// </summary>
    /// <param name="tileStates"></param>
    /// <param name="tileBonus"></param>
    /// <returns></returns>
    public int CalculateTotalScore(List<TileTypes> tileStates)
    {
        /// Generated with help from ChatGPT using following prompt:
        /// I want to constantly calculate total score of a grid. I have a list of grid states, and a list of bools, these lists map to the grid.
        /// so I want to check if the gridstates[i] has a building and if boolList[i] is true 
        /// unity c#

        int totalScore = 0; //reset score before changing it in loop below

        foreach (var tile in GridManager.Instance.scoreTiles)
        {
            int currentTile = GridManager.GetTileIndex(tile.position);
            if (tileStates[currentTile] == tile.building && WirePlacement.Instance.isTileConnected(currentTile))
            {
                totalScore += BONUSSCORE;
            }
            foreach (var adj in tile.adjacent)
            {
                int adjTile = GridManager.GetTileIndex(adj.position);
                if (tileStates[adjTile] == tile.building && WirePlacement.Instance.isTileConnected(adjTile))
                {
                    totalScore += ADJSCORE;
                }

            }
            foreach (var diag in tile.diagonals)
            {
                int diagTile = GridManager.GetTileIndex(diag.position);
                if (tileStates[diagTile] == tile.building && WirePlacement.Instance.isTileConnected(diagTile))
                {
                    totalScore += DIAGSCORE;
                }
            }
        }
        return totalScore; //This function will return the score as an int
    }

    /// <summary>
    /// This function will calculate the optimal score required to win
    /// </summary>
    public int CalculateScoreRequired()

    {
        //The Optimal Score is the number of open slots multiplied by the score for an adjacent connection
        return CalculateOpenSlots(GridManager.Instance.GetGoalTiles()) * ADJSCORE;
    }

    //Win Condition Requirements (Both or Either?)
    //The player must have a score equal to or greater than the score required to win
    //The player must have powered all slots connected to the goal



    /// <summary>
    /// This function will calculate the number of open slots that need to be powered to win
    /// </summary>
    /// <param name="goalTiles">This is the list of Goal tiles, assuming they are all adjacent to eachother</param>
    /// <returns></returns>
    public int CalculateOpenSlots(List<Vector2> goalTiles)
    {
        int openSlots = 0;
        foreach (Vector2 tile in goalTiles)
        {
            List<Vector2> adjTiles = new List<Vector2>()  //List of tiles directly next to the goal tile
            {
                new Vector2(tile.x + 1, tile.y),
                new Vector2(tile.x - 1, tile.y),
                new Vector2(tile.x, tile.y + 1),
                new Vector2(tile.x, tile.y - 1)
            };
            foreach (Vector2 adjTile in adjTiles)
            {
                if (!goalTiles.Contains(adjTile)) //ignore if the tile is also a goal tile
                {
                    openSlots++;
                }
            }
        }
        goalSlots = openSlots;
        return openSlots;
    }

    /// <summary>
    /// This function will check if the player has met the win condition
    /// </summary>
    /// <param name="score">The Score to check against the optimal</param>
    public void CheckWinCondition(int score)
    {
        float percentageScore = (float)score / CalculateScoreRequired();
        percentageScore *= 100;
        percentageScore = UnityEngine.Mathf.Round(percentageScore);
        percentageScore /= 100.0f;
        Debug.Log("PERCENTAGE SCORE:" + percentageScore);
        if (WirePlacement.Instance.ConnectedBuildings.Count >= goalSlots)
        {
            winOverlay.SetActive(true);
            winOverlay.transform.GetChild(1).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score;
            winOverlay.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = percentageScore;
            if (score >= CalculateScoreRequired())
            {

                Debug.Log("You Win!");
                winOverlay.transform.GetChild(2).gameObject.SetActive(false);
                winOverlay.transform.GetChild(3).gameObject.SetActive(true);

            }
            else
            {
                Debug.Log("You have powered all the goal slots, but you need a higher score to win");
                winOverlay.transform.GetChild(2).gameObject.SetActive(true);
                winOverlay.transform.GetChild(3).gameObject.SetActive(false);

            }
            PauseGame.Instance.Pause();
        }


        Debug.Log("OPTIMAL SCORE:" + CalculateScoreRequired());
        Debug.Log("CURRENT SCORE:" + score);
        Debug.Log("CONNECTED BUILDINGS:" + WirePlacement.Instance.ConnectedBuildings.Count);
        Debug.Log("GOAL SLOTS:" + goalSlots);
    }
}
