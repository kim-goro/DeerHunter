using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class OBJ_Shotgun : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    float shotDelay = 0f;
    float shotDelay_Define = 0.2f;
    public Transform ShellOut;
    public Transform FireOut;
    public GameObject OBJ_Cartridge;
    public GameObject Fx_GunFire;
    public AudioSource SFX_Reload;
    public AudioSource SFX_Reload_cloth;
    public AudioSource SFX_ReloadShells;
    public AudioSource SFX_Shot1;
    public AudioSource SFX_Shot2;
    public AudioSource SFX_EmptyAmmo;
    public AudioSource SFX_bullet_flyby1;
    public AudioSource SFX_bullet_flyby2;
    public UnityEvent AddShells_UIEvent = new UnityEvent();

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetInteger("Bullet", 0);
    }
    void Update()
    {
        if (shotDelay > 0)
        {
            shotDelay -= Time.deltaTime;
            if (shotDelay < 0)
            {
                shotDelay = 0;
            }
        }
    }

    public bool Shot()
    {
        if (GameManager_Hunter.IsShotable)
        {
            if (anim.GetInteger("Bullet") > 0 && shotDelay <= 0)
            {
                if (Random.value > 0.5f)
                {
                    SFX_Shot1.Play();
                }
                else
                {
                    SFX_Shot2.Play();
                }
                if (Random.value > 0.7f)
                {
                    if (Random.value > 0.5f) SFX_bullet_flyby1.Play();
                    else SFX_bullet_flyby2.Play();
                }
                anim.SetTrigger("Shot");
                shotDelay = shotDelay_Define;
                FindObjectOfType<GameManager_Hunter>().UpdateRound();
                return true;
            }
            else
            {
                SFX_EmptyAmmo.Play();
                return false;
            }
        }
        else{
            return false;
        }
    }

    public void Reload()
    {
        anim.SetInteger("Bullet", 0);
        anim.SetTrigger("Reload");
    }

    #region 애니메이션 이벤트 관련 함수
    public void Reload_Event()
    {
        SFX_Reload_cloth.Play();
    }
    public void Reload_Complete_Event()
    {
        SFX_Reload.Play();
    }
    public void AddShells_Event()
    {
        SFX_ReloadShells.Play();
        anim.SetInteger("Bullet", anim.GetInteger("Bullet") + 1);
        AddShells_UIEvent.Invoke();
    }

    public void Shot_Evnet()
    {
        anim.SetInteger("Bullet", anim.GetInteger("Bullet") - 1);
        Destroy(Instantiate(Fx_GunFire, FireOut.position, FireOut.rotation), 3.0f);
        GameObject fxo = Instantiate(OBJ_Cartridge, ShellOut.position, ShellOut.rotation);
    }
    #endregion
}
