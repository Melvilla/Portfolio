using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BossEnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed;

    private Rigidbody2D enemyRb;
    private Animator anim;
    private Transform currentPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //Start point
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        //Assign direction enemy wants to go towards
        Vector2 point = currentPoint.position - transform.position;
        //If enemy has a current assigned as point B
        if (currentPoint == pointB.transform)
        {
            //Then head in that direction
            enemyRb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            //Otherwise head in the opposite direction
            enemyRb.linearVelocity = new Vector2(-speed, 0);
        }

        //If distance to point B is less than 0.5f
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            //Runs the pause patrol coroutine
            StartCoroutine(PausePatrol());

            //Flip enemy direction
            flip();

            //Set current point to point A
            currentPoint = pointA.transform;
        }

        //If distance to point A is less than 0.5f
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            //Runs the pause patrol coroutine
            StartCoroutine(PausePatrol());

            //Flip enemy direction
            flip();

            //Set current point to point B
            currentPoint = pointB.transform;
        }
    }

    //Function to pause enemy movement and set idle animation
    private void wait()
    {
        //Freezes the enemy
        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;

        //Sets idle to true and plays the idle animation
        anim.SetBool("isRunning", false);
    }

    //Function to allow enemy to continue patrolling
    private void go()
    {
        //Unfreezes the enemy
        enemyRb.constraints = RigidbodyConstraints2D.None;

        //Sets running to true and plays the running animation
        anim.SetBool("isRunning", true);
    }

    //Function to flip the enemy direction
    private void flip()
    {
        //Assigns localScale a value using transforms localScale
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    //Gizmos to draw outlines for points of patrols
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    private IEnumerator PausePatrol()
    {
        //Run wait function
        wait();

        //Wait for four seconds
        yield return new WaitForSeconds(2f);

        //Run go function
        go();
    }


}
