using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using TMPro;


public class Player : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] Material cubeMaterial;
    float speed = 2f;




    // Update is called once per frame
    void Update()
    {
        Swipe();
        // PlayerMovement();
    }

    float horizontal;
    public void PlayerMovement()
    {

        player.transform.position += player.transform.right * horizontal * speed * Time.deltaTime * 2f;

    }

    public Vector2 startPos, currentPos, endPos, dis;
    bool tap, swipeLeft, swipeRight;
    bool isDragging = false;
    void Swipe()
    {
        tap = swipeLeft = swipeRight = false;

        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDragging = true;
            startPos = (Vector2)Input.mousePosition;

        }else if (Input.GetMouseButtonUp(0))
        {
            tap = false;
            isDragging = false;
            startPos = Vector2.zero;
        }
        if (Input.touches.Length > 0 )
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDragging = true;
                startPos = Input.touches[0].position;
            }

        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            isDragging = false;

        }
        dis = Vector2.zero;
        if (isDragging)
        {
            if (Input.touches.Length > 0)
                dis = Input.touches[0].position - startPos;
            else if (Input.GetMouseButton(0))
                dis = (Vector2)Input.mousePosition - startPos;

        }
        if (dis.magnitude > 125)
        {
            print(" here ");
            if (dis.x < 0)
            {
                swipeLeft = true;
                player.transform.position -= player.transform.right* speed * Time.deltaTime * 2f;

            }
            else
            {
                swipeRight = true;
                player.transform.position += player.transform.right* speed * Time.deltaTime * 2f;
            }
        }

    }
    //for player score
    int jackpot = 10;
    int jackpot1 = -10;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "HoboFinish":
                PlayerManager.instance.winPanel.SetActive(true);
                Time.timeScale = 0f;
                StopAllCoroutines();

                break;
            case "poorFinish":
                PlayerManager.instance.winPanel.SetActive(true);
                Time.timeScale = 0f;
                StopAllCoroutines();
                break;
            case "decentFinish":
                PlayerManager.instance.winPanel.SetActive(true);
                Time.timeScale = 0f;
                StopAllCoroutines();
                break;
            case "richFinish":
                PlayerManager.instance.winPanel.SetActive(true);
                Time.timeScale = 0f;
                StopAllCoroutines();
                break;
            case "millionFinish":
                PlayerManager.instance.winPanel.SetActive(true);
                Time.timeScale = 0f;
                StopAllCoroutines();
                break;
            case "Coin":
                PlayerManager.instance.OverallScore += 20;
                PlayerManager.instance.ScorePerObj += 20;
                Destroy(other.gameObject);
                break;
            case "Bottles":
                PlayerManager.instance.OverallScore -= 40;
                PlayerManager.instance.ScorePerObj -= 40;
                Destroy(other.gameObject);
                break;
            case "Vaccine":
                PlayerManager.instance.OverallScore += 20;
                PlayerManager.instance.ScorePerObj += 20;
                Destroy(other.gameObject);
                break;
            case "Covid":
                PlayerManager.instance.ScorePerObj -= 40;
                PlayerManager.instance.OverallScore -= 40;
                Destroy(other.gameObject);
                break;
            case "Jackpot":
                PlayerManager.instance.ScorePerObj -= Random.Range(jackpot1, jackpot);
                PlayerManager.instance.OverallScore -= Random.Range(jackpot1, jackpot);
                break;

            default:
                break;
        }


        PlayerManager.instance.Lose();

    }


}
public enum PlayerState
{
    hobo,
    poor,
    decent,
    rich,
    millionare
}