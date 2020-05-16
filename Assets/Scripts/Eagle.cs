namespace GoogleVR.HelloVR
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(Collider))]
    public class Eagle : MonoBehaviour
    {
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