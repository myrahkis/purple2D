using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillBar;
    public float health;

    // hurt sound
    public AudioClip ouchSound;

    // death sound
    // public AudioClip deathSound;

    public void LoseHealth(int value) {
        AudioSource.PlayClipAtPoint(ouchSound, transform.position);

        health -= value;

        fillBar.fillAmount = health / 100;

        if (health <= 0) {
            // Debug.Log("YOU DIED");
            // AudioSource.PlayClipAtPoint(deathSound, transform.position);
            FindObjectOfType<PlayerController>().Die();
        }
    }
}
