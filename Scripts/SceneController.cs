using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    //private SolitaireGame game;
    
    public Sprite cardBackSC;
    public Sprite blue;
    public Sprite red;

    public GameObject cardBackSCGO;
    public GameObject blueGO;
    public GameObject redGO;

    void Start()
    {
        //game = FindObjectOfType<SolitaireGame>();
        //int decision;
        
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Input" + Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                Debug.Log(hit.transform.name);
                if (hit.collider.CompareTag("BluePlayer"))
                    //userChoiceObject = controlBlue;
                    //userChoiceSprite = controlBlueSprite;
                    //decision = 1;
                    //BlueOption();
                    StartGame();
                    
                    

                if (hit.collider.CompareTag("RedPlayer"))
                    //userChoiceObject = controlRed;
                    //userChoiceSprite = controlRedSprite;
                    //decision = 2;
                    //RedOption();
                    StartGame();
                    
            }
        }
    }

    public void TitleGame()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void ChooseCard()
    {
        SceneManager.LoadScene("ChooseCardScene");
    }

    public void StartGame()
    {

        SceneManager.LoadScene("SolitareScene");
    }

    public void EndGame()
    {

        SceneManager.LoadScene("EndScene");
    }
    
    public void ToRulesPage()
    {
        //PauseGame();
        SceneManager.LoadScene("RulesScene");

    }

    public void ToPlayScreen()
    {
        StartGame();
        //ResumeGame();
        
    }

    public void RetryButton()
    {
        StartGame();
    }


    public void BlueOption()
    {
        cardBackSC = blue;
        cardBackSCGO = blueGO;
    }

    public void RedOption()
    {
        cardBackSC = red;
        cardBackSCGO = redGO;
    }

    

   

/*
    public void GreyOption()
    {
       
        Collision();
        Debug.Log("Decision" + decision);
        StartGame();
    }

    public void RulesCall()
    {
        SceneManager.loadScene("RulesScene");
    }

    public void GameOver()
    {
        SceneManager.loadScene("Ending Page");
    }*/
}
