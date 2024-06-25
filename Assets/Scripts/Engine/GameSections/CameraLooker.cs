using UnityEngine;

namespace Engine.GameSections
{
    public class CameraLooker : MonoBehaviour
    {
        Camera _cam;
        private void Start()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            Vector3 forward = _cam.transform.forward;
            transform.rotation = Quaternion.LookRotation(forward);
        }
        public void SpecialFunc()
        {
            
        }
    }
}
