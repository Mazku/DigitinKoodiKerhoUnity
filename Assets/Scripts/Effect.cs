using UnityEngine;
using System.Collections;

public class Effect : BaseBehaviour
{
    public float lifetime;

    void Start()
    {
        DelayThenDo(lifetime, DestroyIt);
    }

    void DestroyIt()
    {
        Destroy(gameObject);
    }
}
