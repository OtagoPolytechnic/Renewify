using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int totalScore = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Update()
    {
        //TODO: This function doesn't need to be called every frame. It could be called when placing a building potentially?
        CalculateTotalScore(GridManager.Instance.tileStates, GridManager.Instance.tileBonus);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tileStates"></param>
    /// <param name="tileBonus"></param>
    /// <returns></returns>
    public int CalculateTotalScore(List<TileTypes> tileStates, List<bool> tileBonus)
    {
        /// Generated with help from ChatGPT using following prompt:
        /// I want to constantly calculate total score of a grid. I have a list of grid states, and a list of bools, these lists map to the grid.
        /// so I want to check if the gridstates[i] has a building and if boolList[i] is true 
        /// unity c#

        totalScore = 0; //reset score before changing it in loop below
        for(int i = 0; i < tileStates.Count; i++)
        {
            if(tileStates[i] == TileTypes.Windmills && tileBonus[i])
            {
                totalScore++;
            }
        }

        return totalScore;
    }
}
