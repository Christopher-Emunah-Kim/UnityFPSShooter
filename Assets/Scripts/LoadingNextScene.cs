using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingNextScene : MonoBehaviour
{
    //씬을 비동기 방식으로 로드
    //현재 씬에는 로딩 진행률 표현
    
    //진행 씬 번호
    public int sceneNumber = 2;
    //슬라이더 바
    public Slider loadingBar;
    //로딩 텍스트
    public Text loadingText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //비동기 씬로드 코루틴 호출
        StartCoroutine(TransitionNextScene(sceneNumber));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //비동기 씬로드 코루틴
    IEnumerator TransitionNextScene(int num)
    {
        //지정된 씬을 비동기 형식으로 로드
        AsyncOperation ao = SceneManager.LoadSceneAsync(num);
        
        //로드되는 씬의 모습이 화면에 보이지 않게
        if (ao != null)
        {
            ao.allowSceneActivation = false;
            //로딩완료될때까지 반복해서 씬의 요소를 로드하고, 진행률을 표시
            while (!ao.isDone)
            {
                loadingBar.value = ao.progress;
                loadingText.text = (ao.progress * 100f) + "%";
                
                //90%가 넘으면
                if (ao.progress >= 0.9f)
                {
                    //로드된 씬을 화면에 보이게
                    ao.allowSceneActivation = true;
                }
                
                //다음 프레임까지 기다림
                yield return null;
            }
        }
    }
}
