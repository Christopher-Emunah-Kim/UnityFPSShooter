using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //캐릭터 컨트롤러 인스턴스
    private CharacterController cc;
    
    //속도
    public float moveSpeed = 7f;
    
    //점프 관련 변수
    private float gravity = -15f;
    public float yVelocity = 0f;
    public float jumpPower = 3f;
    private bool isGrounded;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //캐릭터 컨트롤러 받기
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //wasd 입력 처리
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        
        //메인카메라 기준으로 벡터전환
        dir = Camera.main.transform.TransformDirection(dir);
        transform.position += dir * (moveSpeed * Time.deltaTime);
        
        
        //점프 입력 처리(스페이스바)
        isGrounded = cc.isGrounded;
        //바닥에 닿았고
        if (isGrounded)
        {
            //점프중이었던 상태면
            if (yVelocity < 0)
            {
                yVelocity = -1f; //수직속도 0으로 초기화
            }
            
            //키보드 입력과 점프가능상태라면
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
        }
        else
        {
            yVelocity += gravity * Time.deltaTime; //공중에선 중력 적용
        }
        
        dir.y = yVelocity;
        //이동속도에 맞춰 컨트롤러로 이동
        cc.Move(dir * (moveSpeed * Time.deltaTime));
        
        
        
    }
}
