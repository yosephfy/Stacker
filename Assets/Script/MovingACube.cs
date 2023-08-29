using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingACube : MonoBehaviour
{
    public static MovingACube CurrentCubeInst { get; private set; }
    public static MovingACube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }

    public int Level = -1;

    public void SetLevel(int lvl)
    {
        Level = lvl;
    }

    private void OnEnable()
    {
        if (LastCube == null)
        {
            LastCube = GameObject.Find("Start").GetComponent<MovingACube>();
        }

        CurrentCubeInst = this;
        //GetComponent<Renderer>().material.color = GetCubeColor();

        Level = ScoreText.Instance.GetScore();

        Debug.Log("IT IS " + Level);
        CurrentCubeInst.GetComponent<Renderer>().material.SetColor("_Color", Color.HSVToRGB((Level / 50f) % 1f, 1f, 1f));
        //Camera.main.backgroundColor = Color.HSVToRGB(((Level - 2) / 100f) % 1f, 0.5f, 0.5f);

        if (LastCube.gameObject != GameObject.Find("Start").gameObject)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, LastCube.transform.position.y + 5f, Camera.main.transform.position.z);
        else
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, LastCube.transform.position.y + 5f + 0.55f, Camera.main.transform.position.z);

        //}

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);

        //
        gameOver_ = false;
    }

    private Color GetCubeColor()
    {
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
    }

    [SerializeField]
    private float moveSpeed = 1f;

    internal void Stop()
    {
        StartCoroutine(fadeOut(GameObject.Find("Cube"), 1f));
        moveSpeed = 0;

        float hangover = GetHangover();

        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if (Math.Abs(hangover) > max)
        {
            EndGame();
        }

        if (Math.Abs(hangover) < 0.05f)
        {
            //Debug.Log("Hooray");
            hangover = 0.0f;
            transform.position = new Vector3(LastCube.transform.position.x, transform.position.y, LastCube.transform.position.z);
            ScoreText.Instance.SetGrowNum(1);
            if (ScoreText.Instance.GetScore() > 0)
            {
                CubeEffect();
                AudioPlayer.Instance.PlayAudioPowerUp(9 - ScoreText.Instance.GetGrowNum());
            }
            Debug.Log("grow " + ScoreText.Instance.GetGrowNum());

            if (ScoreText.Instance.GetGrowNum() == 0)
            {
                if (MoveDirection == MoveDirection.X)
                {
                    transform.localScale += new Vector3(0.05f, 0, 0);
                    transform.position += new Vector3(0.025f, 0, 0);
                }
                else
                {
                    transform.localScale += new Vector3(0, 0, 0.05f);
                    transform.position += new Vector3(0, 0, 0.025f);
                }

                ScoreText.Instance.SetGrowNum(0);
            }
        }
        else
        {
            ScoreText.Instance.SetGrowNum(0);
            AudioPlayer.Instance.PlayDefaultAudio();
        }

        float direction = hangover > 0 ? 1f : -1f;
        if (hangover != 0)
            if (MoveDirection == MoveDirection.Z)
                SlpitCubesOnZ(hangover, direction);
            else
                SlpitCubesOnX(hangover, direction);

        LastCube = this;
    }

    public bool gameOver_;

    private void EndGame()
    {
        LastCube = null;
        CurrentCubeInst = null;
        ScoreText.Instance.SetGrowNum(0);
        //SceneManager.LoadScene(0);

        Destroy(this.gameObject, 0);
        gameOver_ = true;

        //GameObject start = GameObject.Find("Start");
        //Vector3 LookingPos = Vector3.Lerp(new Vector3(start.transform.position.x, transform.position.y, start.transform.position.z), start.transform.position, 0.5f);
        //
        //Camera.main.transform.LookAt(LookingPos);
    }

    private float GetHangover()
    {
        if (MoveDirection == MoveDirection.Z)
            return transform.position.z - LastCube.transform.position.z;
        else
            return transform.position.x - LastCube.transform.position.x;
    }

    private void SlpitCubesOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Math.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPos = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPos);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockPos = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockPos, fallingBlockSize);
    }

    private void SlpitCubesOnX(float hangover, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Math.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXSize;

        float newXPos = LastCube.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockPos = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockPos, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockPos, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPos);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockPos, transform.position.y, transform.position.z);
        }

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;

        Destroy(cube.gameObject, 1);
    }

    private float countDown = 1.58f;

    private void Update()
    {
        if (gameOver_ == false)
        {
            MoveBlockBackAndForth();
            Camera.main.backgroundColor = Color.HSVToRGB(Mathf.Repeat(Time.time / 200f, 1f), 0.5f, 0.5f);
        }
    }

    private void MoveBlockBackAndForth()
    {
        if (countDown <= -1.58f) countDown = 1.58f;

        if (countDown > 0.0f)
        {
            if (MoveDirection == MoveDirection.Z)
                transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
            else
                transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            //EndGame();
            countDown -= Time.deltaTime;
        }
        else if (countDown <= 0.0f)
        {
            if (MoveDirection == MoveDirection.Z)
                transform.position += Vector3.back * Time.deltaTime * moveSpeed;
            else
                transform.position += Vector3.left * Time.deltaTime * moveSpeed;

            countDown -= Time.deltaTime;
        }
    }

    public void FadeOut(GameObject obj)
    {
        if (obj != null)
        {
            obj.GetComponent<Renderer>().material.SetColor("_Color", new Color(255, 255, 255, Mathf.Lerp(255, 0, Mathf.Repeat(Time.deltaTime * 255f, 255f))));
        }
    }

    private IEnumerator fadeOut(GameObject MyRenderer, float duration)
    {
        if (MyRenderer != null)
        {
            float counter = 0;
            //Get current color
            Color spriteColor = MyRenderer.GetComponent<Renderer>().material.color;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                //Fade from 1 to 0
                float alpha = Mathf.Lerp(1, 0, counter / duration);
                Debug.Log(alpha);

                //Change alpha only
                if (MyRenderer != null)
                {
                    MyRenderer.GetComponent<Renderer>().material.SetColor("_Color", new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha));
                    MyRenderer.transform.localScale += new Vector3(1, 1, 0.1f) * Time.deltaTime / 10f;
                }

                //Wait for a frame
                yield return null;
            }
        }
    }

    public GameObject effect;

    private void CubeEffect()
    {
        var the_effect = Instantiate(effect);
        the_effect.transform.position = transform.position;
        the_effect.transform.localScale = new Vector3(transform.localScale.x / 4f, transform.localScale.z / 4f, transform.localScale.y / 4f) + new Vector3(0.02f, 0.02f, 0.02f);
        StartCoroutine(fadeOut(the_effect, 1.5f));
        Destroy(the_effect, 2f);
    }
}