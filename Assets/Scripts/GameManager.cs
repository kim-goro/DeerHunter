using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//gameManager
public enum state
{
    Title,StageSelect,InGame,Result
}

public class GameManager : MonoBehaviour
{
    public state GameState;
    //title obejct
    public GameObject TitlePanel;
    //select object

    //ingame object
    public GameObject Player_Control_Panel;
    //result object
    private void Start()
    {
        GameState = state.Title;
    }
    private void Update()
    {
        if(GameState == state.Title)
        {
            state_Title();
        }
        else if(GameState == state.StageSelect)
        {
            state_StageSelect();
        }
        else if(GameState == state.InGame)
        {
            state_InGame();
        }
        else if(GameState == state.Result)
        {
            state_Result();
        }
    }

    //setting
    public void state_Title()
    {

    }
    public void state_StageSelect()
    {

    }
    public void state_InGame()
    {
     
    }
    public void state_Result()
    {

    }





}
