using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField id;
    public TMP_InputField pw;
    
    //검사 텍스트 변수
    public TMP_Text notify;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //검사텍스트창 비우기
        notify.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //아이디, 패스워드 저장 함수
    public void SaveUserData()
    {
        //공백체크에 문제가 있다면 아무것도 안함
        if(!CheckInput(id.text, pw.text))
        {
            return;
        }
        //중복 아이디가 없다면
        if(false == PlayerPrefs.HasKey(id.text))
        {
            //사용자 id, pw를 key-value로 저장
            PlayerPrefs.SetString(id.text, pw.text);
            notify.text = "ID Create Complete";
        }
        else
        {
            notify.text = "Existed ID";
        }
    }
    
    //로그인시 유저 데이터 체크 함수
    public void CheckUserData()
    {
        //공백체크에 문제가 있다면 아무것도 안함
        if(!CheckInput(id.text, pw.text))
        {
            return;
        }
        //사용자가 입력한 아이디 키로 값 불러오기
        string pass = PlayerPrefs.GetString(id.text);
        //사용자의 입력 패스워드와 불러온값의 value를 비교
        if (pw.text == pass)
        {
            //다음번 씬 로드
            SceneManager.LoadScene(1);
        }
        else
        {
            notify.text = "ID and password you entered do not match";
        }
    }
    
    //공백체크함수
    bool CheckInput(string id, string pw)
    {
        //아이디랑 패스워드중 하나라도 비어있으면 입력요구
        if (id == "" || pw == "")
        {
            notify.text = "please enter the id and password";
            return false;
        }
        else
        {
            return true;
        }
    }
}
