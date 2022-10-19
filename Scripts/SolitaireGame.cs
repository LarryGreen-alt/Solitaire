using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SolitaireGame : MonoBehaviour
{
    public static List<string> suits = new List<string>{ "C", "D", "H", "S" };
    public static List<string> values = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" };
    public List<string> deck;
    public GameObject cardPrefab;

    public GameObject bluePrefab;
    public GameObject redPrefab;

    
    private CardFlip flipCard;

    private int test;


    private System.Random r = new System.Random();
    public List<string>[] tabs;
    public List<string>[] foundations;
    private List<string> tab0 = new List<string>();
    private List<string> tab1 = new List<string>();
    private List<string> tab2 = new List<string>();
    private List<string> tab3 = new List<string>();
    private List<string> tab4 = new List<string>();
    private List<string> tab5 = new List<string>();
    private List<string> tab6 = new List<string>();
    private List<string> foundation0 = new List<string>();
    private List<string> foundation1 = new List<string>();
    private List<string> foundation2 = new List<string>();
    private List<string> foundation3 = new List<string>();
    private List<string> waste = new List<string>();
    public GameObject[] tabPos;
    public GameObject[] foundationPos;
    public GameObject wastePos;
    public Sprite[] cardSprites;
    public GameObject selectedCard;
    public int selectedTab = -1;
    public int selectedFoundation = -1;
    public float wasteZOffset = 0f;
    public Sprite borderSprite;
    public Sprite cardBack;

    public Sprite blueBack;
    public Sprite redBack;

    private SceneController controller;
    
    // Start is called before the first frame update
    void Start()
    {
        tabs = new List<string>[] { tab0, tab1, tab2, tab3, tab4, tab5, tab6 };
        foundations = new List<string>[] { foundation0, foundation1, foundation2, foundation3 };
        flipCard = FindObjectOfType<CardFlip>();
        controller = FindObjectOfType<SceneController>();
        //int decide = GameObject.Find("SceneManagerHome").GetComponent<SceneController>().decision;
        Sprite cB = GameObject.Find("SceneManagerHome").GetComponent<SceneController>().cardBackSC;
        GameObject cBGO = GameObject.Find("SceneManagerHome").GetComponent<SceneController>().cardBackSCGO;
        
        

        Debug.Log("Decide: " + test);
        
        //Debug.Log("Here: " + decide);



        //cardBack = cB;
        cardPrefab = bluePrefab;

        
        
        

        deck = GenerateDeck();
        Shuffle();
        ArrangeTabs();
        StartCoroutine(DealCards());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static List<string> GenerateDeck()
    {
        List<string> ret = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                ret.Add(s + v);
            }
        }

        return ret;
    }

    void ArrangeTabs()
    {
        for (int i = 0; i!= 7; ++i)
        {
            for (int j = i; j != 7; ++j)
            {
                tabs[j].Add(deck[deck.Count - 1]);
                deck.RemoveAt(deck.Count - 1);
                
            }
        }
    }

    IEnumerator DealCards()
    {
        for (int i = 0; i != 7; ++i)
        {
            float yOffset = 0f;
            float zOffset = 0f;
            
            foreach (string c in tabs[i])
            {
                yield return new WaitForSeconds(.05f);
                GameObject card = Instantiate(cardPrefab, tabPos[i].transform.position -new Vector3(0, yOffset, zOffset), Quaternion.identity, tabPos[i].transform);
                yOffset += .3f;
                zOffset += .03f;
                card.name = c;
                card.GetComponent<CardSpriteController>().faceUp = false;
                if (c == tabs[i][tabs[i].Count-1])
                    card.GetComponent<CardSpriteController>().faceUp = true;
            }
        }
    }

    void Shuffle()
    {
        int i = deck.Count;
        while(i > 1)
        {
            int k = r.Next(i--);
            string t = deck[k];
            deck[k] = deck[i];
            deck[i] = t;
        }
    }

    public void SelectCard(GameObject c)
    {
        Debug.Log("Selectcard clicked!" + c.name);
        if (c != selectedCard)
        {
            if (selectedCard == null)
            {
                if (c.transform.parent.CompareTag("Tabs"))
                {
                    int t = int.Parse(c.transform.parent.name.Split("Tab")[1]);
                    if (c.GetComponent<CardSpriteController>().faceUp)
                    {
                        c.GetComponent<SpriteRenderer>().color = Color.grey;
                        selectedCard = c;
                        selectedTab = t;
                        selectedFoundation = -1;
                    } else
                    {
                        if (c.name.Equals(tabs[t][tabs[t].Count-1]))
                        {
                            //FlipTheCard();
                            c.GetComponent<CardSpriteController>().faceUp = true;
                        }
                    }

                } else if (c.transform.parent.CompareTag("Foundation"))
                {
                    int t = int.Parse(c.transform.parent.name.Split("Foundation")[1]);
                    if (c.GetComponent<CardSpriteController>().faceUp)
                    {
                        c.GetComponent<SpriteRenderer>().color = Color.grey;
                        selectedCard = c;
                        selectedTab = t;
                        selectedFoundation = -1;
                    } else
                    {
                        if (c.name.Equals(tabs[t][tabs[t].Count-1]))
                        {
                            c.GetComponent<CardSpriteController>().faceUp = true;
                        }
                    }
                } else //waste
                {
                    if (c.name.Equals(waste[waste.Count-1]))
                    {
                        c.GetComponent<SpriteRenderer>().color = Color.grey;
                        selectedCard = c;
                        selectedTab = -1;
                        selectedFoundation = -1;
                    }
                }
            } else // When the card is not null
            {
                if (c.transform.parent.CompareTag("Tabs"))
                {
                    int t = int.Parse(c.transform.parent.name.Split("Tab")[1]);
                    if (c.name.Equals(tabs[t][tabs[t].Count-1]))
                    {
                        
                        if (ValidMove(selectedCard,c))
                        {
                            
                            if (selectedTab != -1)
                            {
                                //string cardToMove = tabs[selectedTab][tabs[selectedTab].Count - 1];
                                string cardToMove = selectedCard.name;
                                tabs[t].Add(cardToMove);
                                tabs[selectedTab].Remove(cardToMove);
                                Transform child = tabPos[selectedTab].transform.Find(cardToMove);
                                Debug.Log("child: " + child.name);
                                child.parent = tabPos[t].transform;
                                Debug.Log("child.parent: " + child.parent.name);
                                Debug.Log("selected card: " + selectedCard.name);
                                child.localPosition = c.transform.localPosition + new Vector3(0,-.3f,-.03f);
   
                            } else if (selectedFoundation != -1)
                            {
                                string cardToMove = selectedCard.name;
                                tabs[t].Add(cardToMove);
                                foundations[selectedFoundation].Remove(cardToMove);
                                Transform child = foundationPos[selectedFoundation].transform.Find(cardToMove);
                                Debug.Log("child: " + child.name);
                                child.parent = tabPos[t].transform;
                                Debug.Log("child.parent: " + child.parent.name);
                                Debug.Log("selected card: " + selectedCard.name);
                                child.localPosition = c.transform.localPosition + new Vector3(0, -.3f, -.03f);
                            } else // moving from waste to tab
                            {
                                string cardToMove = selectedCard.name;
                                tabs[t].Add(cardToMove);
                                waste.Remove(cardToMove);
                                Transform child = wastePos.transform.Find(cardToMove);
                                Debug.Log("child: " + child.name);
                                child.parent = tabPos[t].transform;
                                Debug.Log("child.parent: " + child.parent.name);
                                Debug.Log("selected card: " + selectedCard.name);
                                child.localPosition = c.transform.localPosition + new Vector3(0, -.3f, -.03f);
                            }
                        } else
                        {
                            Debug.Log("not valid from: " + selectedCard.name + " to " + c.name);
                        }
                        DeselectCard();
                    }
                }
                else if (c.transform.parent.CompareTag("Foundation"))
                {
                    int f = int.Parse(c.transform.parent.name.Split("Foundation")[1]);
                    string fromSuit = selectedCard.GetComponent<CardSpriteController>().suit;
                    string toSuit = c.GetComponent<CardSpriteController>().suit;
                    int fromVal = selectedCard.GetComponent<CardSpriteController>().value;
                    int toVal = c.GetComponent<CardSpriteController>().value;
                    if (fromSuit.Equals(toSuit) && fromVal == toVal+1)
                    {
                        if (selectedTab != -1)
                        {
                            string cardToMove = selectedCard.name;
                            foundations[f].Add(cardToMove);
                            tabs[selectedTab].Remove(cardToMove);
                            Transform child = tabPos[selectedTab].transform.Find(cardToMove);
                            Debug.Log("child: " + child.name);
                            child.parent = foundationPos[f].transform;
                            Debug.Log("child.parent: " + child.parent.name);
                            Debug.Log("selected card: " + selectedCard.name);
                            child.localPosition = c.transform.localPosition + new Vector3(0, -.3f, -.03f);
                        } else if (selectedFoundation != -1)
                        {
                            Debug.Log("Don't move between foundations please :D");
                            DeselectCard();
                        } else
                        {
                            string cardToMove = selectedCard.name;
                            foundations[f].Add(cardToMove);
                            waste.Remove(cardToMove);
                            Transform child = wastePos.transform.Find(cardToMove);
                            Debug.Log("child: " + child.name);
                            child.parent = foundationPos[f].transform;
                            Debug.Log("child.parent: " + child.parent.name);
                            Debug.Log("selected card: " + selectedCard.name);
                            child.localPosition = c.transform.localPosition + new Vector3(0, -.3f, -.03f);
                        }
                    }
                }
                else // waste
                {

                }
            }
        } else
        {
            DeselectCard();
        }
    }

    public void FlipDeck()
    {
        if (deck.Count == 0)
            ResetDeck();
        else
        {
            int flipCt = 0;
            if (deck.Count >= 3) flipCt = 3;
            else flipCt = deck.Count;
            foreach (Transform child in wastePos.transform)
            {
                //set position to wastePos
                child.position = wastePos.transform.position + new Vector3(0, 0, child.position.z);
            }
            float xOffset = 0f;
            for (int i = flipCt; i != 0; --i)
            {
                GameObject card = Instantiate(cardPrefab, wastePos.transform.position - new Vector3(xOffset,0,wasteZOffset), Quaternion.identity, wastePos.transform);
                card.name = deck[deck.Count - 1];
                card.GetComponent<CardSpriteController>().faceUp = true;
                xOffset -= .3f;
                wasteZOffset += .03f;
                waste.Add(deck[deck.Count - 1]);
                deck.RemoveAt(deck.Count - 1);
            }
            if (deck.Count == 0) {
                GameObject.Find("DeckButton").GetComponent<SpriteRenderer>().sprite = borderSprite;
            }
        }
    }

    /*public void FlipTheCard()
    {
         if (selectedCard.GetComponent<CardSpriteController>().faceUp != true)
         {
            flipCard.StartFlip();
         }
    }*/
    

    public void FlipHorizontal()
    {
        if (selectedCard.GetComponent<CardSpriteController>().faceUp == false)
        {
            selectedCard.transform.Rotate(0, 180, 0);
            Debug.Log("Works!");
        }
        else
        {
            Debug.Log("Failed");
        }
    }

    public void ResetDeck()
    {
        wasteZOffset = 0f;
        if (waste.Count != 0)
        {
            Debug.Log("Deck:" + GameObject.Find("DeckButton"));
            GameObject.Find("DeckButton").GetComponent<SpriteRenderer>().sprite = cardBack;
            while (waste.Count > 0)
            {
                deck.Add(waste[waste.Count - 1]);
                waste.RemoveAt(waste.Count - 1);
            }
            foreach(Transform child in wastePos.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void MoveToFoundation(GameObject f)
    {
        int fNum = int.Parse(f.name.Split("Foundation")[1]);
        Debug.Log("fNum: " + fNum);
        int f0 = foundations[0].Count;
        int f1 = foundations[1].Count;
        int f2 = foundations[2].Count;
        int f3 = foundations[3].Count;
        int final = f0 + f1 + f2 + f3;
        
        Debug.Log("Count of 0: " + f0);
        Debug.Log("Count of 1: " + f1);
        Debug.Log("Count of 2: " + f2);
        Debug.Log("Count of 3: " + f3);
        Debug.Log("Count of All: " + final);

        if (selectedCard != null)
        {
            if (selectedTab != -1)
            {
                if (selectedCard.name.Equals(tabs[selectedTab][tabs[selectedTab].Count-1]))
                {
                    if (foundations[fNum].Count == 0)
                    {
                        if (selectedCard.GetComponent<CardSpriteController>().value == 1)
                        {
                            selectedCard.transform.parent = f.transform;
                            selectedCard.transform.localPosition = new Vector3(0, 0, -.03f);
                            tabs[selectedTab].RemoveAt(tabs[selectedTab].Count - 1);
                            foundations[fNum].Add(selectedCard.name);
                            DeselectCard();
                        }
                        else
                        {
                            Debug.Log("Can't move non-ace to empty foundation");
                        }
                    }
                }
            } else if (selectedFoundation != -1)
            {
                Debug.Log("Don't move between foundations please :D");
                DeselectCard();
            } else // must be from waste
            {
                string cardToMove = selectedCard.name;
                foundations[fNum].Add(cardToMove);
                waste.Remove(cardToMove);
                Transform child = wastePos.transform.Find(cardToMove);
                Debug.Log("child: " + child.name);
                child.parent = foundationPos[fNum].transform;
                Debug.Log("child.parent: " + child.parent.name);
                Debug.Log("selected card: " + selectedCard.name);
                child.localPosition =  new Vector3(0, 0f, -.03f);
            }
        }
    }

    public void FinishGame(GameObject f)
    {
        // Does the foundation itself. How I numbered it. 
        //int fNum = int.Parse(f.name.Split("Foundation")[1]);
        int f0 = foundations[0].Count;
        int f1 = foundations[1].Count;
        int f2 = foundations[2].Count;
        int f3 = foundations[3].Count;
        
        Debug.Log("Count of 0: " + f0);
        Debug.Log("Count of 1: " + f1);
        Debug.Log("Count of 2: " + f2);
        Debug.Log("Count of 3: " + f3);
        int final = f0 + f1 + f2 + f3;

        if (final == 52) {
             SceneManager.LoadScene("EndScene");
        }

         
    }

    public void MoveToEmptyTab(GameObject t)
    {
        int fNum = int.Parse(t.name.Split("Tab")[1]);
        // The bottom of tab.
        Debug.Log("fNum: " + fNum);
        int tVal = selectedCard.GetComponent<CardSpriteController>().value;
        Debug.Log(tVal);
        
        if (selectedCard != null)
        {
            if (selectedTab != -1)
            {
                if (selectedCard.name.Equals(tabs[selectedTab][tabs[selectedTab].Count-1]))
                {
                    Debug.Log("selected: " + selectedCard); 
                    if (tabs[fNum].Count == 0)
                    {
            
                        if (tVal == 13) 
                        {
                        
                            Debug.Log("Broken?");
                            selectedCard.transform.parent = t.transform;
                            selectedCard.transform.localPosition = new Vector3(0, 0, -.03f);
                            tabs[selectedTab].RemoveAt(tabs[selectedTab].Count - 1);
                            tabs[fNum].Add(selectedCard.name);
                            DeselectCard();
                        }
                    } else
                        {
                            Debug.Log("Can't move non-ace to empty tab");
                        }
                    }
                }
            } else if (selectedTab != -1)
            {
                Debug.Log("Don't move between tabs please :D");
                DeselectCard();
            } else // must be from waste
            {
                if (tVal == 13) 
                        {
                        
                            Debug.Log("Broken?");
                            selectedCard.transform.parent = t.transform;
                            selectedCard.transform.localPosition = new Vector3(0, 0, -.03f);
                            tabs[selectedTab].RemoveAt(tabs[selectedTab].Count - 1);
                            tabs[fNum].Add(selectedCard.name);
                            DeselectCard();
                        }
                else 
                {
                    string cardToMove = selectedCard.name;
                    tabs[fNum].Add(cardToMove);
                    waste.Remove(cardToMove);
                    Transform child = wastePos.transform.Find(cardToMove);
                    Debug.Log("child: " + child.name);
                    child.parent = tabPos[fNum].transform;
                    Debug.Log("child.parent: " + child.parent.name);
                    Debug.Log("selected card: " + selectedCard.name);
                    child.localPosition =  new Vector3(0, 0f, -.03f);
                    DeselectCard();
                }
        }
    }

    private bool ValidMove(GameObject from, GameObject to)
    {
        string fromSuit = from.GetComponent<CardSpriteController>().suit;
        string toSuit = to.GetComponent<CardSpriteController>().suit;
        int fromVal = from.GetComponent<CardSpriteController>().value;
        int toVal = to.GetComponent<CardSpriteController>().value;
        if (fromVal != toVal - 1) return false;
        if ((fromSuit.Equals("D") || fromSuit.Equals("H")) && (toSuit.Equals("D") || toSuit.Equals("H"))) return false;
        if ((fromSuit.Equals("C") || fromSuit.Equals("S")) && (toSuit.Equals("C") || toSuit.Equals("S"))) return false;
        //if ((fromVal.Equals(13))) return true;
        return true;
    }

    private void DeselectCard()
    {
        selectedCard.GetComponent<SpriteRenderer>().color = Color.white;
        selectedCard = null;
        selectedTab = -1;
        selectedFoundation = -1;
    }

    public int UserChoiceBlue()
    {
        //cardPrefab = b;
        return test = 1;
    }

    public int UserChoiceRed()
    { 
        //cardPrefab = r;
        return test = 2;
    }
    

    
}