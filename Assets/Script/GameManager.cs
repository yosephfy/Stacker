using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static event Action OnCubeSpawned = delegate { };

    private CubeSpawner[] spawners;
    private int spawnerIndex;
    private CubeSpawner CurrentSpawner;

    public GameObject RestartButton;
    public GameObject ClickerButton;
    public GameObject ScoreText;
    public GameObject HomeButtons;
    public GameObject GameOverButtons;

    public GameObject Blocks;

    public int lvl;

    private void Awake()
    {
        lvl = 0;

        spawners = FindObjectsOfType<CubeSpawner>();
        ScoreText.SetActive(true);
        ClickerButton.SetActive(true);
        HomeButtons.SetActive(true);
        GameOverButtons.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        //if (MovingACube.CurrentCubeInst != null)
        //    if (Input.GetMouseButtonDown(0) && MovingACube.CurrentCubeInst.gameOver_ == false)
        //    {
        //        //if (BeforeStart.Instance.IsStarted())
        //        //WhenClicked();
        //        //BeforeStarting();
        //    }

        if (MovingACube.CurrentCubeInst != null)
        {
            if (MovingACube.CurrentCubeInst.gameOver_ == false)
            {
                RestartButton.SetActive(false);
                ClickerButton.SetActive(true);
                GameOverButtons.SetActive(false);
            }
        }
        else
        {
            //debugRestart();
            GameObject start = GameObject.Find("Start");
            Vector3 LookingPos = Vector3.Lerp(new Vector3(start.transform.position.x, GameObject.FindObjectsOfType<MovingACube>()[0].transform.position.y, start.transform.position.z), start.transform.position, 0.1f);
            Camera.main.transform.LookAt(LookingPos);

            RestartButton.SetActive(true);
            //ScoreText.SetActive(false);
            ClickerButton.SetActive(false);
            GameOverButtons.SetActive(true);
        }
    }

    public void WhenClicked()
    {
        if (MovingACube.CurrentCubeInst.gameOver_ == false)
        {
            HomeButtons.SetActive(false);

            if (MovingACube.CurrentCubeInst != null)
                MovingACube.CurrentCubeInst.Stop();

            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            CurrentSpawner = spawners[spawnerIndex];

            CurrentSpawner.SpawnCube();
            OnCubeSpawned();

            //MovingACube.LastCube.SetLevel(lvl);
            lvl++;
        }
    }

    private bool started;

    private void BeforeStarting()
    {
        if (!started && Input.GetMouseButtonDown(0))
        {
            started = true;
        }
        else if (started && Input.GetMouseButtonDown(0))
        {
            WhenClicked();
        }
    }

    public void Restart()
    {
        Debug.Log("Yo restart");
        SceneManager.LoadScene(0);
    }
}