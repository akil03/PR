using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PingPongScale : MonoBehaviour {

    public float fromValue;
    public float toValue;
    public float time;

	// Use this for initialization
	void Start () {
        Animation();
	}
	
    void Animation()
    {
        transform.DOScale(toValue, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOScale(fromValue, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                Animation();
            });
        });
    }
}
