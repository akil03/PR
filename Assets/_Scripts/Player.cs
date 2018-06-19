using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Player : MonoBehaviour {
    public static Player instance;
	public Transform puzzle,Cam;
	public bool isReady,isOpen,isDead;
    public GameObject[] Faces;
    public int score;
    public Text ScoreTxt;
    public UIPop Bonus;
    public AudioClip coinSound,closeSound;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnTap();
        }
		
	}

    bool isClose;
	void OnTap()
	{
        
        if (isDead)
        {
            GameManager.instance.RestartGame();
        }

        if (!isReady)
            return;

        isReady = false;
        isOpen = !isOpen;
        isClose = true;
        Vector3 newRot = new Vector3(puzzle.transform.rotation.eulerAngles.x, puzzle.transform.rotation.eulerAngles.y, puzzle.transform.rotation.eulerAngles.z+90);
        puzzle.DORotate(newRot, 0.25f, RotateMode.Fast).OnComplete(()=>{
            
            isReady = true;
            isClose = false;
        });
        

    }

    public void CamShake()
    {
        Cam.GetComponent<Animator>().Play("CameraShake");
    }

    public void HideFaces()
    {
        foreach (GameObject go in Faces)
            go.SetActive(false);
    }

    public void MehFace()
    {
        HideFaces();
        Faces[0].SetActive(true);
    }

    public void HappyFace()
    {
        HideFaces();
        Faces[1].SetActive(true);
        CamShake();

        if (isClose)
        {
            GameManager.instance.PlaySound(closeSound);
            Bonus.OnScore("Close one !!");
            score += 100;
        }
        else
        {
            Bonus.OnScore("+50");
            score += 50;
        }            


        GameManager.instance.PlaySound(coinSound);


        Invoke("MehFace", 1.2f);
        
        ScoreTxt.transform.DOScale(Vector3.one * 0.15f, 0.2f).OnComplete(() =>
        {
            ScoreTxt.transform.DOScale(Vector3.one, 0.1f);
        });
        ScoreTxt.text = score.ToString();

        


        if (score > 1500)
            GameManager.instance.EnlargeScreen();

        if (score > 3000)
            GameManager.instance.EnlargeScreen2();

        if (score > 5000)
            GameManager.instance.Frenzy();

        if (score > 7500)
            GameManager.instance.Frenzy2();
    }

    public void CryFace()
    {
        HideFaces();
        Faces[2].SetActive(true);
        Time.timeScale = 0;
        isDead = true;
        GameManager.instance.OnGameOver();
    }



}
