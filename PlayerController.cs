using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 250f;
    public int maxJumps = 2;
    public Rigidbody2D myRigidbody;
    public SpriteRenderer myRenderer;
    public Animator myAnimator;
    public AudioSource jumpAudio;
    public AudioSource stepAudio;
    public AudioSource gameOverAudio;


    private float horizontal;
    private int timesJumped = 0;
    private bool running = false;
    private bool jumping = false;
    private bool gameOver = false;


    // Update is called once per frame
    void Update()
    {
        //If the game is over
        if (gameOver == true)
        {
            //Stop running the Update function. This means the player cannot move
            return;
        }

        Move();

        HandleFacingDirection();

        HandleAnimation();
    }

    //Movement input values
    public void MovementInput(InputAction.CallbackContext context)
    {
        //Gets the (x, y) values from A & D keys
        Vector2 moveInput = context.ReadValue<Vector2>();
        //Gets x value and applies it to our horizontal variable
        horizontal = moveInput.x;
    }

    //Jump input values
    public void JumpInput(InputAction.CallbackContext context)
    {
        //If jumpn button has been pressed
        if (context.started)
        {
            //Run the Jump function
            Jump();
        }
    }

    //Detect move input
    private void Move()
    {
        //If there is no input from player
        if (horizontal == 0)
        {
            //Then we are not running
            running = false;
        }
        //Otherwise there is horizontal input from player
        else
        {
            //So we are running
            running = true;
        }

        //Calcutes the direction the player should move
        Vector3 moveDirection = new Vector3(horizontal, 0, 0);
        //adds the direction onto the player's transform and multiples speed by time 
        transform.position += moveDirection * Time.deltaTime * moveSpeed;
    }

    //Private Jump values
    private void Jump()
    {
        //If Jumps greater than max, stop jumping
        if (timesJumped >= maxJumps)
        {
            //End function
            return;
        }

        //Add upward force to player's rigidbody
        myRigidbody.AddForce(transform.up * jumpForce);

        //Increase Jump counter
        timesJumped++;

        //We are now jumping
        jumping = true;

        //Play the jump SFX
        jumpAudio.Play();
    }

    //Function runs on contact with trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Reset Jump counter on collision with Ground
        if (collision.tag == "Ground")
        {
            //Reset the number of times jumped to zero
            timesJumped = 0;

            //We are not jumping
            jumping = false;
        }
    }

    //Function runs while inside trigger area
    private void OnTriggerStay2D(Collider2D collision)
    {
        //If objects within collision area is tagged as Ground
        if (collision.tag == "Ground")
        {
            //Reset the number of times jumped to zero
            timesJumped = 0;

            //We are no longer jumping
            jumping = false;
        }

        //If the object which entered our trigger area is tagged as Hazard
        if (collision.tag == "Enemy")
        {
            //The game is over
            Debug.Log("Game Over!");

            //Fires player up twice as high as the jump force
            myRigidbody.AddForce(transform.up * jumpForce * 2f);

            //Turn off the capsule and circle colliders
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;

            //Unparent the camera from the player
            Camera.main.transform.parent = null;

            //Set game over function to true
            gameOver = true;

            //Play the game over SFX
            gameOverAudio.Play();

            //Accesses Coroutine function and calls the restart function to restart game
            StartCoroutine(RestartGame());
        }
    }

    //Function runs when leaving trigger area
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If we have left the ground, but the times jumped is still zero
        if (collision.tag == "Ground" && timesJumped == 0)
        {
            //Increase the number of times jumped by 1
            timesJumped++;

            //We are jumping
            jumping = true;
        }
    }

    //Changes direction of player based on x axis
    private void HandleFacingDirection()
    {
        if (horizontal < 0f)
        {
            //Player is moving left - do flip sprite on x axis
            myRenderer.flipX = true;
        }
        else if (horizontal > 0f)
        {
            //Player is moving right - do not flip sprite on x axis
            myRenderer.flipX = false;
        }
    }

    //Detect Animations
    private void HandleAnimation()
    {
        //If we are jumping
        if (jumping == true)
        {
            //Play the jumping animation
            myAnimator.Play("Player_Jump");
        }
        //If we are not jumping, but we are running
        else if (running == true)
        {
            //Play the running animation
            myAnimator.Play("Player_Run");
        }
        //If we are not jumping or running, we are idle
        else
        {
            //Play the idle animation
            myAnimator.Play("Player_Idle");
        }
    }

    //Step audio
    public void PlayStepAudio()
    {
        //Randomise the pitch of the footstep SFX
        stepAudio.pitch = Random.Range(0.8f, 1.0f);

        //Play the footstep SFX
        stepAudio.Play();
    }

    //Function to restart the game
    private IEnumerator RestartGame()
    {
        //Wait for four seconds
        yield return new WaitForSeconds(4f);

        //Restart the scene
        SceneManager.LoadScene("MainGame");
    }
}
