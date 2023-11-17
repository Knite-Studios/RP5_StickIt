using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Color Low;
    [SerializeField] Color High;
    [SerializeField] Vector3 offset;

    public void SetHealth(float health, float maxHealth)
    {
        healthBar.gameObject.SetActive(health < maxHealth);
        healthBar.value = health;
        healthBar.maxValue = maxHealth;

        healthBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, healthBar.normalizedValue);
    }
    void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
