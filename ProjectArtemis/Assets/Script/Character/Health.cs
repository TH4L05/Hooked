using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public bool noHealthLeft;

    public void Start()
    {
        //GameEvents.OnCharacterHealthValueChanged += UpdateHealth;
        var character = transform.parent.GetComponentInParent<Character>();
        //id = transform.parent.gameObject.name;
        StartCoroutine("WaitASecond");
    }

    private void OnDestroy()
    {
        //GameEvents.OnCharacterHealthValueChanged -= UpdateHealth;
    }

    IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(1f);
    }

    public void UpdateHealth(string id, float value)
    {

        if (noHealthLeft) return;

        currentHealth += value;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
        }

        if (currentHealth == 0 && !noHealthLeft)
        {
            //GameEvents.OnCharacterNoHealthLeft?.Invoke(this.id);
            noHealthLeft = true;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        //Debug.Log($"The health of  been {currentHealth} health left");
        //GameEvents.UpdateUIBar.Invoke(this.id, currentHealth, maxHealth);
    }
}
