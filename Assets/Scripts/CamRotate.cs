using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //회전속도
    public float rotSpeed = 200.0f;
    
    //회전값 처리 변수
    private float mx = 0;
    private float my = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //게임 상태가 '게임중'일떄만 조작 가능
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        
        //마우스 입력처리
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //마우스입력값만큼 회전값 누적
        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime;
        //클램프
        my = Mathf.Clamp(my, -90f, 90f);
        //회전
        transform.eulerAngles = new Vector3(-my, mx, 0f);
        
    }
}
