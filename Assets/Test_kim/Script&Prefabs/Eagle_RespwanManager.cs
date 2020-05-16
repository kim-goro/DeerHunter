using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle_RespwanManager : MonoBehaviour
{
    public GameObject Eagle_prefab;
    public GameObject KingEagle_prefab;
    public GameObject SpinEagle_prefab;
    public GameObject FX_Repawn;
    public AudioSource SFX_Repawn1;
    public AudioSource SFX_Respawn_leaves;
    public AudioSource SFX_Bird;

    public int Respawn(int amount = 0, float lifeTime = 20.0f)
    {
        StartCoroutine(DelayTimer(amount, lifeTime));
        return amount;
    }

    IEnumerator DelayTimer(int amount = 0, float lifeTime = 20.0f)
    {
        yield return new WaitForSeconds(5);
        FindObjectOfType<GameManager_Hunter>().SwitchScene(null, false);
        GameManager_Hunter.IsShotable = true;

        yield return new WaitForSeconds(Random.Range(2f, 5f));
        SFX_Repawn1.Play();
        SFX_Respawn_leaves.Play();
        if (Random.value > 0.7f)
        {
            SFX_Bird.Play();
        }
        Destroy(Instantiate(FX_Repawn, transform.position, transform.rotation), 10.0f);
        while (amount != 0)
        {
            Destroy(Instantiate(Eagle_prefab, transform.position, transform.rotation), lifeTime);
            amount--;
            if (GameManager_Hunter.Round > 1)
            {
                if (Random.value > 0.9f)
                {
                    Destroy(Instantiate(KingEagle_prefab, transform.position, transform.rotation), lifeTime);
                    continue;
                }
                if (Random.value > 0.9f)
                {
                    Destroy(Instantiate(SpinEagle_prefab, transform.position, transform.rotation), lifeTime);
                    continue;
                }
            }
        }
    }

}

