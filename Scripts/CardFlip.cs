using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    public float x, y, z;

    public GameObject cardBack;
    private SolitaireGame game;

    public bool cardBackActive;

    public int timer;

    // Start is called before the first frame update
    void Start()
    {
        //cardBackActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //StartFlip();
            game.FlipHorizontal();
        }
    }

/*
    public void StartFlip()
    {
        
            StartCoroutine(Calculateflip());
        
    }

    public void Flip()
    {
        if(cardBackActive == true)
        {
            cardBack.SetActive(false);
            cardBackActive = false;
        }
        else
        {
            cardBack.SetActive(true);
            cardBackActive = true;
        }
    }

    IEnumerator Calculateflip()
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Rotate(new Vector3(x, y, z));
            timer++;

            if(timer == 90 || timer == -90)
            {
                Flip();
            }
        }

        timer=0;
    }
*/

}
