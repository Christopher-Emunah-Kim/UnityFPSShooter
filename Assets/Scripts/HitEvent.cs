using UnityEngine;

public class HitEvent : MonoBehaviour
{
    
    //EnemyFSM변수
    public EnemyFSM efsm;
    
    //데미지 이벤트 함수
    public void PlayerHit()
    {
        efsm.AttackAction();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
