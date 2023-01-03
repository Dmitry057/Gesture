using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public float health = 100;
    private float fullHealth;
    [SerializeField] private Slider sliderHP;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject head;
    private bool isDied;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fullHealth = health;
    }
    public void TakeDamage(float damage)
    {
        if (!isDied)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
            sliderHP.value = health / fullHealth;
        }
    }
    private void Die()
    {
        head?.SetActive(false);
        isDied = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Vector3.back, ForceMode.Impulse);
        sliderHP.gameObject.SetActive(false);
    }
}
