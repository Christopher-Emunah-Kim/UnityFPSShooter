using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
    {
        //폭발 이펙트 프리팹
        public GameObject bombEffect;
        
        //수류탄 데미지
        public int attackPower = 10;
        
        //폭발효과 반경
        public float explosionRadius = 5f;
        
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
        //폭발효과 내 'enemy'레이어에 속한 오브젝트들의 collider들을 배열에 저장
        //1<<10은 첫번쨰 비트부터 'enemy'라는 이름을 가진 레이어 번호만큼 좌측으로 이동했을때 선택된 비트
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);
        //col 배열에 있는 모든 에너미에 수류탄 데미지 적용
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitDamage(attackPower);
        }
        
        GameObject eff = Instantiate(bombEffect); //프리팹 생성
        eff.transform.position = transform.position; //위치 설정
        Destroy(gameObject);  //자기자신 제거
    }
}
