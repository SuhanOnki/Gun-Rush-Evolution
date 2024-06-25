using UnityEngine;
using UnityEngine.UI;

namespace Engine.Oponent
{
    public class VitalityBar : MonoBehaviour
    {
        public Slider damagedSlider;
        public Slider healthSlider;
        public float health;
        public float speed;
        public bool healthChanging;
        private bool damaged;
        private EnemyController enemyController;

        void Start()
        {
            enemyController = transform.parent.GetComponent<EnemyController>();
            damagedSlider.maxValue = enemyController.health;
            healthSlider.maxValue = enemyController.health;
            damagedSlider.value = damagedSlider.maxValue;
            healthSlider.value = healthSlider.maxValue;
            health = enemyController.health;
        }

        public void SpecialFunc()
        {
            
        }
        private void Update()
        {
            /*
                if (health < 0)
                {
                    zombieControl.health = 0;
                }*/

            if (healthChanging)
            {
                healthSlider.value = Mathf.MoveTowards(healthSlider.value, health, Time.deltaTime * 40 + speed);
                if (Mathf.Abs(healthSlider.value - health) < 0.01f)
                {
                    healthChanging = false;
                    damaged = true;
                }
            }

            if (damaged)
            {
                damagedSlider.value =
                    Mathf.MoveTowards(damagedSlider.value, healthSlider.value, Time.deltaTime * 30 + speed);
                if (Mathf.Abs(damagedSlider.value - health) < 0.01f)
                {
                    damaged = false;
                }
            }
        }

        public void TakeDamage(float damage)
        {
            speed = damage / 4;
            health -= damage;
            healthChanging = true;
        }
    }
}