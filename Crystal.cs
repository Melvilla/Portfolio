using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Crystal : MonoBehaviour
{
    public bool chestOpen = false;
    public string winScene;
    public Rigidbody2D myRigidbody;
    public GameObject interactionText;

    private bool CanInteract = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Stop object from playing
        GetComponentInChildren<ParticleSystem>().Stop();
    }

    //Updates function per frame
    void Update()
    {
        //Check if the E key is being pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            //And if the player can interact
            if (CanInteract == true)
            {
                //Run key collected function, set key collected to true
                if (chestOpen == true)
                {
                    //Turn off interaction text
                    interactionText.SetActive(false);

                    //Switch off object
                    GetComponent<SpriteRenderer>().enabled = false;

                    //Switch off colliders
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<CircleCollider2D>().enabled = false;

                    //Stop object from playing
                    GetComponentInChildren<ParticleSystem>().Stop();

                    //Run Crystal Collected function
                    StartCoroutine(WinGame());
                }
            }
        }
    }

    //This built-in function detects when something has overlapped with the trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If object collision has Player tag
        if (collision.tag == "Player")
        {
            //Can interact with object
            CanInteract = true;

            //Display interaction text
            interactionText.SetActive(true);
        }
    }

    //This built-in function detects when something is no longer overlapped with the trigger collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If object collision has Player tag
        if (collision.tag == "Player")
        {
            //Can interact with object
            CanInteract = false;

            //Can no longer display interaction text
            interactionText.SetActive(false);
        }
    }

    //Function to check if the chest is open
    public void ChestCheck()
    {
        //Switch on renderer
        GetComponent<SpriteRenderer>().enabled = true;

        //Switch on colliders
        GetComponent<BoxCollider2D>().enabled = true;        
        GetComponent<CircleCollider2D>().enabled = true;

        //Switch on Particle System
        GetComponentInChildren<ParticleSystem>().Play();

        //Throw crystal out of chest
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;

        //Set chest open to true
        chestOpen = true;
    }

    //This is a coroutine that runs when the crystal has been collected
    private IEnumerator WinGame()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        //Load the win scene
        SceneManager.LoadScene(winScene);
    }

}
