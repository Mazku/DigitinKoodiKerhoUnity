using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour
{
    public float lifetime;

    void Start()
    {
        Invoke("DestroyIt", lifetime);
    }

    void DestroyIt()
    {
        Destroy(gameObject);
    }
}
