using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour
{
    public bool keyCollected = false;
    public bool open = false;
    public Animator myAnimator;
    public AudioSource chestOpenAudio;
    public GameObject interactionText;

    private bool CanInteract = false;

    // Update is called once per frame
    void Update()
    {
        //Check if the E key is being pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            //And if the player can interact
            if (CanInteract == true)
            {
                //And if the key has been collected
                if (keyCollected == true)
                {
                    //Turn off interaction text
                    interactionText.SetActive(false);

                    //Run chest open function, set open to true
                    ChestOpen();

                    //Run wait coroutine
                    StartCoroutine(Wait());
                    
                    //Play Key Audio
                    chestOpenAudio.Play();
                }
                
            }

        }
        
        HandleAnimation();

    }

    //Check if the collider has been entered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the object colliding has the Player tag
        if (collision.tag == "Player")
        {
            //Can interact with object
            CanInteract = true;

            //Display interaction text
            interactionText.SetActive(true);
        }
    }

    //Check if the collider has been exited
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the object is no longer being collided with by the Player
        if (collision.tag == "Player")
        {
            //Can no longer interact
            CanInteract = false;

            //Can no longer display interaction text
            interactionText.SetActive(false);
        }
    }

    //Detect Animations
    private void HandleAnimation()
    {
        //If we are opening the chest
        if (open == true)
        {
            //Play the chest open animation
            myAnimator.Play("Chest_Open");
        }
    }

    //Function to increase number of keys collected
    public void KeyCollected()
    {
        //Increase number of keys collected
        keyCollected = true;
    }

    //Function to open the chest
    public void ChestOpen()
    {
        //Tells Crystal script the chest is open
        FindAnyObjectByType<Crystal>().ChestCheck();
        //Set open to true
        open = true;
    }

    //Coroutine to delay chest being disabled until after the animation plays
    private IEnumerator Wait()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        //Switch off object
        gameObject.SetActive(false);                
    }
}
