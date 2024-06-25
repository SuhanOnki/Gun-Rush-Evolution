using Engine.GameSections;
using TMPro;
using UnityEngine;

namespace Engine.Other
{
    public class EmptyBall : MonoBehaviour
    {
        //public static UnityEvent BallsOutLine = new UnityEvent();
        //public static UnityEvent VeryNearBall = new UnityEvent();

        [SerializeField] private Transform defaultSphere;
        [SerializeField] private float speed;
        [SerializeField] private GameObject particle;

        public int ballIndex;
        public int ballNumberPrevious;
        public int ballNumber;
        public TextMeshProUGUI ballText;
        public float localScaleBall;

        private GameplayMaestro gameplayMaestro;
        private float speed2;

        public void SpecialFunc()
        {
            
        }
        private void Start()
        {
            speed *= 1.5f;
            speed2 = speed / 1.8f;
            localScaleBall = transform.localScale.x;
            gameplayMaestro = GameplayMaestro.Instance;
            //VeryNearBall.AddListener(VeryNearBall_Invoke);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gun"))
            {
                defaultSphere.gameObject.SetActive(false);
            }

            if (other.CompareTag("Ball"))
            {
                Trigger(other);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                Trigger(other);
            }
        }

        private void Trigger(Collider other)
        {
            float z1 = defaultSphere.position.z, z2 = other.transform.parent.position.z;
            if (z1 != z2)
            {
                defaultSphere.position += new Vector3(0, 0, z1 - z2) * Time.deltaTime * speed * 5f;
            }
            else
            {
                defaultSphere.position += new Vector3(0, 0, 1) * Time.deltaTime * speed * 5f;
            }
        }
    }
}