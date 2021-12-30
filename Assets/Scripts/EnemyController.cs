using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Vector2 directionToTarget;
    Animator animator;
    private Transform startPosition;
    private string currentState;
    public float distanceToAttack = 0.5f;
    private float currentTimeToAttack;
    public float TimeToAttack;

    private int currentLife = 10;

    private void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = new GameObject().transform;
        startPosition.name = "Skeleton"+ gameObject.GetInstanceID()+" Start position ";
        startPosition.tag = "startPosition";
        startPosition.position = transform.position;
        //currentTimeToAttack = TimeToAttack;
    }
    // Update is called once per frame
    void Update()
    {
        if(target != null)
            directionToTarget = target.position - transform.position;
        float distanceToTarget;
        switch (currentState)
        {
            case "Stopped":
                resetAnimation();

                break;
            case "Chasing":
                distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (distanceToTarget > distanceToAttack)
                {
                    setAnimation(true);
                    followTarget();
                }
                else
                {
                    //near player
                   // currentTimeToAttack = TimeToAttack;
                    currentState = "Attacking";
                }

                break;
            case "Backing":
                distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (distanceToTarget > 0.1f)//distance to spawn
                {
                    target = startPosition;
                    setAnimation(true);
                    followTarget();
                }
                else
                {
                    currentState = "Stopped";
                }
                    
                break;
            case "Attacking":

                setAnimation(false);
                animator.SetBool("attacking", true);

                currentTimeToAttack += Time.deltaTime;
                if(currentTimeToAttack >= TimeToAttack)
                {
                    distanceToTarget = Vector2.Distance(transform.position, target.position);

                    if (distanceToTarget > distanceToAttack)
                    {
                        animator.SetBool("attacking", false);
                        currentState = "Chasing";
                    }
                    else
                    {
                        //attack the player
                        target.GetComponent<PlayerController>().TakeDamage(1);
                    }
                    currentTimeToAttack = 0;
                }

                
                    break;
            default:
                break;
        }

        if (directionToTarget.y >0)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        else
            GetComponent<SpriteRenderer>().sortingOrder = 1;

    }

    void setAnimation(bool walking)
    {
        animator.SetBool("walking", walking);
        animator.SetFloat("walk_x", directionToTarget.x);
        animator.SetFloat("walk_y", directionToTarget.y);
    }

    void resetAnimation()
    {
        animator.SetBool("walking", false);
        animator.SetFloat("walk_x", 0);
        animator.SetFloat("walk_y", 0);
    }

    void followTarget()
    {
            transform.position = Vector2.MoveTowards(
                transform.position, target.position,
                speed * Time.deltaTime);

    }

    public void ChangeState(string stateName)
    {
        currentState = stateName;
    }

    public void ReceiveDamage(int damage)
    {
        StartCoroutine(RedDamageFx());
        currentLife = currentLife - damage;
        if(currentLife <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator RedDamageFx()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        for (float oc = 1f; oc >= 0; oc -= 0.1f)
        {
            color.g = oc;
            color.b = oc;
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(.025f);
        }
        for (float oc = 0f; oc <= 1; oc += 0.1f)
        {
            color.g = oc;
            color.b = oc;
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(.025f);
        }

        GetComponent<SpriteRenderer>().color = Color.white;
    }

}
