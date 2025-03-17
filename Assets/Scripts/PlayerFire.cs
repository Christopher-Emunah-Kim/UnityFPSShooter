using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    
    public GameObject firePosition; //발사위치
    public GameObject bombFactory; //수류탄오브젝트
    public GameObject bulletEffect; //피격이펙트
    
    public float throwPower = 15f; //던지는 힘
    public int weaponPower = 3;
    
    private ParticleSystem ps; //파티클시스템
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //파티클시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //게임 상태가 '게임중'일떄만 조작 가능
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        
        //마우스 우클릭 입력하면 바라보는 방향으로 수류탄 던지기
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;
            
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
            
        }
        
        //마우스 좌클릭 입력하면 레이캐스트로 총 발사
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000f, Color.red, 2f);
            
            //RaycastHit 구조체에 정보 저장
            RaycastHit hitinfo = new RaycastHit();
            LayerMask lm = ~0;
            //부딪쳤으면
            if (Physics.Raycast(ray, out hitinfo, 100000, lm))
            {
                Debug.Log(hitinfo.collider.gameObject.name);
                
                //만약 부딪친게 Enemy 레이어라면 데미지 함수 실행
                if (hitinfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //FSM가져와서 데미지 함수 실행
                    EnemyFSM eFSM = hitinfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitDamage(weaponPower);
                }
                //아니라면 피격이펙트 재생
                else
                {
                    if (bulletEffect == null)
                    {
                        Debug.LogError("No bullet effect");
                        return;
                    }
                    //피격이펙트를 레이가 부딪친 지점에.
                    bulletEffect.transform.position = hitinfo.point;
                
                    //피격이펙트의 방향을 부딪친 곳의 법선 벡터와 일치시킴
                    bulletEffect.transform.forward = hitinfo.normal;
                
                    if(ps == null)
                    {
                        Debug.LogError("No particle system");
                        return;
                    }
                    ps.Play();
                }
            }
            else
            {
                Debug.Log("No Hit");
            }
        }
    }
}
