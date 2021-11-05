using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameSys : KG.Singleton<GameSys>
{



    public List<int> CardFlopList = new List<int>();


    public Deck deck;
    public TextAsset deckXML;


    public List<Card> AllCard;

    public List<Card> D1Card;
    public List<Card> D2Card;
    public List<Card> D3Card;
    public List<Card> D4Card;

    public List<Card> DALLCard;


    public List<Card> CKCard;

    public List<Card> PZCard;

    public Card SelectCard;

    public Transform ClickPos, CKPos;


    public GameObject PD;

    public Text ScoreTxt;
    public int ScoreNum;

    public GameObject OverUI;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //deck = GetComponent<Deck>();
        //deck.InitDeck(deckXML.text);
        List<Vector3> tempV3 = AllCard.Select(v => v.transform.position).ToList();
        Deck.Shuffle(ref AllCard);

        for (int i = 0; i < AllCard.Count; i++)
        {
            AllCard[i].transform.position = tempV3[i];
        }

      

        for (int i = 0; i < 3; i++)
        {
            D1Card.Add(AllCard[i]);
        }
        for (int i = 3; i < 9; i++)
        {
            D2Card.Add(AllCard[i]);
        }
        for (int i = 9; i < 18; i++)
        {
            D3Card.Add(AllCard[i]);
        }
        for (int i = 18; i < 28; i++)
        {
            D4Card.Add(AllCard[i]);
        }
        for (int i = 28; i < AllCard.Count; i++)
        {
            PZCard.Add(AllCard[i]);
        }

        for (int i = 0; i < D1Card.Count; i++)
        {
            for (int t = 0; t < 2; t++)
            {
                D1Card[i].PushList.Add(D2Card[t+i*2]);
            }
        }
        D2Card[0].PushList.Add(D3Card[0]);
        D2Card[0].PushList.Add(D3Card[1]);

        D2Card[1].PushList.Add(D3Card[1]);
        D2Card[1].PushList.Add(D3Card[2]);

        D2Card[2].PushList.Add(D3Card[3]);
        D2Card[2].PushList.Add(D3Card[4]);
        D2Card[3].PushList.Add(D3Card[4]);
        D2Card[3].PushList.Add(D3Card[5]);

        D2Card[4].PushList.Add(D3Card[6]);
        D2Card[4].PushList.Add(D3Card[7]);
        D2Card[5].PushList.Add(D3Card[7]);
        D2Card[5].PushList.Add(D3Card[8]);



        for (int i = 0; i < D3Card.Count; i++)
        {
            for (int t = 0; t < 2; t++)
            {
                D3Card[i].PushList.Add(D4Card[t + i * 1]);
            }
        }

        PZCard.ForEach(v=>v.faceUp=true);
        D1Card.ForEach(v => DALLCard.Add(v));
        D2Card.ForEach(v => DALLCard.Add(v));
        D3Card.ForEach(v => DALLCard.Add(v));
        D4Card.ForEach(v => DALLCard.Add(v));

        D2Card.ForEach(v=>v.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s=>s.sortingOrder=1));
        D3Card.ForEach(v => v.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s => s.sortingOrder = 2));
        D4Card.ForEach(v => v.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s => s.sortingOrder = 3));
        OnUpdateCard();
        D2Card.ForEach(v => v.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s => s.sortingOrder = 1));
        D3Card.ForEach(v => v.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s => s.sortingOrder = 2));
        D4Card.ForEach(v =>
        {
            v.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s => s.sortingOrder = 3);
         //   Debug.Log(v.gameObject.name);
        }
      
        );


    }

    // Update is called once per frame
    void Update()
    {
        if (AllCard.FindAll(v => v != null).Count <= 0)
        {
            OverUI.SetTrue();
        }
        OnUpdateCard();
        ScoreTxt.text = "Score : "+ScoreNum.ToString();
    }

    public void OnUpdateCard()
    {
        DALLCard.ForEach(v =>
        {
            if (v!=null)
            {
                bool IsOn = true;
                IsOn = v.PushList.FindAll(p => p != null).Count > 0;
                if (DALLCard.Contains(v))
                {
                    IsOn = false;
                }
                v.faceUp = IsOn;
            }
          
         
        });
    }

    public void OnAddCK(Card card)
    {
        CKCard.Add(card);
        card.transform.position = CKPos.position;
        CKCard.ForEach(v => v.SetActiveF());
        CKCard[CKCard.Count - 1].SetActiveT();
        card.faceUp = false;
        OnUpdateCard();
    }
    public void OnPushCK()
    {
      
        if (CKCard.Count <= 0)
        {
            return;
        }
        CKCard.ForEach(v => v.SetActiveF());
        CKCard[CKCard.Count - 1].SetActiveT();
        CKCard[CKCard.Count - 1].faceUp = false;
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    public void ADDFQ()
    {
        PD = Instantiate(PD, PD.transform.position+Vector3.right*0.1f,Quaternion.identity);
    }
    public void CardClicked(Card card)
    {
        if (PZCard.Contains(card))
        {
            PZCard.Remove(card);
            OnAddCK(card);
            return;
        }

        if (card.faceUp == true&&DALLCard.Contains(card)==false)
        {


            return;
        }
        bool IsOn = true;
        IsOn = card.PushList.FindAll(p => p != null).Count > 0;
        if (IsOn)
        {
            return;
        }

        {
            if (SelectCard == null)
            {
                if (card.rank == 13)
                {
                    if (CKCard.Contains(card))
                    {
                        CKCard.Remove(card);
                    }
                    Destroy(card.gameObject);
                  //  Destroy(SelectCard.gameObject);
                    OnPushCK();
                    OnUpdateCard();
                    ScoreNum += 10;
                    PD.SetTrue();
                    if (AllCard.FindAll(v => v != null).Count <= 0)
                    {
                        OverUI.SetTrue();
                    }
                    OnUpdateCard();
                    return;
                }
                card.StartPos = card.transform.position;
                SelectCard = card;
                SelectCard.transform.position = ClickPos.position;
                OnUpdateCard();
                return;
            }
            else
            {
                if (card.rank + SelectCard.rank == 13)
                {
                    if (CKCard.Contains(card))
                    {
                        CKCard.Remove(card);
                    }
                    if (CKCard.Contains(SelectCard))
                    {
                        CKCard.Remove(SelectCard);
                    }
                    Destroy(card.gameObject);
                    Destroy(SelectCard.gameObject);
                    OnPushCK();
                    OnUpdateCard();
                    ScoreNum += 10;
                    PD.SetTrue();
                    if (AllCard.FindAll(v=>v!=null).Count<=0)
                    {
                        OverUI.SetTrue();
                    }
                    OnUpdateCard();
                }
                else
                {
                    SelectCard.transform.position = SelectCard.StartPos;
                   
                    card.StartPos = card.transform.position;
                    SelectCard = card;
                    SelectCard.transform.position = ClickPos.position;
                    OnUpdateCard();
                    if (card.rank == 13)
                    {
                        if (CKCard.Contains(card))
                        {
                            CKCard.Remove(card);
                        }
                        Destroy(card.gameObject);
                        //  Destroy(SelectCard.gameObject);
                        OnPushCK();
                        OnUpdateCard();
                        ScoreNum += 10;
                        PD.SetTrue();
                        if (AllCard.FindAll(v => v != null).Count <= 0)
                        {
                            OverUI.SetTrue();
                        }
                        OnUpdateCard();
                        return;
                    }
                }
            }
        }

    }


}
