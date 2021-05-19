using DeltaSky.Controllers.UI;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour
    {
        public static Temp temp;

        private void Awake()
        {
            temp = this;
        }

        public GameObject player;
        public float health = 100f;
        private float _maxHealth = 100f;
        private float smoothSpeed;
        public Image healthRing;

        private void Update()
        {
            if(health.Equals(_maxHealth))
            {
                health = _maxHealth;
            }
            
            smoothSpeed = 3f * Time.deltaTime; //To smooth transition from one colour to another
            Health();
            UpdateHealthRing();
        }

        public void Health()
        {
            healthRing.fillAmount = Mathf.Lerp(healthRing.fillAmount, health / _maxHealth, smoothSpeed);
        }

        public void UpdateHealthRing()
        {
            Color healthCol = Color.Lerp(Color.red, Color.green, (health/_maxHealth));
            healthRing.color = healthCol;
        }
    }

