using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    private int speed;
    private Vector3 dir;
    void Start () {
        speed = 2;
        dir = new Vector3 (Random.Range (-0.5f, 0.5f), 1, Random.Range (-0.5f, 0.5f));
    }

    void Update () {
            // gameObject.transform.LookAt (gameObject.transform.position + dir);
            //gameObject.transform.position = gameObject.transform.position + dir * Time.deltaTime * (speed / 2);
            transform.position += (transform.position - Vector3.zero).normalized * Time.deltaTime;
            transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
    }

}
