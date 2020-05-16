using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelect : MonoBehaviour
{
    public Text m_round; //라운드 명
    public Text m_goal;
    public Image[] m_shells;
    public void Initialize(){
        GameManager_Hunter gmr = FindObjectOfType<GameManager_Hunter>();
        foreach (var item in m_shells)
        {
            item.color = new Color(item.color.r,item.color.g,item.color.b,0.2f);
        }
        m_round.text = gmr.m_round;
        m_goal.text = GameManager_Hunter.Goal.ToString();
        FindObjectOfType<OBJ_Shotgun>().AddShells_UIEvent.AddListener(FillShellUI);
    }

    void FillShellUI()
    {
        int index = 8-FindObjectOfType<OBJ_Shotgun>().anim.GetInteger("Bullet");
        m_shells[index].color = new Color(m_shells[index].color.r,m_shells[index].color.g,m_shells[index].color.b,255);
    }
}
