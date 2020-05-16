using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky_Object : MonoBehaviour
{

    public GameObject Gun;
    public void Shot_Sky()
    {
        //GameObject.Find("shotgun").GetComponent<OBJ_Shotgun>().Shot();
        Gun.GetComponent<OBJ_Shotgun>().Shot(); //animation;
    }
}
