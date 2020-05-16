using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public GameObject DamagePopup_Prefab;
    public Transform ScreenSpace_Canvas;
    public void DamegePopup(Vector3 Position, int Damage = 0)
    {
        GameObject Dp = Instantiate(DamagePopup_Prefab, ScreenSpace_Canvas.transform);
        Dp.GetComponentInChildren<UnityEngine.UI.Text>().text = Damage.ToString();
        Dp.transform.position = Camera.main.WorldToScreenPoint(Position);

        Vector2 adjustedPosition = Camera.main.WorldToScreenPoint(Position);
        adjustedPosition.x *= ScreenSpace_Canvas.GetComponent<RectTransform>().rect.width / (float)Camera.main.pixelWidth;
        adjustedPosition.y *= ScreenSpace_Canvas.GetComponent<RectTransform>().rect.height / (float)Camera.main.pixelHeight;
        Dp.transform.localPosition = adjustedPosition - ScreenSpace_Canvas.GetComponent<RectTransform>().sizeDelta / 2f;
        Dp.transform.localPosition = new Vector3(Dp.transform.position.x,Dp.transform.position.y,0);


        if (Damage >= 100)
        {
            Dp.GetComponentInChildren<UnityEngine.UI.Text>().color = Color.red;
            Dp.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.yellow;
            Dp.GetComponentInChildren<Animator>().SetTrigger("Critical");
            Dp.transform.localScale *= 1.1f;
        }
        else if (Damage > 60)
        {
            Dp.GetComponentInChildren<UnityEngine.UI.Text>().color = Color.red;
            Dp.GetComponentInChildren<Animator>().SetTrigger("Critical");
        }
        Destroy(Dp,0.2f);
    }
}
