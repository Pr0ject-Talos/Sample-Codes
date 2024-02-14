using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPSystem : MonoBehaviour
{


    public int level;
    public float currentEXP;
    public float requiredEXP;
    GameObject Player;
    
    private float lerpTimer;
    private float delayTimer;
    // float LVLHP = 10;
    
    public Image frontEXPBar;
    public Image backEXPBar;
    public float additionMultiplier = 300;
    public float PowerMultiplier = 2;
    public float DivisionMultiplier = 7;
    // Start is called before the first frame update
    void Start()
    {
        frontEXPBar.fillAmount = currentEXP / requiredEXP;
        backEXPBar.fillAmount = currentEXP / requiredEXP;
        requiredEXP = calculateRequiredEXP();
        GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        UpdateEXPUI();
       // if (Input.GetKeyDown(KeyCode.Equals))
           //GainEperienceFlatRate(20);
        if (currentEXP > requiredEXP)
            LevelUP();
    }

    public void UpdateEXPUI()
    {
        float EXPFraction = currentEXP / requiredEXP;
        float FEXP = frontEXPBar.fillAmount;
        if(FEXP < EXPFraction)
        {
            delayTimer += Time.deltaTime;
            backEXPBar.fillAmount = EXPFraction;
            if(delayTimer > 3)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                frontEXPBar.fillAmount = Mathf.Lerp(FEXP, backEXPBar.fillAmount, percentComplete);
            }
        } 
    }
    public void GainEperienceFlatRate(float EXPGained)
    {
        currentEXP += EXPGained;
        lerpTimer = 0f;
        delayTimer = 0f;
        //print("Works");
    }

    public void GainEXPScalable(float EXPGained, int passedLevel)
    {
        if (passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.1f;
            currentEXP += EXPGained * multiplier;
        }
        else
        {
            currentEXP += EXPGained;
        }
        lerpTimer = 0f;
        delayTimer = 0f;
    }
    public void LevelUP()
    {
        
        level++;
        Debug.Log("LevelUp!!!");
        frontEXPBar.fillAmount = 0f;
        backEXPBar.fillAmount = 0f;
        currentEXP = Mathf.RoundToInt(currentEXP - requiredEXP);
        requiredEXP = calculateRequiredEXP();
        if (level == 5)
        {
            Debug.Log("Level 5");
        }

        if (level == 10)
        {
            Debug.Log("level 10");
        }

        if (level == 15)
        {
            Debug.Log("Level 15");
        }
    }


    private int calculateRequiredEXP()
    {
        int SolveForRequiredEXP = 0;

        for (int LevelCycle = 1; LevelCycle <= level; LevelCycle++)
        {
            SolveForRequiredEXP += (int)Mathf.Floor(LevelCycle + additionMultiplier * Mathf.Pow(PowerMultiplier, LevelCycle / DivisionMultiplier));
        }
        return SolveForRequiredEXP / 4;
    }
}
