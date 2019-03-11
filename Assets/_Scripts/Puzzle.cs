using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    public Color explodeColour;
    public bool isOpen;
    public float speed, timeTaken;
    public Material explodeMat;
    public int assignedColour;
    public bool shieldActive, freezActivate;
    public int indexofpuzzle, countLeft;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        AssignRotation();
        //AssignColours();
    }


    void AssignRotation()
    {
        int Rand;
        Rand = Random.Range(0, 20);
        if (Rand < 10)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            isOpen = false;
        }
        else
            isOpen = true;
    }

    public void MoveToPlayer(int direction,bool rotateToLocation = false)
    {
        if (GameManager.instance.godMode)
        {
            timeTaken = GameManager.instance.godModeMovementSpeed;
        }
        else
        {
            timeTaken = GameManager.instance.puzzleMovementSpeed;
        }

        if (rotateToLocation && !GameManager.instance.godMode)
        {
            if(isOpen)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
                transform.DORotate(Vector3.zero, timeTaken - 0.3f);

            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.DORotate(new Vector3(0,0,90), timeTaken - 0.3f);
            }
            transform.DOScale(new Vector3(0.75f,0.75f,0.64f), timeTaken / 4).OnComplete(() =>
            {
                transform.DOScale(new Vector3(0.64f, 0.64f, 0.64f), timeTaken / 4).OnComplete(() =>
                {
                    transform.DOScale(new Vector3(0.75f, 0.75f, 0.64f), timeTaken / 4).OnComplete(() =>
                    {
                        transform.DOScale(new Vector3(0.64f, 0.64f, 0.64f), timeTaken / 4).OnComplete(() =>
                        {

                        });
                    });
                });
            });
        }

        //print(timeTaken);
        if (direction == 1)
        {
            transform.parent.DOLocalMoveY(Player.instance.transform.position.y + 2f, timeTaken).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!GameManager.instance.godMode)
                {

                    if (Player.instance.isOpen == isOpen)
                    {
                        if (!GameManager.instance.shieldActivated)
                        {
                            Player.instance.CryFace();
                        }
                        else
                        {
                            DestroyEnemy();
                            GameManager.instance.DeActivateShield();

                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOShakeRotation(0.3f, 45, 100);
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(0.5f, 0.15f).OnComplete(() =>
                            {
                                GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(1, 0.15f);
                            });

                      
                        }
                    }
                    else
                    {
                        if (shieldActive)
                        {
                            GameManager.instance.ActivateShield();
                        }
                        if (freezActivate)
                        {
                            GameManager.instance.ActiateSlowMotion();
                        }
                        DestroyEnemy();
                        Player.instance.HappyFace();
                        //Instantiate(GameManager.instance.ExplodeParticle);
                        GameManager.instance.imageContainer[indexofpuzzle].transform.DOShakeRotation(0.3f, 45, 100);
                        GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(0.5f, 0.15f).OnComplete(() =>
                        {
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(1, 0.15f);
                        });

                    }
                }
                else
                {
                    DestroyEnemy();
                    Instantiate(GameManager.instance.coinPrefab);
                    Player.instance.CamShake();
                    GameManager.instance.PlaySound(GameManager.instance.matchsound);
                    StartCoroutine(GameManager.instance.IncrementCoin(5));
                 
                }
                //Handheld.Vibrate();
            });
        }
        else if (direction == 2)
        {
            transform.parent.DOLocalMoveY(Player.instance.transform.position.y - 2.2f, timeTaken).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!GameManager.instance.godMode)
                {

                    if (Player.instance.isOpen == isOpen)
                    {
                        if (!GameManager.instance.shieldActivated)
                        {
                            Player.instance.CryFace();
                        }
                        else
                        {
                            DestroyEnemy();
                            //Instantiate(GameManager.instance.ExplodeParticle);
                            GameManager.instance.DeActivateShield();
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOShakeRotation(0.3f, 45, 100);
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(0.5f, 0.15f).OnComplete(() =>
                            {
                                GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(1, 0.15f);
                            });
                          
                        }
                    }
                    else
                    {
                        if (shieldActive)
                        {
                            GameManager.instance.ActivateShield();
                        }
                        if (freezActivate)
                        {
                            GameManager.instance.ActiateSlowMotion();
                        }
                        Player.instance.HappyFace();
                        DestroyEnemy();
                        //Instantiate(GameManager.instance.ExplodeParticle);
                        GameManager.instance.imageContainer[indexofpuzzle].transform.DOShakeRotation(0.3f, 45, 100);
                        GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(0.5f, 0.15f).OnComplete(() =>
                        {
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(1, 0.15f);
                        });
                     
                    }
                }
                else
                {
                    DestroyEnemy();
                    //Player.instance.HappyFace();
                    Instantiate(GameManager.instance.coinPrefab);
                    Player.instance.CamShake();
                    StartCoroutine(GameManager.instance.IncrementCoin(5));
                    GameManager.instance.PlaySound(GameManager.instance.matchsound);

                }
                //Handheld.Vibrate();
            });
        }
        else if (direction == 3)
        {
            transform.parent.DOLocalMoveX(Player.instance.transform.position.x + 2.2f, timeTaken).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!GameManager.instance.godMode)
                {
                    if (Player.instance.isOpen == isOpen)
                    {
                        if (!GameManager.instance.shieldActivated)
                        {
                            Player.instance.CryFace();
                        }
                        else
                        {
                            DestroyEnemy();
                            //Instantiate(GameManager.instance.ExplodeParticle);
                            GameManager.instance.DeActivateShield();
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOShakeRotation(0.3f, 45, 100);
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(0.5f, 0.15f).OnComplete(() =>
                            {
                                GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(1, 0.15f);
                            });

                        }
                    }
                    else
                    {
                        if (shieldActive)
                        {
                            GameManager.instance.ActivateShield();
                        }
                        if (freezActivate)
                        {
                            GameManager.instance.ActiateSlowMotion();
                        }
                        Player.instance.HappyFace();
                        DestroyEnemy();
                        GameManager.instance.imageContainer[indexofpuzzle].transform.DOShakeRotation(0.3f, 45, 100);
                        GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(0.5f, 0.15f).OnComplete(() =>
                        {
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(1, 0.15f);
                        });
                        //Instantiate(GameManager.instance.ExplodeParticle);

                    }
                }
                else
                {
                    DestroyEnemy();
                    Instantiate(GameManager.instance.coinPrefab);
                    Player.instance.CamShake();
                    StartCoroutine(GameManager.instance.IncrementCoin(5));
                    GameManager.instance.PlaySound(GameManager.instance.matchsound);

                }
                //Handheld.Vibrate();
            });
        }
        else if (direction == 4)
        {
            transform.parent.DOLocalMoveX(Player.instance.transform.position.x - 2.2f, timeTaken).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!GameManager.instance.godMode)
                {
                    if (Player.instance.isOpen == isOpen)
                    {
                        if (!GameManager.instance.shieldActivated)
                        {
                            Player.instance.CryFace();
                        }
                        else
                        {
                            DestroyEnemy();
                            //Instantiate(GameManager.instance.ExplodeParticle);
                            GameManager.instance.DeActivateShield();
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOShakeRotation(0.3f, 45, 100);
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(0.5f, 0.15f).OnComplete(() =>
                            {
                                GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(1, 0.15f);
                            });
                           
                        }
                    }
                    else
                    {
                        if (shieldActive)
                        {
                            GameManager.instance.ActivateShield();
                        }
                        if (freezActivate)
                        {
                            GameManager.instance.ActiateSlowMotion();
                        }
                        DestroyEnemy();
                        Player.instance.HappyFace();
                        GameManager.instance.imageContainer[indexofpuzzle].transform.DOShakeRotation(0.3f, 45, 100);
                        GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(0.5f, 0.15f).OnComplete(() =>
                        {
                            GameManager.instance.imageContainer[indexofpuzzle].transform.DOScale(1, 0.15f);
                        });
                        //Instantiate(GameManager.instance.ExplodeParticle);


                       
                    }
                }
                else
                {
                    DestroyEnemy();
                    Instantiate(GameManager.instance.coinPrefab);
                    Player.instance.CamShake();
                    StartCoroutine(GameManager.instance.IncrementCoin(5));
                    GameManager.instance.PlaySound(GameManager.instance.matchsound);

                }
                //Handheld.Vibrate();
            });
        }
    }


    void DestroyEnemy()
    {
        GameManager.instance.enemyCount -= 1;
        GameManager.instance.createdPuzzel.Remove(gameObject.transform.parent.gameObject);
        //Player.instance.HappyFace();
        Destroy(gameObject.transform.parent.gameObject);
        explodeMat.color = explodeColour;
        GameManager.instance.destroyedObjectsCount += 1;
        if (float.Parse(GameManager.instance.imageContainer[indexofpuzzle].transform.GetChild(2).GetComponent<Text>().text) > 10)
        {
            GameManager.instance.imageContainer[indexofpuzzle].transform.GetChild(2).GetComponent<Text>().text = (float.Parse(GameManager.instance.imageContainer[indexofpuzzle].transform.GetChild(2).GetComponent<Text>().text) - 1).ToString();
        }
        else
        {
            GameManager.instance.imageContainer[indexofpuzzle].transform.GetChild(2).GetComponent<Text>().text = 0 + (float.Parse(GameManager.instance.imageContainer[indexofpuzzle].transform.GetChild(2).GetComponent<Text>().text) - 1).ToString();
        }


        if (float.Parse(GameManager.instance.imageContainer[indexofpuzzle].transform.GetChild(2).GetComponent<Text>().text) == 0)
        {
            GameManager.instance.ActivateCheckBox(GameManager.instance.imageContainer[indexofpuzzle]);
        }
    }
}
