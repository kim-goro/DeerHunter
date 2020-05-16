using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum bulletClasses
{
    MainBullet,subBullet
}

public class Bullet : MonoBehaviour
{

    private void Start()
    {
        Destroy(this.gameObject, 4.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.CompareTag("BasicObject")) // eagle을 제외한 오브젝트에 충돌 할 경우.
        {
            Destroy(this.gameObject);
        }

        if (collision.other.CompareTag("TargetObject")) //target object - 
        {
            //find magager score ++
            //collision HP DOWN.
        }
    }

}
