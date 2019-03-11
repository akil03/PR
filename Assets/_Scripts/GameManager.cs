using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Facebook.Unity;

//using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Space(5)]
    [Header("Camera")]
    public Camera GameCam;

    [Space(5)]
    [Header("UI Part")]
    public Image powerupFillImage;
    public Image countDownImage, xpProgressImage,xpProgressOnGameover;
    public GameObject MainMenu, InGame, GameOver, slider, noThanksButton, retryButton;
    public Text HighScoreTxt, HighScoreTxt2, EndScoreTxt, coinCountText, coinsCollectedText, costText, progressFromText,progressToText,gameoverBlocksDestroyedText, progressToTextGameover, progressFromTextGameover;
    public float sliderStartingX, sliderEndingX;
    public GameObject coinAnimation, continueButton;
    public GameObject[] imageContainer;
    public GameObject priceText;
    public GameObject soundOn, soundOff, vibrationOff, vibrationOn;
    //public GameObject unlockbutton;
    public Button isunlockedButton, islockedButton;

    [Space(5)]
    [Header("BG && Colours")]
    public Transform BG;
    public Color[] BGColours;

    [Space(5)]
    [Header("Spawner SetUp")]
    public float TimeGap;
    public float godModeTimeGap;
    public PuzzleSpawner[] Spawners;
    public float puzzleMovementSpeed, godModeMovementSpeed;

    [Space(5)]
    [Header("Booleans")]
    public bool isAutoSpawn;
    public bool isEnlarged, isEnlarged2, isFrenzy, isFrenzy2, godMode, shieldActivated, gameOver, slowMotion, gameStarted, vibration, powerSaver, shieldCreated,freezCreated;

    [Space(5)]
    [Header("Additional Variables")]
    public Image freezImage;
    public float chargePercentagePerCloseCall;
    public GameObject shield;
    public float playerSpeed = 0.2f;
    public int puzzlesCreatedCount;
    public int destroyedObjectsCount;
    // public float[] speedMultiplier;

    [Space(5)]
    [Header("Prefab Management")]
    public GameObject coinPrefab;
    public GameObject ExplodeParticle;
    public GameObject AudioSourcePrefab;


    [Space(5)]
    [Header("Enemy Setup")]
    public List<EnemyIndex> enemyIndex;
    public List<EnemyCreated> createdEnemy;
    public List<int> enemyCountInLevelProgress;
    //public List<int> enemytoBecrested;
    public int enemyLevelProgressIndex;
    public int enemyCount;

    [HideInInspector]
    public List<GameObject> createdPuzzel;
    public List<SpeedMultiplier> speedMultipliers;
    public List<EnemyInDesk> enemyToBeCreated;

    [Space(5)]
    [Header("Score Manager")]
    public float score, coin, coinsCollected;
    public int coinMultiplier;
    public int coinMultiplierinGodMode;

    [Space(5)]
    [Header("Audio Controller")]
    public AudioClip buttonClick;
    public AudioClip flip01, flip02, freezEffect, shieldEffect, progressBar, matchsound, missmatch01, missmatch02;

    public enum FaceExpressionsTypes
    {
        idle,
        happy,
        sad
    }

    [Space(5)]
    [Header("Charecter Selecton")]
    public int indexOftheCurrentCharecter;
    public int currentActiveCharecter;
    public List<CharecterSelection> charecterSelection;

    [Space(5)]
    [Header(" Expression Images")]
    public List<CharecterSprites> playerImages;
    public List<CharecterSprites> enemyImages;

    [Space(5)]


    [HideInInspector]
    public float chargePercent;
    // Use this for initialization


    Vector3 camPosition;
    float orthoSize, camScaleValue, BgScaleValue;
    int directionBeforeGodMode;
    bool decrement;
    public bool levelCompleted;
    //DoTween Sequence
    public Sequence powerupFillImageSequence;
    bool spinActive;

    #region Deegate
    public delegate void CharecterExpression();
    public static event CharecterExpression OnIdleAnimation, OnCloseOne, OnFailure;
    #endregion

    protected void Awake()
    {
        InitializeFB();
        // GameAnalytics.Initialize();
        Application.targetFrameRate = 60;
        instance = this;
    }

    void Start()
    {
       // PlayerPrefs.DeleteAll();
        coinCountText.text = "0";
        coinsCollected = PlayerPrefs.GetFloat("CoinsCollected", 0);
        coinsCollectedText.text = coinsCollected.ToString();
        if (isAutoSpawn)
            StartCoroutine(Spawn(TimeGap));
        LoadScore();
        CalculatePlayerPrefs();
        CreateEnemyList();
        StartCoroutine(ChangeBgColours());
        coin = 0;
        countDownImage.fillAmount = 0;
        countDownImage.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
        noThanksButton.transform.localPosition = new Vector3(0, -680, 0);
        noThanksButton.transform.localScale = Vector3.zero;
        if (PlayerPrefs.GetInt("Charecter0", 0) == 0)
        {
            PlayerPrefs.SetInt("Charecter0", 1);
        }
        currentActiveCharecter = PlayerPrefs.GetInt("CurrentActiveCharecter", 0);
        indexOftheCurrentCharecter = currentActiveCharecter;
        CheckUnlockStatus();
        CheckStartingCharecter();
        ChangeMute(PlayerPrefs.GetInt("Mute", 0));
        CheckVibrate(PlayerPrefs.GetInt("Vibrate", 0));
        OnIdleAnimation();
        CheckExpProgress();
    }

    public void ChangeMute(int index)
    {
        if (index == 0)
        {
            AudioListener.pause = false;
            PlayerPrefs.SetInt("Mute", index);
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            AudioListener.pause = true;
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            PlayerPrefs.SetInt("Mute", index);
        }
    }

    public void CheckVibrate(int index)
    {
        if (index == 0)
        {
            vibration = true;
            PlayerPrefs.SetInt("Vibrate", index);
            vibrationOn.SetActive(true);
            vibrationOff.SetActive(false);
        }
        else
        {
            vibration = false;
            vibrationOn.SetActive(false);
            vibrationOff.SetActive(true);
            PlayerPrefs.SetInt("Vibrate", index);
        }
    }


    public void CheckStartingCharecter()
    {
        foreach (CharecterSelection item in charecterSelection)
        {
            if (item.index == indexOftheCurrentCharecter)
            {
                item.charecter.transform.DOScale(1, 0.2f);
            }
            else
            {
                item.charecter.transform.localScale = Vector3.zero;
            }
        }
    }

    public void OnCloseCallEvent()
    {
        OnCloseOne();
    }

    public void OnFailureEvent()
    {
        OnFailure();
    }

    public void CalculatePlayerPrefs()
    {
        int gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
        PlayerPrefs.SetInt("GamesPlayed", gamesPlayed + 1);
        if (gamesPlayed <= 3 && gamesPlayed > 1)
        {
            chargePercentagePerCloseCall = 8;
        }
        if (gamesPlayed > 3 && gamesPlayed < 10)
        {
            chargePercentagePerCloseCall = 13;
        }
        else if (gamesPlayed >= 10)
        {
            chargePercentagePerCloseCall = 13;
        }
    }

    [ContextMenu("EnemyListTest")]
    public void CreateEnemyList()
    {
        if (!levelCompleted)
        {
            int[] enemyCount1 = SplitEnemyCount();
            puzzlesCreatedCount = enemyCountInLevelProgress[enemyLevelProgressIndex];
            destroyedObjectsCount = 0;
            enemyLevelProgressIndex += 1;
            Sprite[] enemySelected = ChooseEnemyFace();
            for (int i = 0; i < enemyCount1.Length; i++)
            {
                imageContainer[i].GetComponent<Image>().sprite = enemySelected[i];
                if (enemyCount1[i].ToString().Length > 1)
                {
                    imageContainer[i].gameObject.transform.GetChild(2).GetComponent<Text>().text = enemyCount1[i].ToString();
                }
                else
                {
                    imageContainer[i].gameObject.transform.GetChild(2).GetComponent<Text>().text = "0" + enemyCount1[i].ToString();
                }
                foreach (EnemyIndex item in enemyIndex)
                {
                    if (item.uiImage == enemySelected[i])
                    {
                        enemyToBeCreated[i].index = item.index;
                        enemyToBeCreated[i].enemyCount = enemyCount1[i];
                    }
                }
            }
        }

    }
    public List<int> indexofCreated;
    int[] SplitEnemyCount()
    {
        indexofCreated.Clear();
        int[] number = new int[imageContainer.Length];
        int tempNumber = 0;
        enemyCount = enemyCountInLevelProgress[enemyLevelProgressIndex];
        tempNumber = enemyCountInLevelProgress[enemyLevelProgressIndex];
        for (int i = 0; i < imageContainer.Length - 1; i++)
        {
            number[i] = Random.Range(1, tempNumber / 2);
            tempNumber -= number[i];
        }
        number[imageContainer.Length - 1] = tempNumber;

        for (int i = 0; i < imageContainer.Length; i++)
        {
            int randomNumber = Random.Range(0, enemyIndex.Count);
            if (!indexofCreated.Contains(randomNumber))
            {
                indexofCreated.Add(randomNumber);
                imageContainer[i].gameObject.GetComponent<Image>().sprite = enemyIndex[randomNumber].uiImage;
            }
            else
            {
                i--;
            }
        }
        return number;
    }


    public void LevelPassed()
    {
        CancelInvoke("StopGodMode");
        StopGodMode();
        CancelInvoke("DeActivateSlowMotion");
        CancelInvoke("DeActivateSlowMotion");
        DeActivateSlowMotion();

    }

    Sprite[] ChooseEnemyFace()
    {
        int numberofEnemy = imageContainer.Length;
        List<int> enemySelected = new List<int>();
        Sprite[] enemySprites = new Sprite[imageContainer.Length];
        for (int i = 0; i < numberofEnemy; i++)
        {
            enemySelected.Add(NewemenyIndex(enemySelected));
        }

        for (int i = 0; i < enemySprites.Length; i++)
        {
            enemySprites[i] = enemyIndex[enemySelected[i]].uiImage;
        }
        return enemySprites;
    }

    int NewemenyIndex(List<int> enemySelected)
    {
        int num = Random.Range(0, enemyIndex.Count);

        if (enemySelected.Contains(num))
        {
            num = NewemenyIndex(enemySelected);
        }
        return num;
    }

    public void LoadScore()
    {
        HighScoreTxt.text = "Best : " + PlayerPrefs.GetFloat("HScore").ToString();
        HighScoreTxt2.text = "Best : " + PlayerPrefs.GetFloat("HScore").ToString();
    }

    public void SaveScore()
    {
        print(Player.instance.score > PlayerPrefs.GetFloat("HScore", 0));
        if (Player.instance.score > PlayerPrefs.GetFloat("HScore"))
            PlayerPrefs.SetFloat("HScore", Player.instance.score);
    }

    private void Update()
    {
        powerSaver = SystemInfo.batteryLevel > 0.2f ? false : true;
        if (decrement)
        {
            if (slider.transform.localPosition.x < sliderEndingX)
                slider.transform.localPosition -= new Vector3(0.0002f * Time.timeScale, 0, 0);
        }
    }

    public void StartGame()
    {
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
        foreach (CharecterSelection item in charecterSelection)
        {
            //item.charecter.SetActive(false);
            item.charecter.transform.localScale = Vector3.zero;
            item.charecter.transform.GetChild(0).transform.GetChild(0).GetComponent<StopObjectFromRotating>().enabled = false;
        }
        int indexofCurrectCharecter = PlayerPrefs.GetInt("CurrentActiveCharecter", 0);
        charecterSelection[indexofCurrectCharecter].charecter.transform.DOScale(1, 0.5f);
        charecterSelection[indexofCurrectCharecter].charecter.transform.GetChild(0).transform.GetChild(0).GetComponent<StopObjectFromRotating>().enabled = true;
        Player.instance.degree = charecterSelection[indexofCurrectCharecter].directions;
        gameStarted = true;
        camPosition = GameCam.transform.position;
        orthoSize = GameCam.fieldOfView;
        camScaleValue = GameCam.transform.localScale.x;
        BgScaleValue = BG.transform.localScale.x;
        print(BgScaleValue);
        MainMenu.SetActive(false);
        powerupFillImage.fillAmount = 0;
        Time.timeScale = 1;
        InGame.SetActive(true);
        StartCoroutine(Spawn(TimeGap));
    }


    public void OnGameOver()
    {
        gameStarted = false;
        SaveScore();
        DOTween.KillAll();
        InGame.SetActive(false);
        GameOver.SetActive(true);
        DOTween.KillAll();
        PlayerPrefs.SetFloat("CoinsCollected", PlayerPrefs.GetFloat("CoinsCollected") + coin);
        coinsCollected = PlayerPrefs.GetFloat("CoinsCollected", 0) + coin;
        coinsCollectedText.text = coinsCollected.ToString();
        StartCountDown();
        EndScoreTxt.text = Player.instance.score.ToString();
        IncreaseXp();
        LoadScore();
        CheckSpeed(Player.instance.score);
        // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,"game", Player.instance.score);
    }

    public void StartCountDown()
    {
        // print("asdffffffffffff");
        if (countDownImage.gameObject.activeSelf)
        {
            Invoke("NothanksAppears", 2.5f);
            countDownImage.DOFillAmount(1, 5f).SetEase(Ease.Linear).OnComplete(() =>
            {

                DeactivareContinue();
            });
        }
        else
        {
            DeactivareContinue();
        }
    }

    void DeactivareContinue()
    {
        retryButton.SetActive(true);
        retryButton.transform.DOLocalMoveY(-350, 0.3f);
        retryButton.transform.DOScale(1.5f, 0.3f);
        countDownImage.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        noThanksButton.gameObject.SetActive(false);
    }

    public void NothanksAppears()
    {
        //  print("1111111111111111");
        noThanksButton.gameObject.SetActive(true);
        noThanksButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
    }

    private void InitializeAppFlyer()
    {

    }

    private void InitializeApplovin()
    {

    }

    private void InitializeFB()
    {
        FB.Init();
    }

    public void Continue()
    {
        gameStarted = true;
        StopAllCoroutines();
        continueButton.SetActive(false);
        slider.transform.DOLocalMoveX(sliderStartingX, 0.5f);
        CancelInvoke("DeActivateSlowMotion");
        UIPop.instance.Continue();
        StopAllCoroutines();
        powerupFillImage.fillAmount = 0;
        GameOver.SetActive(false);
        InGame.SetActive(true);
        foreach (GameObject item in createdPuzzel)
        {
            ReassignSpawnedCount(item.transform.GetChild(0).GetComponent<Puzzle>().indexofpuzzle);
            enemyCount += 1;
            Destroy(item);
        }
        DeActivateSlowMotion();
        createdPuzzel.Clear();
        Player.instance.isDead = false;
        Player.instance.puzzle.transform.DORotate(new Vector3(0, 0, 0), 0.1f).OnComplete(() =>
        {
            Player.instance.isReady = true;
            Player.instance.currentAngle = 0;
            Player.instance.UpdateStatuesofPlayer();
            Player.instance.isClose = false;
            gameOver = false;
        });
        StartCoroutine(Spawn(TimeGap));
        OnIdleAnimation();
        DeactivareContinue();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        Application.LoadLevel(1);
    }

    public void EnlargeScreen()
    {
        if (isEnlarged)
            return;
        isEnlarged = true;
        BG.DOScale(1.15f, 1.5f);
        BG.DOMoveY(0, 1.5f);
        camPosition = new Vector3(GameCam.transform.position.x, 0, GameCam.transform.position.z);
        orthoSize = 75;
        BgScaleValue = 1.12f;
        GameCam.transform.parent.DOMoveY(0, 1.5f).OnComplete(() =>
        {
            Player.instance.enlargedScreen1 = true;
        });
        GameCam.DOFieldOfView(75, 1);
    }

    public void IncrementCharge()
    {
        decrement = false;
        powerupFillImageSequence.Kill(true);
        StopCoroutine(DecrementCharge());
        powerupFillImage.gameObject.transform.DOShakePosition(0.2f, 10);
       // print(chargePercentagePerCloseCall);
        powerupFillImageSequence.Append(slider.transform.DOLocalMoveX((slider.transform.localPosition.x + ((sliderEndingX + 1) - sliderStartingX) / (chargePercentagePerCloseCall)), 0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (slider.transform.localPosition.x >= sliderEndingX)
                {
                    GodMode();
                }
                else
                {
                    StartCoroutine(DecrementCharge());
                }
            }));
    }

    public void AddCoins()
    {
        coin += 1;
        Instantiate(coinAnimation);
        coinCountText.text = coin.ToString();
    }

    public IEnumerator DecrementCharge()
    {
        yield return new WaitForSeconds(5f);
        decrement = true;
    }

    [ContextMenu("GodMode")]
    public void GodMode()
    {
        StopCoroutine(ChangeBgColours());
        godMode = true;
        Player.instance.GodMode();
        GodModeCamera();
        StartCoroutine(SwitchColour());
        DeActivateSlowMotion();
        Invoke("StopGodMode", 3f);
        Invoke("ActivateBonusText", 0.5f);
    }

    void ActivateBonusText()
    {
        UIPop.instance.ShowText("Bonus!");
    }

    public void StopGodMode()
    {
        //DOTween.Kill(Player.instance.puzzleSpin);
        slider.transform.DOLocalMoveX(sliderStartingX, 0.5f);
        CancelInvoke();
        StopAllCoroutines();

        Invoke("CameraBackToNormalPosition", 1f);
        //StartCoroutine(ChangeBgColours());
        DeActivateSlowMotion();
    }


    public void CameraBackToNormalPosition()
    {
        BG.transform.DOScale(BgScaleValue, 0.5f);
        GameCam.DOFieldOfView(orthoSize, 0.5f);
        GameCam.transform.parent.DOMove(camPosition, 0.5f, false).OnComplete(() =>
          {
              //powerupFillImage.fillAmount = 0;
              // Player.instance.isOpen = true;
              Player.instance.currentAngle = 0;
              Player.instance.UpdateStatuesofPlayer();
              godMode = false;
              CancelInvoke();
              StopAllCoroutines();

              if (levelCompleted)
              {
                  levelCompleted = false;
                  Invoke("CreateEnemyList", 0.5f);
                  Invoke("DeActivateCheckBox", 0.5f);
                  Invoke("ActivateSpawn", 0.75f);
              }
              else
              {
                  StartCoroutine(Spawn(0.75f));
              }
          });
    }

    IEnumerator SwitchColour()
    {
        yield return new WaitForSeconds(0.2f);
        //BG.GetComponent<SpriteRenderer>().DOColor(BGColours[Random.Range(0, BGColours.Length)], 0.2f);
        StartCoroutine(SwitchColour());
    }

    IEnumerator ChangeBgColours()
    {
        yield return new WaitForSeconds(Random.Range(25, 50));
        // BG.GetComponent<SpriteRenderer>().DOColor(BGColours[Random.Range(0, BGColours.Length)], 1.5f);
    }

    public void ActivateShield()
    {
        if (!shieldActivated)
        {
            Player.instance.popUpShield = true;
            int createdShieldinGame = PlayerPrefs.GetInt("ShieldActivated", 0);           
            if (createdShieldinGame == 1)
            {
                UIPop.instance.ShowText("Shield !");
            }
            else
            {
                PlayerPrefs.SetInt("ShieldActivated", 1);
            }
            shield.SetActive(true);
            shieldActivated = true;
            GameManager.instance.PlaySound(GameManager.instance.shieldEffect);
        }
    }

    public void DeActivateShield()
    {
        int shieldDeactivatedingame = PlayerPrefs.GetInt("ShieldDeactivated", 0);
        if (shieldDeactivatedingame == 0)
        {
            Time.timeScale = 0.4f;
            Invoke("NormalTime", 0.5f);
            PlayerPrefs.SetInt("ShieldDeactivated", 1);
        }
        UIPop.instance.ShowText("Shield Broken!");
        shield.SetActive(false);
        shieldActivated = false;
        shieldCreated = false;
    }

    void NormalTime()
    {
        Time.timeScale = 1;
    }

    [ContextMenu("Activate Slow Motion")]
    public void ActiateSlowMotion()
    {
        if (!slowMotion)
        {
            Player.instance.popUpFreez = true;
            //GameCam.transform.GetComponent<Animator>().Play("FrostEffect");
            int createdFreezinGame = PlayerPrefs.GetInt("FreezActivated", 0);
            if (createdFreezinGame == 1)
            {
                UIPop.instance.ShowText("Relax");
            }
            else
            {
                PlayerPrefs.SetInt("FreezActivated", 1);
            }
            freezImage.DOFade(1, 0.5f);
            Time.timeScale = 0.7f;
            playerSpeed = 0.14f;
            slowMotion = true;
            Invoke("DeActivateSlowMotion", 5f);
            Invoke("ActivateSpeedText", 4.5f);
            GameManager.instance.PlaySound(GameManager.instance.freezEffect);
        }
    }

    void ActivateSpeedText()
    {
        UIPop.instance.ShowText("Speedy");
    }

    [ContextMenu("De-Activate Slow Motion")]
    public void DeActivateSlowMotion()
    {
        //GameCam.transform.GetComponent<Animator>().Play("FrostEffectFade");
       
        freezImage.DOFade(0, 0.5f);
        Time.timeScale = 1;
        playerSpeed = 0.2f;
        slowMotion = false;
        freezCreated = false;
    }

    public void GodModeCamera()
    {
        GameCam.transform.parent.DOMoveY(0, 1.5f);
        //GameCam.DOOrthoSize(10, 3);
        BG.DOScale(2.12f, 1f);
        GameCam.DOFieldOfView(85, 1);
    }

    public void EnlargeScreen2()
    {
        if (isEnlarged2)
        {
            return;
        }
        isEnlarged2 = true;
        camPosition = new Vector3(GameCam.transform.position.x, 0, GameCam.transform.position.z);
        orthoSize = 85;
        camScaleValue = GameCam.transform.localScale.x;
        BgScaleValue = 1.12f;
        GameCam.DOFieldOfView(85, 1);
    }

    public void Frenzy()
    {
        if (isFrenzy)
        {
            return;
        }
        isFrenzy = true;
        camPosition = GameCam.transform.position;
        BgScaleValue = 1.12f;
        orthoSize = GameCam.fieldOfView;
        camScaleValue = GameCam.transform.localScale.x;
    }

    public void Frenzy2()
    {
        if (isFrenzy2)
        {
            return;
        }
        isFrenzy2 = true;
        camPosition = GameCam.transform.position;
        BgScaleValue = 1.12f;
        orthoSize = GameCam.fieldOfView;
        camScaleValue = GameCam.transform.localScale.x;
    }

    GameObject puzzle;
    int indexofpuzzle;
    public IEnumerator Spawn(float timeGap)
    {
        yield return new WaitForSeconds(timeGap);
        if (!Player.instance.isDead)
        {
            List<int> possibleEnemyIndex = new List<int>();
            for (int i = 0; i < enemyToBeCreated.Count; i++)
            {
                if (enemyToBeCreated[i].enemyCount > 0)
                {
                    possibleEnemyIndex.Add(i);
                }
                else
                {
                    //return;

                }
            }
            if (possibleEnemyIndex.Count > 0)
            {
                int randomNumber = Random.Range(0, possibleEnemyIndex.Count);
                foreach (EnemyIndex item in enemyIndex)
                {
                    if (item.index == enemyToBeCreated[possibleEnemyIndex[randomNumber]].index)
                    {

                        puzzle = item.enemy;
                        enemyToBeCreated[possibleEnemyIndex[randomNumber]].enemyCount -= 1;
                        //enemyCount1 = enemyToBeCreated[possibleEnemyIndex[randomNumber]].enemyCount;
                        indexofpuzzle = possibleEnemyIndex[randomNumber];
                    }
                }

                int Rand = 0;
                if (isEnlarged)
                    Rand = Random.Range(0, 2);
                if (isEnlarged2)
                    Rand = Random.Range(0, 4);
                if (!godMode)
                {
                    Spawners[Rand].Spawn(puzzle, indexofpuzzle);
                    StartCoroutine(Spawn(TimeGap));
                }
                else
                {
                    int Rand1 = Random.Range(0, 4);
                    Spawners[Rand1].Spawn(puzzle, indexofpuzzle);
                    StartCoroutine(Spawn(godModeTimeGap));
                }
            }
        }
    }

    public void ReassignSpawnedCount(int index)
    {
        enemyToBeCreated[index].enemyCount += 1;
        if (enemyToBeCreated[index].enemyCount.ToString().Length > 1)
        {
            imageContainer[index].transform.GetChild(2).GetComponent<Text>().text = enemyToBeCreated[index].enemyCount.ToString();
        }
        else
        {
            imageContainer[index].transform.GetChild(2).GetComponent<Text>().text = "0" + enemyToBeCreated[index].enemyCount.ToString();
        }
    }

    public void PlaySound(AudioClip _clip,float audioVolume = 0.8f)
    {
        GameObject SS = Instantiate(AudioSourcePrefab);
        SS.GetComponent<AudioSource>().volume = audioVolume;
        SS.GetComponent<AudioSource>().PlayOneShot(_clip);
        Destroy(SS, _clip.length);
    }

    public void CheckSpeed(float currentScore)
    {
        for (int i = 0; i < 5; i++)
        {
            string maxString = "maxSpeed" + i.ToString();
            string minString = "minSpeed" + i.ToString();

            float maxivalue = PlayerPrefs.GetFloat(maxString, 0);
            float minivalue = PlayerPrefs.GetFloat(minString, 0);
            if (minivalue == 0)
            {
                PlayerPrefs.SetFloat(maxString, Player.instance.timeGapControllers[i].initialSpeed);
            }
            if (maxivalue == 0)
            {
                PlayerPrefs.SetFloat(maxString, Player.instance.timeGapControllers[i].endSpeed);
            }
            else
            {
                if (currentScore >= speedMultipliers[i].scoreValueFrom)
                {
                    if (maxivalue > minivalue)
                    {
                        PlayerPrefs.SetFloat(maxString, maxivalue - speedMultipliers[i].speedMultiplier);
                    }
                }
            }
        }
    }


    public void ActivateCheckBox(GameObject imageIndexObject)
    {
        imageIndexObject.transform.GetChild(2).gameObject.SetActive(false);
        imageIndexObject.transform.GetChild(3).gameObject.SetActive(true);
        if (destroyedObjectsCount == puzzlesCreatedCount)
        {
            Invoke("LevelUpText",0.5f);
            if (godMode)
            {
                levelCompleted = true;
                StopGodMode();
                return;
            }

            Invoke("CreateEnemyList", 1f);
            Invoke("DeActivateCheckBox", 1f);
            Invoke("ActivateSpawn", 1.5f);
        }
    }

    void LevelUpText()
    {
        UIPop.instance.ShowText("Level Up!");
    }

    public IEnumerator IncrementCoin(int coinCount)
    {
        if (!godMode)
            for (int i = 0; i < coinCount; i++)
            {
                coin += 1;
                if (i % 5 == 0 && i > 5)
                {
                    yield return new WaitForSeconds(0.01f);
                    coinCountText.text = coin.ToString();
                }
                else
                {
                    coinCountText.text = coin.ToString();
                }
            }
        else
        {
            coin += coinCount;
            coinCountText.text = coin.ToString();
        }
    }



    public void DeActivateCheckBox()
    {
        if (!levelCompleted)
        {
            foreach (GameObject item in imageContainer)
            {
                item.transform.GetChild(2).gameObject.SetActive(true);
                item.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
    }

    public void ActivateSpawn()
    {
        if (!levelCompleted)
        {
            StartCoroutine(Spawn(TimeGap));
        }
    }

    Sequence charecterSelectionAnimation, buttonAnimation, textAnimation;


    public void ChangeCharecter(int index)
    {
        if (!spinActive)
        {
           // print(indexOftheCurrentCharecter);
            if (index < 0)
            {
                indexOftheCurrentCharecter -= 1;
                ChangePreviousPlayer(indexOftheCurrentCharecter);
                CheckUnlockStatus();
                ///currentActiveCharecter = indexOftheCurrentCharecter;
            }
            if (index > 0)
            {
                indexOftheCurrentCharecter += 1;
                ChangeNextPlayer(indexOftheCurrentCharecter);
                CheckUnlockStatus();
                //currentActiveCharecter = indexOftheCurrentCharecter
            }
        }
    }



    public void ChangeNextPlayer(int currentPlayer)
    {
        //DOTween.KillAll();
        if (currentPlayer < charecterSelection.Count)
        {
            spinActive = true;
            charecterSelection[currentPlayer - 1].charecter.transform.GetChild(0).transform.GetChild(0).GetComponent<StopObjectFromRotating>().enabled = false;
            charecterSelectionAnimation.Append(charecterSelection[currentPlayer - 1].charecter.transform.DOScale(0, 0.25f).SetEase(Ease.Linear))
                                      .Append(charecterSelection[currentPlayer - 1].charecter.transform.DORotate(new Vector3(0, 720, 0), 0.25f, RotateMode.FastBeyond360).OnComplete(() =>
                                       {
                                           charecterSelection[currentPlayer].charecter.transform.GetChild(0).transform.GetChild(0).GetComponent<StopObjectFromRotating>().enabled = false;
                                           charecterSelectionAnimation.Append(charecterSelection[currentPlayer].charecter.transform.DOScale(1, 0.25f))
                                                                      .Append(charecterSelection[currentPlayer].charecter.transform.DORotate(new Vector3(0, -720, 0), 0.25f, RotateMode.FastBeyond360).OnComplete(() =>
                                                                      {
                                                                          charecterSelection[currentPlayer].charecter.transform.GetChild(0).transform.GetChild(0).GetComponent<StopObjectFromRotating>().enabled = true;
                                                                          spinActive = false;
                                                                      }));
                                       }));

        }
    }

    public void UnlockCharecter()
    {
        int index = islockedButton.gameObject.GetComponent<IndexOfTheCharecter>().indexOfTheCharecter;
        foreach (CharecterSelection item in charecterSelection)
        {
            if (index == item.index)
            {
                if (coinsCollected > item.pricetoUnlock)
                {
                    coinsCollected -= item.pricetoUnlock;
                    coinsCollectedText.text = coinsCollected.ToString();
                    PlayerPrefs.SetFloat("CoinsCollected", coinsCollected);
                    PlayerPrefs.SetInt("Charecter" + index, 1);
                    CheckUnlockStatus();
                }
            }
        }

    }

    public void UseCharecter()
    {
        int index = isunlockedButton.gameObject.GetComponent<IndexOfTheCharecter>().indexOfTheCharecter;
        currentActiveCharecter = index;
        PlayerPrefs.SetInt("CurrentActiveCharecter", currentActiveCharecter);
        CheckUnlockStatus();
    }

    public void ChangePreviousPlayer(int currentPlayer)
    {
        // DOTween.KillAll();
        if (currentPlayer >= 0)
        {
            spinActive = true;
            charecterSelection[currentPlayer + 1].charecter.transform.GetChild(0).transform.GetChild(0).GetComponent<StopObjectFromRotating>().enabled = false;
            charecterSelectionAnimation.Append(charecterSelection[currentPlayer + 1].charecter.transform.DOScale(0, 0.25f).SetEase(Ease.Linear))
                                       .Append(charecterSelection[currentPlayer + 1].charecter.transform.DORotate(new Vector3(0, 720, 0), 0.25f, RotateMode.FastBeyond360).OnComplete(() =>
                                        {
                                            charecterSelection[currentPlayer].charecter.transform.GetChild(0).transform.GetChild(0).GetComponent<StopObjectFromRotating>().enabled = false;
                                            charecterSelectionAnimation.Append(charecterSelection[currentPlayer].charecter.transform.DOScale(1, 0.25f))
                                                                       .Append(charecterSelection[currentPlayer].charecter.transform.DORotate(new Vector3(0, -720, 0), 0.25f, RotateMode.FastBeyond360).OnComplete(() =>
                                                                        {
                                                                            charecterSelection[currentPlayer].charecter.transform.GetChild(0).transform.GetChild(0).GetComponent<StopObjectFromRotating>().enabled = true;
                                                                            spinActive = false;
                                                                        }));
                                        }));
        }
    }


    float progressFillAmount, savedProgress, progressLevel;

    public int blocksDestroyed, previousDestroyed;

    public void CheckExpProgress()
    {
        savedProgress = PlayerPrefs.GetFloat("ProgressValue", 0);
        progressLevel = PlayerPrefs.GetFloat("PlayerLevel",0);
        previousDestroyed = PlayerPrefs.GetInt("previousDestroyed", 0);
        progressFromText.text = progressLevel.ToString();
        progressFromTextGameover.text = progressLevel.ToString();
        progressToText.text = (progressLevel + 1).ToString();
        progressToTextGameover.text = (progressLevel + 1).ToString();
        xpProgressImage.fillAmount = savedProgress;
        xpProgressOnGameover.fillAmount = savedProgress;
        progressFillAmount = 100 + progressLevel*25;
        blocksDestroyed = 0;
    }


    public void CalculateXP()
    {
        blocksDestroyed += 1;
    }

    public IEnumerator IncreaseXp()
    {
        PlayerPrefs.SetInt("previousDestroyed", blocksDestroyed+previousDestroyed);
        for (int i = 0; i < blocksDestroyed; i++)
        {           
            if (i % 5 == 0)
            {
                yield return new WaitForSeconds(0.075f);
            } else
            {
                yield return new WaitForSeconds(0.01f);
            }
            previousDestroyed += 1;
            xpProgressOnGameover.fillAmount += (1 / progressFillAmount);
            if (xpProgressOnGameover.fillAmount >= 1)
            {
                xpProgressOnGameover.fillAmount = 0;
                progressFillAmount += 25;
                progressLevel += 1;
                PlayerPrefs.SetFloat("PlayerLevel", progressLevel);
                progressFromText.text = progressLevel.ToString();
                progressFromTextGameover.text = progressLevel.ToString();
                progressToText.text = (progressLevel + 1).ToString();
                progressToTextGameover.text = (progressLevel+1).ToString();
                PlayerPrefs.SetInt("previousDestroyed", blocksDestroyed-i);
                previousDestroyed = 0;
            }
            gameoverBlocksDestroyedText.text = ((previousDestroyed)+ " / " + progressFillAmount);
            PlayerPrefs.SetFloat("ProgressValue", xpProgressOnGameover.fillAmount);
        }
        blocksDestroyed = 0;
    }

    public void CheckUnlockStatus()
    {
        foreach (CharecterSelection item in charecterSelection)
        {
            textAnimation.Kill();
            buttonAnimation.Kill();
            islockedButton.transform.localScale = Vector3.zero;
            priceText.gameObject.transform.localScale = Vector3.one * 5;
            priceText.gameObject.GetComponent<Image>().DOFade(0, 0.01f);
            priceText.transform.GetChild(0).gameObject.GetComponent<Text>().DOFade(0, 0.01f);
            if (indexOftheCurrentCharecter == item.index)
            {
                priceText.gameObject.SetActive(false);
                isunlockedButton.transform.localScale = Vector3.one;


                int unlockStatus = PlayerPrefs.GetInt("Charecter" + item.index, 0);
                if (unlockStatus == 1)
                {
                    if (currentActiveCharecter == item.index)
                    {
                        //isInUseButton.SetActive(true);
                        isunlockedButton.gameObject.SetActive(false);
                        islockedButton.gameObject.SetActive(false);
                        priceText.gameObject.SetActive(false);
                    }
                    else
                    {
                        //isInUseButton.SetActive(false);
                        isunlockedButton.gameObject.SetActive(true);
                        buttonAnimation.Append(isunlockedButton.transform.DOScale(1, 0.3f).SetDelay(0.4f).SetEase(Ease.InExpo));
                        isunlockedButton.gameObject.GetComponent<IndexOfTheCharecter>().indexOfTheCharecter = indexOftheCurrentCharecter;
                        islockedButton.gameObject.SetActive(false);
                        priceText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    //isInUseButton.SetActive(false);
                    isunlockedButton.gameObject.SetActive(false);
                    islockedButton.gameObject.SetActive(true);
                    buttonAnimation.Append(islockedButton.gameObject.transform.DOScale(1, 0.3f).SetDelay(0.4f).SetEase(Ease.InExpo));
                    islockedButton.GetComponent<IndexOfTheCharecter>().indexOfTheCharecter = indexOftheCurrentCharecter;
                    priceText.gameObject.transform.GetChild(0).GetComponent<Text>().text = item.pricetoUnlock.ToString();
                    priceText.gameObject.SetActive(true);
                    textAnimation.Append(priceText.transform.DOScale(1, 0.3f).SetDelay(0.5f).SetEase(Ease.InExpo))
                                 .Append(priceText.transform.GetChild(0).gameObject.GetComponent<Text>().DOFade(1, 0.3f).SetDelay(0.7f))
                                 .Append(priceText.gameObject.GetComponent<Image>().DOFade(0.7f, 0.3f).SetDelay(0.7f));
                }

            }
        }
    }

}



[System.Serializable]
public class SpeedMultiplier
{
    public float scoreValueFrom, scoreValueTo, speedMultiplier, gameCount;
}

[System.Serializable]
public class EnemyIndex
{
    public int index;
    public GameObject enemy;
    public Sprite uiImage;
}

[System.Serializable]
public class EnemyCreated
{
    public int indexofTheEnemy;
    public int enemysToCreateCount;
    public int enemysCreated;
}

[System.Serializable]
public class EnemyInDesk
{
    public int index;
    public int enemyCount;
}

[System.Serializable]
public class CharecterSelection
{
    public int index;
    public GameObject charecter;
    public float pricetoUnlock;
    public bool unlockStatus;
    public List<PuzzleDirection> directions;
}

[System.Serializable]
public class CharecterSprites
{
    public List<CharecterImages> charecterImages;
}

[System.Serializable]
public class CharecterImages
{
    public GameManager.FaceExpressionsTypes faceType;
    public Sprite[] Images;
    public float fps;
}

[System.Serializable]
public class PuzzleDirection
{
    public DirectionDegree degree;
    public bool value;
}


public enum DirectionDegree
{
    Zero = 0,
    Ninety = 90,
    Oneeighty = 180,
    Twoseventy = 270
}
