using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIPop : MonoBehaviour 
{
    public static UIPop instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnSmash()
	{
        GetComponent<SpriteRenderer>().DOFade(1, 0);
        transform.DOLocalMoveY(-400, 0);
        transform.DOLocalMoveY(0, 1f);
        GetComponent<SpriteRenderer>().DOFade(0, 1f);
    }

    public void OnScore(string score)
    {
        GetComponent<Text>().DOFade(1, 0);
        transform.DOLocalMoveY(0, 0);
        GetComponent<Text>().text = score;
        transform.DOLocalMoveY(200, 1f);
        GetComponent<Text>().DOFade(0, 1f);
    }

    public void OnTextSmash()
    {
        GetComponent<Text>().DOFade(1, 0);
        transform.DOLocalMoveY(-400, 0);
        GetComponent<Text>().text = "Smash !!";
        transform.DOLocalMoveY(0, 1f);
        GetComponent<Text>().DOFade(0, 1f);
    }

    public void Continue()
    {
        GetComponent<Text>().DOFade(0, 0.01f);
    }

    public void ShowText(string textToShow)
    {
        //print(textToShow);
        GetComponent<Text>().DOFade(1, 0);
        transform.DOLocalMoveY(-400, 0);
        GetComponent<Text>().text = textToShow;
        transform.DOLocalMoveY(0, 1f);
        GetComponent<Text>().DOFade(0, 1f);
    }
}
