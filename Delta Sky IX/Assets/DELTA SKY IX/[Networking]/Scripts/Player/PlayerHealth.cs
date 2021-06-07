using DeltaSky.Controllers.UI;
using Mirror.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    public Image healthRing;
    public float smoothSpeed;
    [SerializeField] private float currentHealth = 100f;
    private float maximumHealth = 100f;

    [Header("Player Stats")]
    public Rigidbody rb;
    public NetworkRigidbody networkRb;
    
    // Start is called before the first frame update
    void Start() {
        currentHealth = 100f;
        maximumHealth = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        smoothSpeed = 3f * Time.deltaTime; //To smooth transition from one colour to another
        CalculateHealth();
        UpdateHealthRing();
    }

    public void CalculateHealth() {
        healthRing.fillAmount = Mathf.Lerp(healthRing.fillAmount, currentHealth / maximumHealth, smoothSpeed);
    }

    public void UpdateHealthRing() {
        Color healthCol = Color.Lerp(Color.red, Color.green, (currentHealth / maximumHealth));
        healthRing.color = healthCol;
    }
    
    public void TakeDamage(float damagePoints)
    {
        currentHealth -= damagePoints;

        if (currentHealth <= 0)
        {
            Time.timeScale = 0;
            Die();
        }
    }

    public void Die() {
        InGameUI.instance.GameOver();
    }
}
