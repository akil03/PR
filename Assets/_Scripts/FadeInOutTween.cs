using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class FadeInOutTween : MonoBehaviour
{
    
    public bool isText;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(Fade());
    }


    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(3f);
        if (isText)
            GetComponent<Text>().DOFade(0, 0.5f).SetUpdate(true).OnComplete(() => {
            StartCoroutine(Show());
            });
        else
            GetComponent<Image>().DOFade(0, 0.5f).SetUpdate(true).OnComplete(() => {
            StartCoroutine(Show());
            });
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(0.2f);
        if (isText)
            GetComponent<Text>().DOFade(1, 0.5f).SetUpdate(true).OnComplete(() => {
            StartCoroutine(Fade());
            });
        else
            GetComponent<Image>().DOFade(1, 0.5f).SetUpdate(true).OnComplete(() => {
            StartCoroutine(Fade());
            });
    }
}