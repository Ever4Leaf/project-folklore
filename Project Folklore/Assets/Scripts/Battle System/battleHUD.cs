using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Head-Up Display (status bar) for player and/or enemy
public class battleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
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
}
