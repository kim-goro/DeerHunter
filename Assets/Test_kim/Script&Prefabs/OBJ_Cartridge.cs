using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_Cartridge : MonoBehaviour
{
    public AudioSource OBJ_BulletFall;
    public AudioSource OBJ_BulletFall2;
    bool isfall = false;

    void Start()
    {
        transform.Rotate(0,0,90);
        Destroy(gameObject,5.0f);
        GetComponent<Rigidbody>().AddForce(Vector3.right *500f);
    }
    void OnCollisionEnter(Collision collision)
    {
        if(!isfall)
        {
            isfall = true;
            if(Random.value > 0.5f) OBJ_BulletFall.Play();
            else OBJ_BulletFall2.Play();
        }
    }
}
