using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Hunter : MonoBehaviour
{
    public static int Round = 1; //라운드 별 난이도 조정을 위한 Static 변수
    public static int Goal = 0; //라운드 별 목표 수를 달성 했는지?
    public static int Score = 0; //라운드 별 목표 수를 달성 했는지?
    public static bool IsShotable = false; //라운드 별 목표 수를 달성 했는지?
    public GameObject TitleScene;
    public GameObject StageScene;
    public GameObject ResultScene;
    public GameObject RankingScene;
    public Eagle_RespwanManager[] repawnPositions;
    [HideInInspector] public int m_timer; //타이머 
    [HideInInspector] public string m_round; //라운드 명
    float tmpTime = 0;
    int maxEagleAmound = 0;
    int maxEagleGoal = 0;
    public AudioSource SFX_StartArarm;
    public AudioSource SFX_TimeTicking;
    public AudioSource SFX_TimeEnd;
    public AudioSource UI_Button;
    public AudioSource UI_Failed;
    public AudioSource UI_Scroll;
    public AudioSource UI_Success;
    public AudioSource VOICE_laugh;
    public AudioSource VOICE_Radio_Fire;
    public AudioSource VOICE_Radio_Get_Ready;
    public AudioSource VOICE_Scream;

    void Awake()
    {
        TitleScene.SetActive(true);
        StageScene.SetActive(false);
        ResultScene.SetActive(false);
        RankingScene.SetActive(false);
        m_round = "Round 0";
    }
    
    public void GameStart()
    {
        Debug.Log("게임스타트");
        if (TitleScene.activeSelf)
            GameBegin();
        else if (RankingScene.activeSelf)
            SwitchScene(TitleScene);
    }


    private void SetRound()
    {
        VOICE_Radio_Get_Ready.Play();
        switch (GameManager_Hunter.Round)
        {
            case 1:
                m_round = "Round 1";
                maxEagleGoal = GameManager_Hunter.Goal = 5;
                m_timer = 12;
                maxEagleAmound = 10;
                break;
            case 2:
                m_round = "Round 2";
                maxEagleGoal = GameManager_Hunter.Goal = 7;
                m_timer = 12;
                maxEagleAmound = 20;
                break;
            case 3:
                m_round = "Round 3";
                maxEagleGoal = GameManager_Hunter.Goal = 10;
                m_timer = 12;
                maxEagleAmound = 30;
                break;
        }
        StageScene.GetComponent<UI_StageSelect>().Initialize();
        FindObjectOfType<OBJ_Shotgun>().Reload();
        StartCoroutine(SetStageTimer());
        switch (GameManager_Hunter.Round)
        {
            case 1:
                maxEagleAmound -= repawnPositions[Random.Range(0, 3)].Respawn(maxEagleAmound);
                break;
            case 2:
                maxEagleAmound -= repawnPositions[Random.Range(0, 3)].Respawn(Random.Range(1, maxEagleAmound / 2));
                maxEagleAmound -= repawnPositions[Random.Range(0, 3)].Respawn(maxEagleAmound);
                break;
            case 3:
                maxEagleAmound -= repawnPositions[Random.Range(0, 3)].Respawn(Random.Range(1, maxEagleAmound / 3));
                maxEagleAmound -= repawnPositions[Random.Range(0, 3)].Respawn(Random.Range(1, maxEagleAmound / 3));
                maxEagleAmound -= repawnPositions[Random.Range(0, 3)].Respawn(maxEagleAmound);
                break;
        }
    }

    public void UpdateRound(bool killEagle = false)
    {
        bool isEaglesAllDie = true;
        if (killEagle)
        {
            if (GameManager_Hunter.Goal > 0) GameManager_Hunter.Goal--;
            if (GameManager_Hunter.Goal == 0 && m_timer > 5){ m_timer = 5;}
            foreach (var eagle in FindObjectsOfType<Eagle_Prefab>())
            {
                if (eagle.isAlive)
                {
                    isEaglesAllDie = false;
                    break;
                }
            }
        }
        if (m_timer <= 0 || (killEagle && isEaglesAllDie))
        {
            SFX_TimeEnd.Play();
            GameManager_Hunter.IsShotable = false;
            SwitchScene(ResultScene);
            ResultScene.transform.Find("-get_goal").GetComponent<Text>().text = (maxEagleGoal-GameManager_Hunter.Goal).ToString()+"/"+maxEagleGoal.ToString()+"마리";
            ResultScene.transform.Find("-get_score").GetComponent<Text>().text = GameManager_Hunter.Score.ToString();
            ResultScene.transform.Find("-failed").gameObject.SetActive(true);
            ResultScene.transform.Find("-passed").gameObject.SetActive(true);
            if (GameManager_Hunter.Goal == 0)
            {
                ResultScene.transform.Find("-failed").gameObject.SetActive(false);
                UI_Success.Play();
                VOICE_Scream.Play();
            }
            else
            {
                ResultScene.transform.Find("-passed").gameObject.SetActive(false);
                UI_Failed.Play();
            }
            StartCoroutine(GetReadyNexyStage(5.0f));
        }
    }

    public void GameBegin()
    {
        UI_Button.Play();
        GameManager_Hunter.Round = 1;
        SwitchScene(StageScene);
    }

    public void GameRestart()
    {
        Score = 0;
        UI_Button.Play();
        SwitchScene(TitleScene);
    }

    public void SwitchScene(GameObject Scene = null, bool Active = true)
    {
        UI_Scroll.Play();
        TitleScene.SetActive(false);
        StageScene.SetActive(false);
        ResultScene.SetActive(false);
        RankingScene.SetActive(false);
        if (Active)
        {
            Scene.SetActive(true);
            if (StageScene.activeSelf) SetRound();
        }
        else
        {
            StopCoroutine(SetStageTimer());
        }
    }

    IEnumerator SetStageTimer()
    {
        yield return new WaitForSeconds(4);
        SFX_StartArarm.Play();
        yield return new WaitForSeconds(1);
        VOICE_Radio_Fire.Play();
        while (m_timer > 0)
        {
            tmpTime += Time.deltaTime;
            if (tmpTime >= 1)
            {
                tmpTime = 0;
                m_timer = m_timer - 1;
                if (m_timer < 5)
                    SFX_TimeTicking.Play();
                UpdateRound();
            }
            yield return null;
        }
    }

    IEnumerator GetReadyNexyStage(float time = 0)
    {
        yield return new WaitForSeconds(time);
        if (++GameManager_Hunter.Round > 3)
        {
            SwitchScene(RankingScene);
            FindObjectOfType<RankManager>().RankInput(Score);
            FindObjectOfType<RankManager>().My_Score.text = Score.ToString();

        }
        else
        {
            SwitchScene(StageScene);
        }
    }

}
