using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseBehaviour
{
    const float deadDelay = 2.0f;
    const float patrolSpeed = 1.0f;

    public Collider2D attackCollider;
    public Collider2D topCollider;

    public RectTransform patrolArea;

    public int damageAmountOnHit = 20;

    bool alive = true;

    new Rigidbody2D rigidbody;
    Animator animator;

    Vector3 patrolAreaStartPos;

    void Start()
    {
        patrolAreaStartPos = patrolArea.position;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("alive", alive); 
        patrolArea.position = patrolAreaStartPos;
        if (alive)
        {
            // TODO: follow the set move area
            var pos = transform.position;

            var patrolAreaLength = patrolArea.rect.width * patrolArea.lossyScale.x;
            var timeForPatrol = patrolAreaLength / patrolSpeed;
            pos.x = patrolArea.position.x + Mathf.Sin(Time.time * (2 * Mathf.PI) / timeForPatrol) * (patrolAreaLength / 2);
            transform.position = pos;
        }
    }

    // Called on collission
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!alive || collision2D.rigidbody == null)
        {
            return; 
        }

        var maybePlayerLogic = collision2D.rigidbody.GetComponent<PlayerLogic>();

        if (maybePlayerLogic != null)
        {
            if (collision2D.otherCollider == attackCollider)
            {
                // Hit player with attack collider
                maybePlayerLogic.OnTakeDamage(collision2D.relativeVelocity, damageAmountOnHit);
            }
            if (collision2D.otherCollider == topCollider)
            {
                // Hit player with top collider, kill monster
                maybePlayerLogic.OnKilledMonster();
                Die(collision2D.collider);
            }
        } 
    }

    void Die(Collider2D playerCollider)
    {
        alive = false;

        rigidbody.AddForce(Vector2.up * 500 + Vector2.left * 500);
        DelayThenDo(deadDelay, () => GameObject.Destroy(gameObject));

        Physics2D.IgnoreCollision(playerCollider, attackCollider);
        Physics2D.IgnoreCollision(playerCollider, topCollider);
    }
}
