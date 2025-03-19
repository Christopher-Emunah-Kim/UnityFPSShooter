using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //게임상태 Enum
    public enum GameState
    {
        Ready,
        Run,
        Pause,
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
    //옵션화면 UI오브젝트 변수
    public GameObject gameOption;
    
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
            //플레이어 애니메이션 정지
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            //상태 텍스트 활성화
            gameLabel.SetActive(true);
            gameText.text = "Game Over!";
            gameText.color = Color.red;
            
            //게임종료UI 호출
            Transform buttons = gameText.transform.GetChild(0); //상태 텍스트의 자식오브젝트의 트랜스폼 데이터 가져오기
            buttons.gameObject.SetActive(true); //버튼 활성화
            
            //상태를 게임오버로 변경
            gState = GameState.GameOver;
        }
    }
    
    //옵션화면켜기
    public void OpenOptionWindow()
    {
        gameOption.SetActive(true);//활성화
        Time.timeScale = 0f; //게임속도 0으로 
        gState = GameState.Pause; //게임상태 변경
    }
    
    //계속하기 옵션
    public void CloseOptionWindow()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1f;
        gState = GameState.Run;
    }
    
    //다시하기 옵션
    public void RestartGame()
    {
        Time.timeScale = 1f;
        //현재씬 다시 로드
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //로딩씬 진입
        SceneManager.LoadScene(1); 
    }
    
    //종료하기 옵션
    public void QuitGame()
    {
        Application.Quit();
    }
}
