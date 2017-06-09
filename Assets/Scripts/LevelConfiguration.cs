using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfiguration : BaseBehaviour
{
    public float deathVerticalLevel;

    static LevelConfiguration instance;

    public static LevelConfiguration GetInstance()
    {
        if (!instance)
        {
            Debug.LogError("Instance of a singleton " + typeof(LevelConfiguration) + " not found."); 
        }
        return instance;
    }

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Instance of a singleton " + GetType() + " already exists.");
        }
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    void OnDrawGizmos()
    {
        var gizmoHeight = 50;

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(0, -gizmoHeight / 2 + deathVerticalLevel), new Vector3(30000, gizmoHeight));
    }

    public void OnGameOver()
    {
        Time.timeScale = 0.0f;
    }
}
