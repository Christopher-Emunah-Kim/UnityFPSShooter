using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    //폭발 이펙트 프리팹
    public GameObject bombEffect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        GameObject eff = Instantiate(bombEffect); //프리팹 생성
        eff.transform.position = transform.position; //위치 설정
        Destroy(gameObject);  //자기자신 제거
    }
}
