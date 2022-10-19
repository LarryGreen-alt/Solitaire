using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpriteController : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    public bool faceUp;
    public string suit;
    public int value;
    private SolitaireGame game;
    

    public Sprite redCardBack;
    public Sprite blueCardBack;
   
    
    // Start is called before the first frame update
    void Start()
    {
        //bool decide = GameObject.Find("SceneManagerHome").GetComponent<SceneController>().decision;
        
        
        
        List<string> deck = SolitaireGame.GenerateDeck();
        game = FindObjectOfType<SolitaireGame>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        int i = 0;
        foreach (string card in deck)
        {
            if (name == card)
            {
                cardFace = game.cardSprites[i];
                suit = name.Substring(0, 1);
                int.TryParse(name.Substring(1), out value);
                break;
            }
            ++i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (faceUp)
            spriteRenderer.sprite = cardFace;
        else
            spriteRenderer.sprite = cardBack;
    }
}
