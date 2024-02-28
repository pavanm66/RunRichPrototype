using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] playerArray;
    //public int playerIndex;
    public GameObject[] destination;
    public Transform followCam;
    public static PlayerManager instance;
    [SerializeField] NavMeshAgent playerNav;
    public int currentPlayer;
    public Transform currentActiveTrans;
    [SerializeField] Text levelText;
    public GameObject winPanel, losePanel;
    [SerializeField] GameObject floatingTextObj;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentPlayer = (int)PlayerState.poor;
        playerState = PlayerState.poor;
        if (waypointArray[0])
            currentDest = waypointArray[0];
        else
            UpdateFinishPoint(currentPlayer);
        ActivePlayer(currentPlayer);
        OverallScore = 40;
    }

    public void ActivePlayer(int activePlayer)
    {

        for (int i = 0; i < playerArray.Length; i++)
        {
            if (i == activePlayer)
            {
                playerArray[i].SetActive(true);
                currentActiveTrans = playerArray[i].transform;
                if (currentDest)
                    playerNav.SetDestination(currentDest.position);
                else
                    playerNav.SetDestination(destination[activeDestination].transform.position);
            }
            else
            {
                playerArray[i].SetActive(false);
            }

        }
        CheckPlayerState(playerState);
    }
    Transform currentDest;
    private void Update()
    {
        if (currentDest)
            if (Vector3.Distance(currentDest.position, currentActiveTrans.position) <= 0.5f)
            {
                WayPointSwitch();
            }
    }
    private int overallScore;
    public int OverallScore
    {
        get
        {
            return overallScore;
        }
        set
        {
            overallScore = value;
            levelText.text = " Score : " + overallScore;
            ChangePlayerState();
        }
    }
    private int scorePerObj;
    public int ScorePerObj
    {
        get
        {
            return scorePerObj;
        }
        set
        {
            scorePerObj = value;
            FloatingText();
        }
    }

    //to instantiate and update text
    GameObject textPopup;
    public void FloatingText()
    {
        textPopup = Instantiate(floatingTextObj, playerArray[currentPlayer].transform.position, Quaternion.identity);
        StartCoroutine(DestroyTextPopups());
    }


    IEnumerator DestroyTextPopups()
    {
        while (textPopup.activeSelf)
        {
            textPopup.GetComponentInChildren<TextMesh>().text = ScorePerObj.ToString();

            yield return new WaitForSeconds(0.5f);
            ScorePerObj = 0;
            textPopup.SetActive(false);
            StopCoroutine(DestroyTextPopups());
        }

    }


    void ChangePlayerState()
    {
        if (OverallScore >= 40 && OverallScore < 80)
        {
            playerState = PlayerState.poor;
        }
        else if (OverallScore < 40 && OverallScore > 0)
        {
            playerState = PlayerState.hobo;
        }
        else if (OverallScore >= 80 && OverallScore < 120)
        {
            playerState = PlayerState.decent;
        }
        else if (OverallScore >= 120 && OverallScore < 140)
        {
            playerState = PlayerState.rich;
        }
        else if (OverallScore >= 140)
        {
            playerState = PlayerState.millionare;
        }
        CheckPlayerState(playerState);
        ActivePlayer((int)playerState);
        UpdateFinishPoint((int)playerState);
    }

    //for losing
    public void Lose()
    {
        if (OverallScore <= 0)
        {
            Time.timeScale = 0f;
            losePanel.SetActive(true);
        }
    }
    //for player states
    PlayerState playerState;
    int activeDestination;
    public void CheckPlayerState(PlayerState state)
    {
        activeDestination = 0;
        playerState = state;
        switch (playerState)
        {
            case PlayerState.hobo:
                //  playerArray[activeDestination].cubeMaterial.color = Color.black;
                activeDestination = 0;
                break;
            case PlayerState.poor:
                //   cubeMaterial.color = Color.white;
                activeDestination = 1;
                break;
            case PlayerState.decent:
                //  cubeMaterial.color = Color.red;
                activeDestination = 2;
                break;
            case PlayerState.rich:
                // cubeMaterial.color = Color.blue;
                activeDestination = 3;
                break;
            case PlayerState.millionare:
                //  cubeMaterial.color = Color.cyan;
                activeDestination = 4;
                break;
            default:
                playerState = PlayerState.poor;
                //  cubeMaterial.color = Color.white;
                break;
        }

        //playerIndex = activeDestination;
        // ActivePlayer();
    }
    public void UpdateFinishPoint(int index)
    {
        for (int i = 0; i < destination.Length; i++)
        {
            if (i == index)
            {
                destination[i].SetActive(true);
                playerNav.SetDestination(destination[index].transform.position);

                playerNav.Move(Vector3.forward * Time.deltaTime);

            }
            else
            {
                destination[i].SetActive(false);
            }
        }

    }

    [SerializeField] Transform[] waypointArray;
    public void WayPointSwitch()
    {
        int i = Array.IndexOf(waypointArray, currentDest);
        if (i >= waypointArray.Length - 1)
        {
            UpdateFinishPoint(activeDestination);
            return;
        }
        i++;
        currentDest = waypointArray[i];
        playerNav.SetDestination(currentDest.position);

    }
    public void LoadNextScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Level" + index);
    }
    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }
    public void Replay()
    {
        int scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
    }
}
