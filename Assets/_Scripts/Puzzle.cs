using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Puzzle : MonoBehaviour {
    public GameObject ExplodeParticle;
	public Color[] mainColour, shadeColour, explodeColour;
    public bool isOpen;
    public float speed,timeTaken;
    public SpriteRenderer mainSprite, ShadeSprite;
    public Material explodeMat;
    public int assignedColour;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {

        AssignRotation();
        AssignColours();


    }

    public void AssignColours()
    {
        assignedColour = Random.Range(0, mainColour.Length);
        mainSprite.color = mainColour[assignedColour];
        ShadeSprite.color = shadeColour[assignedColour];
        

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


    public void MoveToPlayer(int direction)
    {
        if (direction == 1)
        {
            transform.DOLocalMoveY(Player.instance.transform.position.y + 2.2f, timeTaken).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (Player.instance.isOpen == isOpen)
                {
                    Player.instance.CryFace();
                }
                else
                {
                    Player.instance.HappyFace();
                    Destroy(gameObject);
                    explodeMat.color = explodeColour[assignedColour];
                    Instantiate(ExplodeParticle);
                }

                //Handheld.Vibrate();


            });
        }
        else if (direction == 2)
        {
            transform.DOLocalMoveY(Player.instance.transform.position.y - 2.2f, timeTaken).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (Player.instance.isOpen == isOpen)
                {
                    Player.instance.CryFace();
                }
                else
                {
                    Player.instance.HappyFace();
                    Destroy(gameObject);
                    explodeMat.color = explodeColour[assignedColour];
                    Instantiate(ExplodeParticle);
                }

                //Handheld.Vibrate();


            });
        }
        else if (direction == 3)
        {
            transform.DOLocalMoveX(Player.instance.transform.position.x + 2.2f, timeTaken).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (Player.instance.isOpen == isOpen)
                {
                    Player.instance.CryFace();
                }
                else
                {
                    Player.instance.HappyFace();
                    Destroy(gameObject);
                    explodeMat.color = explodeColour[assignedColour];
                    Instantiate(ExplodeParticle);
                }

                //Handheld.Vibrate();


            });
        }
        else if (direction == 4)
        {
            transform.DOLocalMoveX(Player.instance.transform.position.x - 2.2f, timeTaken).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (Player.instance.isOpen == isOpen)
                {
                    Player.instance.CryFace();
                }
                else
                {
                    Player.instance.HappyFace();
                    Destroy(gameObject);
                    explodeMat.color = explodeColour[assignedColour];
                    Instantiate(ExplodeParticle);
                }

                //Handheld.Vibrate();


            });
        }

        
    }


}
