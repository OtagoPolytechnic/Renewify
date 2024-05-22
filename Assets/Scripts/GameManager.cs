using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int goalSlots = 0; //This is the number of slots that need to be powered to win
    public int totalScore = 0; //DEBUG: this can be removed later, it's just here to see the score in the editor.
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
    void Update()
    {
        //TODO: This function doesn't need to be called every frame. It could be called when placing a building potentially?
        CalculateTotalScore(GridManager.Instance.tileStates);
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

        totalScore = 0; //reset score before changing it in loop below
        for (int i = 0; i < tileStates.Count; i++)
        {
            //TODO: allow this function to specify which type of bonus tile this is (wind, solar, water)
            //Note from Palin: Added a check if the tile is connected to the goal
            // if (tileStates[i] == TileTypes.Windmills && tileBonus[i] && WirePlacement.Instance.isTileConnected(i)) //This checks if the player has a windmill on a bonus tile. 
            // {
            //     totalScore++; //increments score by 1
            // }
            
        }
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


//If the player is allowed to score more than this, how should we handle testing for a win condition?
    public int CalculateScoreRequired()
    {
        return GridManager.Instance.scoreTiles.Count * ADJSCORE;
    }

    //Win Condition Requirements (Both or Either?)
        //The player must have a score equal to or greater than the score required to win
        //The player must have powered all slots connected to the goal
    // public bool CheckWinCondition()
    // {
    //     return totalScore >= CalculateScoreRequired() && WirePlacement.Instance.ConnectedBuildings.Count >= slots;
    // }


//This is assuming that the all goal tiles adjacent to eachother
    public int CalculateOpenSlots(List<Vector2> goalTiles)
    {
        int openSlots = 0;
        foreach (Vector2 tile in goalTiles)
        {
            List<Vector2> adjTiles = new List<Vector2>()
            {
                new Vector2(tile.x + 1, tile.y),
                new Vector2(tile.x - 1, tile.y),
                new Vector2(tile.x, tile.y + 1),
                new Vector2(tile.x, tile.y - 1)
            };
            foreach (Vector2 adjTile in adjTiles)
            {
                if (!goalTiles.Contains(adjTile))
                {
                    openSlots++;
                }
            }
        }
        goalSlots = openSlots;
        return openSlots;
    }

    public void CheckWinCondition(int score)
    {
        if (score >= CalculateScoreRequired() && WirePlacement.Instance.ConnectedBuildings.Count >= goalSlots)
        {
            winOverlay.SetActive(true);
        }
        Debug.Log("OPTIMAL SCORE:" + CalculateScoreRequired());
        Debug.Log("CURRENT SCORE:" + score);
        Debug.Log("CONNECTED BUILDINGS:" + WirePlacement.Instance.ConnectedBuildings.Count);
        Debug.Log("GOAL SLOTS:" + goalSlots);
    }	
}
