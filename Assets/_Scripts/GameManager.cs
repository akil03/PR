using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Facebook.Unity;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
    public Color[] BGColours;
    
	public Camera GameCam;
    public Transform BG;
    public float TimeGap;
    public bool isEnlarged,isEnlarged2,isFrenzy, isFrenzy2;
    public PuzzleSpawner[] Spawners;
    public bool isAutoSpawn;


    public GameObject MainMenu, InGame, GameOver;
    public Text HighScoreTxt,EndScoreTxt;
    public GameObject AudioSourcePrefab;
    // Use this for initialization
    protected void Awake()
	{
        FB.Init();
        GameAnalytics.Initialize();
        Application.targetFrameRate = 60;
		instance = this;
	}
	void Start () {
        if (isAutoSpawn)
            Spawn();

        LoadScore();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void LoadScore()
    {
        HighScoreTxt.text = "High Score : "+PlayerPrefs.GetInt("HScore").ToString();
    }

    public void SaveScore()
    {
        if (Player.instance.score > PlayerPrefs.GetInt("HScore"))
            PlayerPrefs.SetInt("HScore", Player.instance.score);
    }


    public void StartGame()
	{
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
        MainMenu.SetActive(false);
        InGame.SetActive(true);
        Spawn();
    }

    public void OnGameOver()
    {
        InGame.SetActive(false);
        GameOver.SetActive(true);
        EndScoreTxt.text = "Score : " + Player.instance.score.ToString();
        SaveScore();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,"game", Player.instance.score);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        Application.LoadLevel(0);
    }

	public void EnlargeScreen()
	{
        if (isEnlarged)
            return;

        isEnlarged = true;
        BG.DOScale(Vector3.one * 40, 1f);
        BG.GetComponent<SpriteRenderer>().DOColor(BGColours[1], 1.5f);
        BG.DOMoveY(0, 1.5f);
        GameCam.transform.parent.DOMoveY(0, 1.5f);
        TimeGap = 0.62f;
        GameCam.DOOrthoSize(10, 3);

    }

    public void EnlargeScreen2()
    {
        if (isEnlarged2)
            return;

        isEnlarged2 = true;
        BG.DOScale(Vector3.one * 60, 1f);
        TimeGap = 0.52f;
        BG.GetComponent<SpriteRenderer>().DOColor(BGColours[2], 1.5f);
        GameCam.DOOrthoSize(15, 2);

    }

    public void Frenzy()
    {
        if (isFrenzy)
            return;

        isFrenzy = true;
        TimeGap = 0.45f;
        BG.GetComponent<SpriteRenderer>().DOColor(BGColours[3], 1.5f);
    }

    public void Frenzy2()
    {
        if (isFrenzy2)
            return;

        isFrenzy2 = true;
        TimeGap = 0.35f;
        BG.GetComponent<SpriteRenderer>().DOColor(BGColours[4], 1.5f);
    }


    public void Spawn()
    {
        int Rand = 0;
        if (isEnlarged)
            Rand = Random.Range(0, 2);

        if (isEnlarged2)
            Rand = Random.Range(0, 4);

        Spawners[Rand].Spawn();

        Invoke("Spawn", TimeGap);
    }

    public void PlaySound(AudioClip _clip)
    {
        GameObject SS = Instantiate(AudioSourcePrefab);
        SS.GetComponent<AudioSource>().PlayOneShot(_clip);
        Destroy(SS, _clip.length);
    }
}
