using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Head-Up Display (status bar) for player and/or enemy
public class battleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpSlider;
    public Slider apSlider;

    public void setHUD(unitModifier unitMod)
    {
        nameText.text = unitMod.unitName;
        levelText.text = "Lvl." + unitMod.unitLevel;
        hpSlider.maxValue = unitMod.maxHP;
        hpSlider.value = unitMod.currentHP;
        apSlider.maxValue = unitMod.maxAP;
        apSlider.value = unitMod.currentAP;
    }

    public void setHP (int HP)
    {
        hpSlider.value = HP;
    }

    public void setAP (int AP)
    {
        apSlider.value = AP;
    }
}
