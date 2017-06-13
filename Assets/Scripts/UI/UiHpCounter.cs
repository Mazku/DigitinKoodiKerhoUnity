using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiHpCounter : BaseBehaviour
{
    public Text hpText;

    public void SetHp(int hp)
    {
        hpText.text = "HP: " + hp;
    }
}

