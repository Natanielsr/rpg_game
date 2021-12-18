using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator legsAnimator;
   // float input_x = 0;
   // float input_y = 0;
    public float speed = 2.5f;
    bool isRunning = false;
    bool canWalk = true;
    Rigidbody2D rb2D;

    public float Life = 10;

    Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (canWalk)
        {
            isRunning = (input.x != 0 || input.y != 0);

            if (isRunning)
            {
               // var move = new Vector3(input.x, input.y, 0).normalized;
                //transform.position += move * speed * Time.deltaTime;
                playerAnimator.SetFloat("input_x", input.x);
                playerAnimator.SetFloat("input_y", input.y);
                legsAnimator.SetFloat("input_x", input.x);
                legsAnimator.SetFloat("input_y", input.y);
            }
            legsAnimator.SetBool("isRunning", isRunning);
            playerAnimator.SetBool("isRunning", isRunning);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            playerAnimator.SetBool("isRunning", false);
            legsAnimator.SetBool("isRunning", false);
            canWalk = false;
            playerAnimator.SetTrigger("attack");

        }

    }

    void FixedUpdate()
    {
        if(canWalk)
            rb2D.MovePosition(rb2D.position + new Vector2(input.x, input.y) * speed * Time.fixedDeltaTime);
    }

    public void CanWalkAgain()
    {
        canWalk = true;
    }

    public void TakeDamage(float damage)
    {
        Life -= damage;
        StartCoroutine(RedDamageFx());
        //time
        if (Life <= 0)
        {
            //die
            Die();
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

    public void Die()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        
    }
}
