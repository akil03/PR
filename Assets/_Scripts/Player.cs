using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Player : MonoBehaviour 
{
    public static Player instance;
	public Transform puzzle,Cam;
	public bool isReady,isOpen,isDead;
    //public GameObject[] Faces;
    public float score;
    public Text ScoreTxt;
    public UIPop Bonus;
    public AudioClip coinSound,closeSound;
    public bool enlargedScreen1, enlargedScreen2, frenzy1, frenzy2;
    public float[] stageScoreLimit;
    public List<IntervalTimeGapController> timeGapControllers;
    public bool popUpShield, popUpFreez;
    public int currentAngle;

    public List<PuzzleDirection> degree;
    // Use this for initialization
    

    public Sequence puzzleSpin;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentAngle = 0;
        UpdateStatuesofPlayer();
    }

    public void UpdateStatuesofPlayer()
    {
        foreach (var item in degree)
        {
            // print(item.degree);
            if ((int)item.degree == currentAngle)
            {
                isOpen = item.value;
            }
        }
    }


    void Update ()
    {
       
        if (Input.GetKeyDown(KeyCode.Mouse0) && !GameManager.instance.godMode && GameManager.instance.gameStarted)
        {
            OnTap();
        }
	}

    public bool isClose;
	void OnTap()
	{       
        if (isDead)
        {
            //GameManager.instance.RestartGame();
        }
        if(GameManager.instance.gameOver)
        {
            return;
        }
        if (!isReady)
        {
            return;
        }
        isReady = false;

        isClose = true;

        currentAngle += 90;
        if (currentAngle >= 360)
        {
            currentAngle -= 360;
        }
        UpdateStatuesofPlayer();
        int rand = Random.Range(0, 2);
        GameManager.instance.PlaySound(rand == 1 ? GameManager.instance.flip01 : GameManager.instance.flip02);
        Vector3 newRot = new Vector3(puzzle.transform.rotation.eulerAngles.x, puzzle.transform.rotation.eulerAngles.y, puzzle.transform.rotation.eulerAngles.z+90);
        puzzle.DORotate(newRot, GameManager.instance.playerSpeed, RotateMode.Fast).OnComplete(() =>
        {
            isReady = true;
            isClose = false;
        });

    }

    public void CamShake()
    {
        if (!GameManager.instance.slowMotion)
        {
            Cam.GetComponent<Animator>().Play("CameraShake");
        }
    }





    //[ContextMenu("Test")]
    public void GodMode()
    {
        puzzleSpin.Append(puzzle.transform.DORotate(new Vector3(0, 0, 180), 0.2f).SetEase(Ease.Linear).OnComplete(() => {
            puzzleSpin.Append(puzzle.transform.DORotate(new Vector3(0, 0, 360), 0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                if(GameManager.instance.godMode)
                    GodMode();
            }));
        }));
    }


   // public void 

    public void HappyFace()
    {
        
        //
        CamShake();
        GameManager.instance.CalculateXP();
        if (isClose)
        {
            if (!GameManager.instance.godMode)
            {
                //HideFaces();
                GameManager.instance.OnCloseCallEvent();
                GameManager.instance.PlaySound(closeSound);
                if (!popUpFreez && !popUpShield)
                {

                    Bonus.OnScore("Close one !!");
                }
                else
                {
                    popUpFreez = false;
                    popUpShield = false;
                }
               
                Instantiate(GameManager.instance.coinPrefab);
                // Instantiate(GameManager.instance.ExplodeParticle);
                score += 100;
                //GameManager.instance.AddCoins();
                GameManager.instance.IncrementCharge();
                GameManager.instance.PlaySound(GameManager.instance.progressBar,0.2f);
                StartCoroutine(GameManager.instance.IncrementCoin(50));              
                if (GameManager.instance.vibration && !GameManager.instance.powerSaver)
                {
                    // Handheld.Vibrate();
                    //StartCoroutine(Vibrate());
                    HapticFeedback.Generate(UIFeedbackType.ImpactHeavy);
                }
                //CamShake();
            }
            else
            {
                StartCoroutine(GameManager.instance.IncrementCoin(5));

            }
        }
        else
        {
            if (!GameManager.instance.godMode)
            {
                if (!popUpFreez && !popUpShield)
                {

                    Bonus.OnScore("+50");
                }
                else
                {
                    popUpFreez = false;
                    popUpShield = false;
                }
                score += 50;
                Instantiate(GameManager.instance.ExplodeParticle);
                if (GameManager.instance.vibration && !GameManager.instance.powerSaver)
                {
                    HapticFeedback.Generate(UIFeedbackType.ImpactLight);
                    // StartCoroutine(Vibrate());
                }
            }
           
        }            

        GameManager.instance.PlaySound(GameManager.instance.matchsound);
        //Invoke("MehFace", 1.2f);
        
        ScoreTxt.transform.DOScale(Vector3.one * 0.15f, 0.2f).OnComplete(() =>
        {
            ScoreTxt.transform.DOScale(Vector3.one, 0.1f);
        });
        ScoreTxt.text = score.ToString();

        if (score > 1500 && !enlargedScreen1)
        {
            PlayerPrefs.SetInt("Crossed0", PlayerPrefs.GetInt("Crossed0", 0)+1);
            //enlargedScreen1 = true;
            GameManager.instance.EnlargeScreen();
        }

        if (score > 3000 && !enlargedScreen2)
        {
            PlayerPrefs.SetInt("Crossed1", PlayerPrefs.GetInt("Crossed1", 0)+1);
            enlargedScreen2 = true;
            GameManager.instance.EnlargeScreen2();
        }

        if (score > 5000 && !frenzy1)
        {
            PlayerPrefs.SetInt("Crossed1", PlayerPrefs.GetInt("Crossed1", 0)+1);
            frenzy1 = true;
            GameManager.instance.Frenzy();
        }

        if (score > 7500 && !frenzy2)
        {
            PlayerPrefs.SetInt("Crossed1", PlayerPrefs.GetInt("Crossed1", 0)+1);
            frenzy2 = true;
            GameManager.instance.Frenzy2();
        }

        for (int i = 0; i < stageScoreLimit.Length; i++)
        {
            if (score < stageScoreLimit[i])
            {
                float tempScore = score;
                float stageScore = stageScoreLimit[i];
                if (i != 0)
                {
                    tempScore -= stageScoreLimit[i - 1];
                    stageScore -= stageScoreLimit[i - 1];
                }
                else
                {
                    tempScore = score;
                    stageScore = stageScoreLimit[i];
                }
                float percent = tempScore / stageScoreLimit[i];
                for (int j = 0; j < timeGapControllers.Count; j++)
                {
                    if (timeGapControllers[j].PositionNumber == i)
                    {
                        GameManager.instance.TimeGap = timeGapControllers[j].initialSpeed - ((timeGapControllers[j].initialSpeed - timeGapControllers[j].endSpeed) * percent);

                        return;
                    }
                }
            }
        }

    }

    public void CryFace()
    {
        // HideFaces();
        GameManager.instance.OnFailureEvent();
        GameManager.instance.gameOver = true;
        isDead = true;
        DOTween.KillAll();
        GameManager.instance.OnGameOver();
        HapticFeedback.Generate(UIFeedbackType.Error);
        int rand = Random.Range(0, 2);
        GameManager.instance.PlaySound(rand == 1 ? GameManager.instance.missmatch01 : GameManager.instance.missmatch02);
        StartCoroutine(GameManager.instance.IncreaseXp());
    }

    private AudioClip MakeSubclip(AudioClip clip, float start, float stop)
    {
        /* Create a new audio clip */
        int frequency = clip.frequency;
        float timeLength = stop - start;
        int samplesLength = (int)(frequency * timeLength);
        AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, 1, frequency, false);
        /* Create a temporary buffer for the samples */
        float[] data = new float[samplesLength];
        /* Get the data from the original clip */
        clip.GetData(data, (int)(frequency * start));
        /* Transfer the data to the new clip */
        newClip.SetData(data, 0);
        /* Return the sub clip */
        return newClip;
    }


}


[System.Serializable]
public class IntervalTimeGapController
{
    //public float percent;
    //public float timeGap;
    public int PositionNumber;
    public float initialSpeed;
    public float endSpeed;
}