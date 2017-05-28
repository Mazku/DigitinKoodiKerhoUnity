using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerLogic : BaseBehaviour
{
    const int maxHitpoints = 100;
    const int startingLives = 3;

    PlayerController playerController;
    new Rigidbody2D rigidbody;
    Animator animator;

    Vector3 spawnPoint;

    int hitPoints = maxHitpoints;
    int lives = startingLives;

    bool dead;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        spawnPoint = transform.position;
    }

    void Update()
    {
        animator.SetBool("isAlive", !dead);
        if (!dead)
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
        hitPoints -= amount;
        if (hitPoints <= 0)
        {
            HandleDead();
        }
    }

    void HandleDead()
    {
        if (!dead)
        {
            lives--;
            dead = true;
            playerController.enabled = false;

            DelayThenDo(0.5f, OnDead);
        }
    }

    void OnDead()
    {
        playerController.enabled = true;
        if (lives > 0)
        {
            // Respawn
            dead = false;
            rigidbody.velocity = Vector2.zero;
            transform.position = spawnPoint;
        }
        else
        {
            LevelConfiguration.GetInstance().OnGameOver();
        }
    }
}


