using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpawner : MonoBehaviour {
	public GameObject Puzzle;
	public float TimeGap;
    public bool isAutoSpawn;
    public int direction;
	// Use this for initialization
	void Start () {
        if (isAutoSpawn)
            Spawn();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Spawn()
	{
        GameObject go = Instantiate(Puzzle);
        go.transform.position = transform.position;
        go.GetComponent<Puzzle>().Init();
        go.GetComponent<Puzzle>().MoveToPlayer(direction);
        if (isAutoSpawn)
            Invoke("Spawn", TimeGap);
	}
}
