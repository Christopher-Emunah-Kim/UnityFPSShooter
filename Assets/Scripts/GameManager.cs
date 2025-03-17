using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //게임상태 Enum
    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }
    
    //싱글턴 객체
    public static GameManager gm;
    
    //PlayerMove클래스 변수
    private PlayerMove player;

    //현재게임상태
    public GameState gState;
    
    //UI
    //UI오브젝트 변수
    public GameObject gameLabel;
    //텍스트 컴포넌트
    private Text gameText;
    
    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        //초기에는 준비상태
        gState = GameState.Ready;
        //플레이어 오브젝트를 찾은 후 PlayerMove 컴포넌트 받기
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        //게임상태 UI오브젝트에서 Text 컴포넌트를 불러옴
        gameText = gameLabel.GetComponent<Text>();
        //텍스트 내용을 '준비'로 함.
        gameText.text = "Ready...";
        //텍스트는 녹색으로
        gameText.color = Color.green;
        //게임 준비 -> 게임중 상태로 전환하는 코루틴함수 호출
        StartCoroutine(ReadyToStart());
    }
    
    //게임상태 변경 코루틴 함수
    IEnumerator ReadyToStart()
    {
        //2초 대기
        yield return new WaitForSeconds(2f);
        //텍스트 변경
        gameText.text = "Go!";
        //0.5초 대기
        yield return new WaitForSeconds(0.5f);
        //텍스트 비활성화
        gameLabel.SetActive(false);
        //상태를 게임중 상태로 변경
        gState = GameState.Run;

    }

    // Update is called once per frame
    void Update()
    {
        if (player.hp <= 0)
        {
            //상태 텍스트 활성화
            gameLabel.SetActive(true);
            gameText.text = "Game Over!";
            gameText.color = Color.red;
            //상태를 게임오버로 변경
            gState = GameState.GameOver;
        }
    }
}
