namespace GoogleVR.HelloVR
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(Collider))]
    //ALL OBJECT
    public class HunterGameObject : MonoBehaviour
    {

        //플레이어 시점에서 장전 시작.
        public void PlayerTriggerIt(BaseEventData eventData)
        {
            PointerEventData ped = eventData as PointerEventData;
            if(ped != null)
            {
                if (ped.button != PointerEventData.InputButton.Left)
                {
                    return;
                }
            }
        }

        //플레이어 샷
        public void PlayerShotIt(BaseEventData eventData)
        {

        }


        public void SetGazeAt(bool gazedAt)
        {

        }

        public void Recenter()
        {
#if !UNITY_EDITOR
            GvrCardboardHelpers.Recenter();
#else
            if (GvrEditorEmulator.Instance != null)
            {
                GvrEditorEmulator.Instance.Recenter();
            }
#endif
        }

    }



}