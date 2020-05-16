using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//rank 최종점수 저장.
public class RankManager : MonoBehaviour
{
    public Text High_text_1;
    public Text High_text_2;
    public Text High_text_3;

    private int HighScore;
    private int HighScore_2;
    private int HighScore_3;

    public Text My_Score;

    private void Start()
    {
        HighScore = PlayerPrefs.GetInt("Score");
        HighScore_2 = PlayerPrefs.GetInt("Score_2");
        HighScore_3 = PlayerPrefs.GetInt("Score_3");
    }

    private void Update()
    {
        High_text_1.text = HighScore.ToString();
        High_text_2.text = HighScore_2.ToString();
        High_text_3.text = HighScore_3.ToString();
    }

    //refer gameamanager
    public void RankInput(int score)
    {
        if(score > HighScore) //1
        {
            PlayerPrefs.DeleteKey("Score_3");
            PlayerPrefs.SetInt("Score_3", HighScore_2);
            PlayerPrefs.DeleteKey("Score_2");
            PlayerPrefs.SetInt("Score_2", HighScore);
            PlayerPrefs.DeleteKey("Score");
            PlayerPrefs.SetInt("Score", score);
            HighScore = PlayerPrefs.GetInt("Score");
            HighScore_2 = PlayerPrefs.GetInt("Score_2");
            HighScore_3 = PlayerPrefs.GetInt("Score_3");
            PlayerPrefs.Save();
        }
        else if((score < HighScore) && (score > HighScore_2)) //2
        {
            PlayerPrefs.DeleteKey("Score_3");
            PlayerPrefs.SetInt("Score_3", HighScore_2);
            PlayerPrefs.DeleteKey("Score_2");
            PlayerPrefs.SetInt("Score_2", score);
            HighScore_2 = PlayerPrefs.GetInt("Score_2");
            HighScore_3 = PlayerPrefs.GetInt("Score_3");
            PlayerPrefs.Save();
        }
        else if(score > HighScore_3) //3
        {
            PlayerPrefs.DeleteKey("Score_3");
            PlayerPrefs.SetInt("Score_3", score);
            HighScore_3 = PlayerPrefs.GetInt("Score_3");
            PlayerPrefs.Save();
        }
        PlayerPrefs.Save();
    }

}