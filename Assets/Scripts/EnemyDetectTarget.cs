using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectTarget : MonoBehaviour
{
    public EnemyController enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy.target = collision.transform;
        enemy.ChangeState("Chasing");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy.ChangeState("Backing");
    }
}
