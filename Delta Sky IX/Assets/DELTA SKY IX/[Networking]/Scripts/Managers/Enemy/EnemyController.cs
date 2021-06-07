using DeltaSky.Controllers.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace DeltaSky.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [Header("AI NavMesh")] public NavMeshAgent agent;
        private Vector3 newPos;
        
        private Transform _target;

        [Header("Enemy AI Stats")] public float chaseRadius = 5f;
        public float attackRadius = 2f;
        [SerializeField] private float speed = 5f;
        private float distance;
        private float moveSpeed;

        [Header("Alien Health")] 
        public Image healthRing;
        public float smoothSpeed;
        [SerializeField] private float currentHealth = 100f;
        private float maximumHealth = 100f;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            currentHealth = 100f;
            maximumHealth = 100f;
        }

        // Update is called once per frame
        void Update()
        {
            //ChasePlayer();

            smoothSpeed = 3f * Time.deltaTime; //To smooth transition from one colour to another
            Health();
            UpdateHealthRing();

            if (currentHealth <= 10f)
            {
                FleeFromPlayer();
            }
        }

        #region Related to Player

        
        
        public void ChasePlayer()
        {
            distance = Vector3.Distance(transform.position, _target.position);

            moveSpeed = speed * Time.deltaTime;

            if (distance <= chaseRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, moveSpeed);
            }

            if (distance <= attackRadius)
            {
                DamagePlayer(2f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                DamagePlayer(5f);
            }
        }

        public void DamagePlayer(float damagePoints)
        {
            Debug.Log("Player is taking damage");
        }

        #endregion

        #region Related to Enemies

        public void Health()
        {
            healthRing.fillAmount = Mathf.Lerp(healthRing.fillAmount, currentHealth / maximumHealth, smoothSpeed);
        }

        public void UpdateHealthRing()
        {
            Color healthCol = Color.Lerp(Color.red, Color.green, (currentHealth / maximumHealth));
            healthRing.color = healthCol;
        }

        public void TakeDamage(float damagePoints)
        {
            currentHealth -= damagePoints;

            if (currentHealth.Equals(0))
            {
                Die();
            }
        }

        /// <summary>
        /// Alien flees when health is below 15%
        /// </summary>
        public void FleeFromPlayer()
        {
            Vector3 directionToPlayer = transform.position - _target.position;
            newPos = transform.position + directionToPlayer;
            agent.SetDestination(newPos);
        }

        public void Die()
        {
            Destroy(gameObject);
            //InGameUI.instance.WinGame();
            Debug.Log("Enemies are dead!");
        }

        #endregion

        #region Visualisation

        /// <summary>
        /// Visible chase, attack & flee radius for Aliens (Testing)
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }

        #endregion
    }
}