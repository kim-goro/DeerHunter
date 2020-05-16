using System.Collections;
using UnityEngine;

public class Eagle_Prefab : MonoBehaviour {
    private int hp;
    private int speed;
    private Vector3 dir;
    private Animator anim;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public int totalDamage = 0;
    public GameObject FX_Hit;
    public GameObject FX_eagleDead;
    public AudioSource SFX_DeadHit;
    public AudioSource SFX_CriticalHit;
    public AudioSource SFX_Hit;
    public bool isKingEagle = false;
    public bool isSpinEagle = false;
    float changeDirTime = 0;
    Vector3 SpawnPosition = Vector3.zero;

    void Start () {
        hp = 100;
        speed = Random.Range (18, 29);
        dir = new Vector3 (Random.Range (-0.5f, 0.5f), 1, Random.Range (-0.5f, 0.5f));
        anim = GetComponentInChildren<Animator> ();
        anim.SetTrigger ("Fly");
        anim.SetFloat ("Speed", speed);
        anim.speed = speed * 0.2f;
        if (isKingEagle) {
            hp += 60;
            speed += 10;
        }
        if(isSpinEagle)
        {            
            SpawnPosition = transform.position;
        }
    }

    void Update () {
        if (isAlive) {
            gameObject.transform.LookAt (gameObject.transform.position + dir);
            gameObject.transform.position = gameObject.transform.position + dir * Time.deltaTime * (speed / 2);
            if (isKingEagle) {
                changeDirTime += Time.deltaTime;
                if(changeDirTime >= 1)
                {
                    changeDirTime = 0;
                    dir = new Vector3 (Random.Range (-1f, 1f), 1, Random.Range (-1f, 1f));
                }
            }
            if(isSpinEagle)
            {
                transform.position += new Vector3((transform.position - SpawnPosition).x,0,(transform.position - SpawnPosition).z).normalized * (6 *  Time.deltaTime) ;
                transform.RotateAround(SpawnPosition, Vector3.up, 150 * Time.deltaTime);
            }
        }
    }

    public void GetShot (int Damage = 20) {
        if (Damage <= 0) return;
        FindObjectOfType<DamagePopupManager> ().DamegePopup (transform.position, Damage);
        GameManager_Hunter.Score += Damage * 2;
        totalDamage = 0;
        if (isAlive) {
            Destroy (Instantiate (FX_Hit, transform.position, transform.rotation), 1.0f);
            hp -= Damage;
            if (hp <= 0) {
                SFX_DeadHit.Play ();
                isAlive = false;
                GameObject fxd = Instantiate (FX_eagleDead, transform.position, Quaternion.Euler (Vector3.right * 90));
                Destroy (fxd, 3.0f);
                Dead ();
            } else {
                if (Damage >= 40) {
                    SFX_CriticalHit.Play ();
                } else {
                    SFX_Hit.Play ();
                }
            }
        }
    }

    private void Dead () {
        if (isAlive) isAlive = false;
        if (this.ContainsParam (anim, "Dead") && !isAlive) {
            anim.SetTrigger ("Dead");
            anim.speed = 1.0f;
            GetComponent<Rigidbody> ().isKinematic = false;
            GetComponent<Rigidbody> ().useGravity = true;
            Destroy (gameObject, 10.0f);
            FindObjectOfType<GameManager_Hunter> ().UpdateRound (true);
            GameManager_Hunter.Score += 1000;
            if(isKingEagle) GameManager_Hunter.Score += 500;
            if(isSpinEagle) GameManager_Hunter.Score += 500;
        }
    }

    private bool ContainsParam (Animator _Anim, string _ParamName) //애니메이션 파라미터에 있으면 실행
    {
        foreach (AnimatorControllerParameter param in _Anim.parameters) {
            if (param.name == _ParamName) return true;
        }
        return false;
    }

    void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.tag == "Monster") {
            Physics.IgnoreCollision (collision.collider, GetComponent<Collider> ());
        }
    }

}