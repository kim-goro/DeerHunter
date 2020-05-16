using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            if(other.GetComponentInChildren<Eagle_Prefab>() != null)
            {
                if(other.GetComponentInChildren<Eagle_Prefab>().isAlive)
                {
                    FindObjectOfType<DamagePopupManager>().DamegePopup(other.transform.position,100);
                    other.GetComponentInChildren<Eagle_Prefab>().GetShot(100);
                }
            }
        }
    }
}
