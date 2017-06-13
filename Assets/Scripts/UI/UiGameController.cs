using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGameController : BaseBehaviour
{
    public UiLivesCounter livesCounter;
    public UiHpCounter hpCounter;

    public PlayerLogic playerLogic;

    void Update()
    {
        livesCounter.SetLives(playerLogic.lives);
        hpCounter.SetHp(playerLogic.hitPoints);
    }
}
