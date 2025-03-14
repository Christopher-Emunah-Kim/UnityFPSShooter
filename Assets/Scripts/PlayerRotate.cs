using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    //회전속도
    public float rotSpeed = 500.0f;
    
    //회전값 처리 변수
    private float mx = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 입력처리
        float mouseX = Input.GetAxis("Mouse X");
        //마우스입력값만큼 회전값 누적
        mx += mouseX * rotSpeed * Time.deltaTime;
        //회전
        transform.eulerAngles = new Vector3(0f, mx, 0f);
    }
}
