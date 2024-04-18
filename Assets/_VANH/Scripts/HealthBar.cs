using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    private float hp;
    private float maxHp;
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHp, Time.deltaTime * 5f);
    }

    public void OnInit(float maxHp)
    {
        this.maxHp = maxHp;
        hp = maxHp;
        imageFill.fillAmount = 1;
    }

    public void SetHealth(float hp)
    {
        this.hp = hp;
        // imageFill.fillAmount = hp / maxHp;
    }
}
