using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Frog_Move : MonoBehaviour
{
    Animator anim;
    public Transform[] Target;
    NavMeshAgent agent;
    public int Number = 0;

    float jumpCount;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(ChangeNumber());
        StartCoroutine(Walk());
        jumpCount = Random.RandomRange(0.1f, 0.5f);
       
    }
    IEnumerator jump()
    {
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Walk());
    }
    IEnumerator Walk()
    {
        anim.SetTrigger("Crawl");
        yield return new WaitForSeconds(jumpCount);
        StartCoroutine(jump());
    }

    IEnumerator ChangeNumber()
    {
        Number = Random.RandomRange(0, 4);
        Debug.Log(Number);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ChangeNumber());
    }

    void Update()
    {
        agent.SetDestination(Target[Number].position);
    }
}
