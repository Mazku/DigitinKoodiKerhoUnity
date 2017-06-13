using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerLogic : BaseBehaviour
{
    const int maxHitpoints = 100;
    const int startingLives = 3;

    const float forceOnDamage = 1000f;
    const float forceOnEnemyKill = 1000f;
    const float disableInputOnDamageFor = 0.3f;

    PlayerController playerController;
    new Rigidbody2D rigidbody;
    Animator animator;

    Vector3 spawnPoint;

    public int hitPoints = maxHitpoints;
    public int lives = startingLives;

    bool alive = true;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        spawnPoint = transform.position;
    }

    void Update()
    {
        animator.SetBool("isAlive", alive);
        if (alive)
        {
            if (transform.position.y < LevelConfiguration.GetInstance().deathVerticalLevel)
            {
                hitPoints = 0;
                HandleDead();
            }
        }
    }

    public void OnTakeDamage(Vector2 direction, int amount)
    {
        rigidbody.AddForce(-direction.normalized * forceOnDamage + Vector2.up * forceOnDamage);
        playerController.DisableInputFor(disableInputOnDamageFor);

        hitPoints -= amount;
        if (hitPoints <= 0)
        {
            HandleDead();
        }
    }

    public void OnKilledMonster()
    {
        rigidbody.AddForce(Vector2.up * forceOnEnemyKill); 
    }

    void HandleDead()
    {
        if (alive)
        {
            lives--;
            alive = false;
            playerController.DisableInputFor(0.5f);

            DelayThenDo(0.5f, OnDead);
        }
    }

    void OnDead()
    {
        if (lives > 0)
        {
            // Respawn
            alive = true;
            hitPoints = maxHitpoints;
            rigidbody.velocity = Vector2.zero;
            transform.position = spawnPoint;
        }
        else
        {
            LevelConfiguration.GetInstance().OnGameOver();
        }
    }
}


