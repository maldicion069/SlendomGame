﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CollectingPapers : MonoBehaviour {
    public int papers = 0;
    public int papersToWin = 8;
    public float paperDistance = 2.5f;
    public AudioClip paperPickup;
    public EnemyScript enemy;

	// Use this for initialization
	void Start () {
        if (enemy == null)
        {
            enemy = GameObject.Find("Enemy").GetComponent<EnemyScript>();
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Paper");
        if (gos.Length > papersToWin)
        {
            for (int i = 0; i < gos.Length; ++i)
            {
                gos[i].SetActive(false);
            }
            HashSet<int> setAux = new HashSet<int>();
            while (setAux.Count != papersToWin)
            {
                int idx = Random.Range(0, gos.Length);
                if (!setAux.Contains(idx))
                {
                    setAux.Add(idx);
                }
            }
            int[] indices = new int[papersToWin];
            setAux.CopyTo(indices);
            for (int i = 0; i < indices.Length; ++i)
            {
                gos[i].SetActive(true);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) )
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f));

            if (Physics.Raycast(ray, out hit, paperDistance))
            {
                if (hit.collider.gameObject.tag == "Paper")
                {
                    papers++;
                    Debug.Log("A paper was picked up. Total papers = " + papers);
                    if (paperPickup)
                    {
                        AudioSource.PlayClipAtPoint(paperPickup, transform.position);
                    }
                    Destroy(hit.collider.gameObject);
                    enemy.ReduceDistance();
                }
            }
        }
    }
    public void OnGUI()
    {
        if (papers < papersToWin)
        {
            GUI.Box(new Rect((Screen.width * 0.5f) - 60, 10, 120, 25), "" + papers.ToString() + " Papers Collected");
        }
        else
        {
            GUI.Box(new Rect((Screen.width / 2) - 100, 10, 200, 35), "All Papers Collected!");
            SceneManager.LoadScene("sceneWin");
        }
    }
}

