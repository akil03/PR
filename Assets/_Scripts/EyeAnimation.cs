using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAnimation : MonoBehaviour {

    //Animator anim;
    public bool isPlayer;

   public  Sprite[] idleSprites;
   public  Sprite[] happySprites;
   public  Sprite[] sadSprites;
    float idlefps,happyfps,sadfps;

    SpriteRenderer imageTobeChanged;
    // Use this for initialization
    void Start()
    {
        //anim = gameObject.GetComponent<Animator>();
        imageTobeChanged = gameObject.GetComponent<SpriteRenderer>();
        SelectImages();
        StartAnimation();
        StartCoroutine(StartIdleAnimation(Random.Range(1,2)));
    }

    private void OnEnable()
    {
        GameManager.OnIdleAnimation += GameManager_OnIdleAnimation;
        GameManager.OnFailure += GameManager_OnFailure;
        GameManager.OnCloseOne += GameManager_OnCloseOne;
    }

    private void OnDisable()
    {
        GameManager.OnIdleAnimation -= GameManager_OnIdleAnimation;
        GameManager.OnFailure -= GameManager_OnFailure;
        GameManager.OnCloseOne -= GameManager_OnCloseOne;
    }

    void SelectImages()
    {
        if(isPlayer)
        {
            int num = Random.Range(0, GameManager.instance.playerImages.Count);
            CharecterSprites sprts = GameManager.instance.playerImages[num];
            foreach (CharecterImages item in sprts.charecterImages)
            {
                if(item.faceType == GameManager.FaceExpressionsTypes.idle)
                {
                    idleSprites = item.Images;
                    idlefps = item.fps;
                }
                if (item.faceType == GameManager.FaceExpressionsTypes.happy)
                {
                    happySprites = item.Images;
                    happyfps = item.fps;
                }
                if (item.faceType == GameManager.FaceExpressionsTypes.sad)
                {
                    sadSprites = item.Images;
                    sadfps = item.fps;
                }
            }
        }
        else
        {
            int num = Random.Range(0, GameManager.instance.enemyImages.Count);
            CharecterSprites sprts = GameManager.instance.enemyImages[num];
            foreach (CharecterImages item in sprts.charecterImages)
            {
                if (item.faceType == GameManager.FaceExpressionsTypes.idle)
                {
                    idleSprites = item.Images;
                    idlefps = item.fps;
                }
                if (item.faceType == GameManager.FaceExpressionsTypes.sad)
                {
                    happySprites = item.Images;
                    happyfps = item.fps;
                }
                if (item.faceType == GameManager.FaceExpressionsTypes.happy)
                {
                    sadSprites = item.Images;
                    sadfps = item.fps;
                }
            }
        }
    }


    IEnumerator StartAnimation(Sprite[] images,float fps)
    {
        //yield return new WaitForSeconds(delay);
        if(!isPlayer)
        yield return new WaitForSeconds(0.5f);
        foreach (Sprite img in images)
        {
            imageTobeChanged.sprite = img;
            yield return new WaitForSeconds(1 / fps);
        }

        yield return new WaitForSeconds(Random.Range(2, 7));
        //StartCoroutine(StartAnimation(images,fps));
    }

    void StartAnimation()
    {
        if(!isPlayer)
        {
            int index = Random.Range(0, 3);
            if(index == 0)
            {
                GameManager_OnIdleAnimation();
            }
            else if(index ==1)
            {
                GameManager_OnFailure();
            }
            else
            {
                GameManager_OnCloseOne();
            }

        }
    }

    IEnumerator StartIdleAnimation(float timedif = 0)
    {
        yield return new WaitForSeconds(timedif);
        float delay = Random.Range(1f, 2);
        yield return new WaitForSeconds(delay);
        StartCoroutine(StartAnimation(idleSprites, idlefps));
    }

    void GameManager_OnIdleAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(StartAnimation(idleSprites, idlefps));
       // StopCoroutine(StartIdleAnimation());
        StartCoroutine(StartIdleAnimation());
    }

    void GameManager_OnFailure()
    {
        StopAllCoroutines();
        StartCoroutine(StartAnimation(sadSprites, sadfps));
       // StopCoroutine(StartIdleAnimation());
       // StartCoroutine(StartIdleAnimation());
    }

    void GameManager_OnCloseOne()
    {
        StopAllCoroutines();
        StartCoroutine(StartAnimation(happySprites, happyfps));
       // StopCoroutine(StartIdleAnimation());
        StartCoroutine(StartIdleAnimation());
    }

    public void Continue()
    {
        GameManager_OnIdleAnimation();
    }
}
