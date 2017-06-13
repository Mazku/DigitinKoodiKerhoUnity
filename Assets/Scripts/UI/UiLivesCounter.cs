using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class UiLivesCounter : BaseBehaviour
{
    public RectTransform counterRoot;

    List<GameObject> hearts;

    void Start()
    {
        hearts = counterRoot.GetComponentsInChildren<Image>().Select(x => x.gameObject).ToList();
    }

    public void SetLives(int lives)
    {
        hearts.ForEach(x => x.SetActive(false));
        hearts.Take(lives).ToList().ForEach(x => x.SetActive(true));
    }
}

