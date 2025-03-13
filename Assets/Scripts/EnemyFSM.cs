using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class EnemyFSM : MonoBehaviour
{
    
    //에너미 상태 상수
    enum EnemyState
    {
        Idle = 1,
        Move = 2,
        Attack = 4,
        Return = 8,
        Damaged = 10,
        Die = 20
    }
    
    //에너미 상태 변수
    private EnemyState m_State;
    //캐릭터 컨트롤러 컴포넌트
    private CharacterController cc;
    
    //플레이어 발견 범위
    public float findDistance = 8f;
    //플레이어 위치
    private Transform player;
    //에너미 초기 위치
    private Vector3 originPos;
    //이동 가능 제한 범위
    public float moveDistance = 20f;
    //공격 범위
    public float attackDistance = 2f;
    //이동속도
    public float moveSpeed = 5f;
    //현재시간
    private float currentTime = 0f;
    //공격딜레이시간
    private float attackDelay = 2f;
    //에너미 공격력
    public int attackPower = 3;
    //에너미 HP
    public int hp = 15;
    //에너미 최대 HP
    public int maxHp = 15;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //최초 상태는 아이들
        m_State = EnemyState.Idle;
        
        //초기 위치 저장
        originPos = transform.position;
        
        //플레이어 위치 받기
        player = GameObject.Find("Player").transform;
        
        //캐릭터 컨트럴로 컴포넌트 받기
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //현재 상태 체크하여 상태 전환
        switch (m_State)
        {
            case EnemyState.Idle: Idle(); break;
            case EnemyState.Move: Move(); break;
            case EnemyState.Attack: Attack(); break;
            case EnemyState.Return: Return(); break;
            case EnemyState.Damaged: break;
            case EnemyState.Die: break;
        }
    }
    
    //대기상태함수
    void Idle()
    {
        //거리가 범위 이하라면 move로 전환
        if (Vector3.Distance(player.position, transform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Idle -> Move");
        }
    }
    
    //이동상태함수
    void Move()
    {
        //공격범위 밖이면 플레이어 향해 이동
        if (Vector3.Distance(player.position, transform.position) > attackDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            cc.Move(direction * (moveSpeed * Time.deltaTime));
        }
        //아니면 공격상태로 전환
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");
            //공격딜레이를 미리 없애놓기
            currentTime = attackDelay;
        }
    }
    
    //공격상태함수
    void Attack()
    {
        //현재 위치가 제한범위 밖이면
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            //복귀 상태 전환
            m_State = EnemyState.Return;
            print("상태 전환 : Move -> Return");
        }
        //공격범위 안이면 공격
        else if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            //일정 시간마다 플레이어 공격
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                print("공격");
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                currentTime = 0f;
            }
        }
        //그렇지 않으면 다시 이동상태 전환
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0f;
        }
    }
    
    //복귀상태함수
    void Return()
    {
        //초기위치에서의 거리가 0.1f이상이면 초기 위치로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * (moveSpeed * Time.deltaTime));
        }
        //아니면 자신의 위치를 초기 위치로 조정하고, 대기상태 전환
        else
        {
            transform.position = originPos;
            //hp회복
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태 전환 : Return->Idle");
        }
    }
    
    //데미지 실행함수
    public void HitDamage(int damage)
    {
        //만일 이미 피격/죽음/복귀 상태면 아무처리하지않음
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }
        
        //플레이어 공격력만큼 체력감소
        hp -= damage;
        //체력이 0보다 크면 피격상태 전환
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any State -> Damaged");
            Damaged();
        }
        //아니면 죽음상태 전환
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any State -> Die");
            Die();
        }
    }
    
    //피격상태함수
    void Damaged()
    {
        //코루틴 실행
        StartCoroutine(DamageProcess());
    }
    
    //데미지 처리함수
    IEnumerator DamageProcess()
    {
        //피격 모션 시간만큼 기다림
        yield return new WaitForSeconds(0.5f);
        
        //이동상태 전환
        m_State = EnemyState.Move;
        print("상태 전환 : Damaged -> Move");
    }
    
    //죽음상태함수
    void Die()
    {
        //진행중인 코루틴 중지
        StopAllCoroutines();
        
        //죽음 상태 처리 코루틴 함수 실행
        StartCoroutine(DieProcess());
    }

    //죽음상태처리 코루틴 함수
    IEnumerator DieProcess()
    {
        //컨트롤러 비활성화
        cc.enabled = false;
        
        //2초 후 자신 제거
        yield return new WaitForSeconds(2f);
        print("ByeBye");
        Destroy(gameObject);
    }
}
