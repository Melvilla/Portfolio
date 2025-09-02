using UnityEngine;
using System.Collections;

public class CastleSign : MonoBehaviour
{
    public GameObject chestSignText;
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
                //Find and turn off the player controller
                FindAnyObjectByType<PlayerController>().enabled = false;

                //Find and turn off the step audio
                FindAnyObjectByType<PlayerController>().stepAudio.enabled = false;

                //Turn off interaction text
                interactionText.SetActive(false);

                //Run text open function, display text box
                TextOpen();                
            }            
        }

        //Check if the Q key is being pressed
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            //And if the player can interact
            if (CanInteract == true)
            {
                //Find and turn on the player controller
                FindAnyObjectByType<PlayerController>().enabled = true;

                //Find and turn on the step audio
                FindAnyObjectByType<PlayerController>().stepAudio.enabled = true;

                //Turn off interaction text
                interactionText.SetActive(true);

                //Close the text down
                TextClose();
            }
        }
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

    //Function to display the text box
    private void TextOpen()
    {
        //Open Text Box
        chestSignText.SetActive(true);
    }

    //Function to stop displaying the text box
    private void TextClose()
    {
        //Close Text Box
        chestSignText.SetActive(false);
    }
}
