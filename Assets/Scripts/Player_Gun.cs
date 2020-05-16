using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    public GameObject GvrController;

    public GameObject m_Player_Gun;//hierarchy
    public GameObject m_Bullet;//pref

    public float SpreadSize = 50;

    public float bullet_speed;
    public int bullet_count = 5;

    public float SpreadAngle;
    public GameObject bulletSpPoint;
    //List<Quaternion> bullets;

    private void Awake()
    {
        MeshRenderer ReticleMesh = GvrController.GetComponent<MeshRenderer>();
        ReticleMesh.enabled = false;
    }

    //pointer click
    public void Gun_Shot() //animation
    {
        m_Player_Gun.GetComponent<OBJ_Shotgun>().Shot();
    }

    void shot_Test()
    {
        Debug.Log("shot_test()");
        //foreach(Quaternion quat in bullets)
        //{
        for (int i = 0; i < bullet_count; i++)
        {
            float width = Random.Range(-1f, 1f) * SpreadSize;
            float height = Random.Range(-1f, 1f) * SpreadSize;

            GameObject b = Instantiate(m_Bullet, bulletSpPoint.transform.position, bulletSpPoint.transform.rotation);
            //b.transform.rotation = Quaternion.RotateTowards(b.transform.rotation,bullets[i],SpreadAngle);
            b.GetComponent<Rigidbody>().AddForce(b.transform.right * width + b.transform.up * height);
            b.GetComponent<Rigidbody>().AddForce(b.transform.forward * -bullet_speed);
        }
        //}
    }

    //bird
    public void Shot_Target()
    {

    }
    //sky
    public void Shot_Null()
    {

    }



}
