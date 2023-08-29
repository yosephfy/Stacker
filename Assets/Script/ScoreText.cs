using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public int Score = 0;
    public int CubeGrowNum = 10;
    public static ScoreText Instance;

    public int GetGrowNum()
    {
        return CubeGrowNum;
    }

    public void SetGrowNum(int num)
    {
        if (num == 0)
            CubeGrowNum = 10;
        else
            CubeGrowNum--;
    }

    public int GetScore()
    {
        return Score;
    }

    public Camera mainCam;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;

        CubeGrowNum = 10;
    }

    // Start is called before the first frame update
    private void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        GameManager.OnCubeSpawned += GameManager_OnCubeSpawned;
    }

    private void OnDestroy()
    {
        GameManager.OnCubeSpawned -= GameManager_OnCubeSpawned;
    }

    private void GameManager_OnCubeSpawned()
    {
        Score++;
        text.text = Score.ToString();
        //float camPos = (float)Score / 200;
        //mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + camPos, mainCam.transform.position.z);
        //mainCam.transform.eulerAngles = new Vector3(mainCam.transform.eulerAngles.x - (camPos), mainCam.transform.eulerAngles.y, mainCam.transform.eulerAngles.z);
    }
}