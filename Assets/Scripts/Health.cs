using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public UnityEvent onDeath;
    public UnityEvent<float> onDamage;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        onDamage?.Invoke(amount);
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    void Death()
    {
        currentHealth = 0;
        onDeath?.Invoke();
    }
}
