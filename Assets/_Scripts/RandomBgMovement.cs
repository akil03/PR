using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomBgMovement : MonoBehaviour {

    public float fromValueX, toValueX, fromValueY, toValueY,timeMin,timeMax;
    public Vector3 rotationDirection;

	// Use this for initialization
	void Start () {
        Invoke("Move", Random.Range(3, 15));
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationDirection);
	}

    void Move()
    {
        Vector2 NewPoint = new Vector2(-transform.localPosition.x, Random.Range(fromValueY, toValueY));
        transform.DOLocalMove(NewPoint, Random.Range(timeMin, timeMax)).OnComplete(() =>
        {
            Move();
        });
    }
}
