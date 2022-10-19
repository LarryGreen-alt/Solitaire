using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private SolitaireGame game;
    private CardFlip flipCard;

    public GameObject preBlue;
    public GameObject preRed;
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<SolitaireGame>();
        flipCard = FindObjectOfType<CardFlip>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Input" + Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                Debug.Log(hit.transform.name);
                if (hit.collider.CompareTag("Card"))
                    ClickCard(hit.collider.gameObject);
                if (hit.collider.CompareTag("Tabs"))
                    ClickTab(hit.collider.gameObject);
                if (hit.collider.CompareTag("Foundation"))
                    ClickFoundation(hit.collider.gameObject);
                if (hit.collider.CompareTag("Deck"))
                    ClickDeck();
                if (hit.collider.CompareTag("BluePlayer"))
                   // ClickBlue();//(preBlue);
                if (hit.collider.CompareTag("RedPlayer")) {}
                   // ClickRed();//(preRed);
            }
        }
    }
    void ClickCard(GameObject c)
    {
        Debug.Log("Card clicked: " + c.name) ;
        
        game.SelectCard(c);
        game.FlipHorizontal();
    }
    void ClickTab(GameObject t)
    {
        Debug.Log("Empty move: " + t.name) ;
        game.MoveToEmptyTab(t);
    }
    
    void ClickFoundation(GameObject f)
    {
        Debug.Log("Foundation clicked");
        game.MoveToFoundation(f);
    }
    void ClickDeck()
    {
        Debug.Log("Deck clicked");
        game.FlipDeck();
    }
/*
    void ClickUserCard()
    {
        Debug.Log("blue clicked");
        game.UserChoiceBlue();
    }

    void ClickRed()
    {
        Debug.Log("red clicked");
        game.UserChoiceRed();
    }
*/
}