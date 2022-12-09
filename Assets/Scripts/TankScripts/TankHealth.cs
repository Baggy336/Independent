using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    // Variables for UI to be set in the inspector
    public float initialHealth = 100f;
    public Slider slider;
    public Image fillImg;
    public Color fullHealthRGB = Color.green;
    public Color lessHealthRGB = Color.red;
    public GameObject explosive;

    /*
    private AudioSource explosionAudio;
    private ParticleSystem explosivePart;
    */
    private float currentHealth;
    private bool isDead;

    /*
    private void OnAwake()
    {
        explosivePart = Instantiate(explosive).GetComponent<ParticleSystem>();
        explosionAudio = explosivePart.GetComponent<AudioSource>();
        explosivePart.gameObject.SetActive(false);
    }
    */

    // Generate the initial health UI
    private void OnEnable()
    {
        currentHealth = initialHealth;
        isDead = false;

        SetHealthUI();
    }

    // Take damage, and update the UI
    public void TakeDamage(float amt)
    {
        currentHealth -= amt;
        SetHealthUI();
        if (currentHealth <= 0f && !isDead)
        {
            OnDeath();
        }
    }

    // Update the slider and color to match the health 
    private void SetHealthUI()
    {
        slider.value = currentHealth;
        fillImg.color = Color.Lerp(lessHealthRGB, fullHealthRGB, (currentHealth/initialHealth));
    }

    private void OnDeath()
    {
        isDead = true;
        /*
        explosivePart.transform.position = transform.position;
        explosivePart.gameObject.SetActive(true);

        explosionAudio.Play();
        explosivePart.Play();
        */
        gameObject.SetActive(false);
    }
}
