using System.Collections;
using System.Net.Mime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    //무기모드 enum
    enum WeaponMode
    {
        Normal,
        Sniper
    }

    private WeaponMode wMode;
    
    public GameObject firePosition; //발사위치
    public GameObject bombFactory; //수류탄오브젝트
    public GameObject bulletEffect; //피격이펙트
    
    public float throwPower = 15f; //던지는 힘
    public int weaponPower = 3;
    
    //카메라 상태 체크 변수
    private bool ZoomMode = false;
    
    private ParticleSystem ps; //파티클시스템
    private Animator anim; //애니메이터 변수
    
    //총기효과 오브젝트 배열
    public GameObject[] eff_Flash;
    
    //UI
    public Text wModeText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //파티클시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();
        //애니메이터 컴포넌트 가져오기
        anim = GetComponentInChildren<Animator>();
        //무기 기본모드는 노멀
        wMode = WeaponMode.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        //게임 상태가 '게임중'일떄만 조작 가능
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        
        //키입력에 따른 무기모드 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;
            //카메라 화면 원래대로
            Camera.main.fieldOfView = 60f;
            wModeText.text = "Normal Mode"; //일반모드 텍스트 출력
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;
            wModeText.text = "Sniper Mode"; //스나이퍼모드 텍스트 출력
        }
        
        //노멀 : 마우스 우클릭 입력하면 바라보는 방향으로 수류탄 던지기
        //스나이퍼 : 마우스 우클릭 입력시 화면확대
        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                //기존 코드
                case WeaponMode.Normal:
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;
            
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
                //스나이퍼모드 추가
                case WeaponMode.Sniper:
                    //만일 줌모드가 아니면 화면확대해서 줌모드로 변경
                    if (!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;
                    }
                    //줌모드였으면 줌모드 해제
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                    }
                    break;
            }
        }
        
        //마우스 좌클릭 입력하면 레이캐스트로 총 발사
        if (Input.GetMouseButtonDown(0))
        {
            //이동블렌드 트리 파라미터 값이 0이면 공격 애니메이션 재생
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }
            
            //총 이펙트 작동
            StartCoroutine(ShootEffectOn(0.05f));
            
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


    //총기이펙트 코루틴 함수
    IEnumerator ShootEffectOn(float duration)
    {
        //랜덤으로 이펙트 활성화
        int num = Random.Range(0, eff_Flash.Length);
        eff_Flash[num].SetActive(true);
        //기다렸다가 비활성화
        yield return new WaitForSeconds(duration);
        eff_Flash[num].SetActive(false);
    }
}
