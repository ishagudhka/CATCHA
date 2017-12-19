using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    //메뉴 디스플레이 텍스트 관련
    private Text languageText;
    private Text roundText;
    private int selectedLanguageNum;
    private int selectedRoundNum;

    //라운드 선택 시 액티비티 관련 함수
    private GameObject selectActivity;

    // Use this for initialization
    void Start()
    {
        //메뉴 디스플레이 내의 언어, 라운드 텍스트 초기화
        Text[] texts = ((GameObject)Resources.Load("MenuDisplay/MenuDisplay")).GetComponentsInChildren<Text>();
        roundText = texts[1];
        languageText = texts[2];

        selectActivity = GameObject.Find("SelectActivityPanel");
        if(selectActivity) selectActivity.SetActive(false);

    }
   
    //씬 전환 관련함수들
    public void StartGame() {
       
        SceneManager.LoadScene("Language");
    }

    public void CharacterIconReset()
    {
        int characterNum = PlayerPrefs.GetInt("characterNum");

        //다른 화면에서도 보여지도록 프리팹의 아이콘 교체
        ((GameObject)Resources.Load("MenuDisplay/MenuDisplay")).GetComponentsInChildren<RawImage>()[1].texture
            = Resources.Load<Texture>("Characters/cat0" + characterNum + "_icon");
    }

    public void GoToMain() {
        SceneManager.LoadScene("Main");
    }

    public void GoToMap()
    {
      
        SceneManager.LoadScene("Map");
    }

    public void GoToStudy()
    {
       
        SceneManager.LoadScene("Study");
    }


    public void GoToQuiz()
    {
       
        SceneManager.LoadScene("Quiz");
    }

    public void BackToPrevious() {

        string sceneName = PlayerPrefs.GetString("previousGameScene");
        SceneManager.LoadScene(sceneName);
    }


    //캐릭터 선택
    public void ChooseCharacter() {

        //현재 있는 씬 이름 호출
        string sceneName = SceneManager.GetActiveScene().name;

        //돌아갈 화면 이름 저장 및 씬 전환
        PlayerPrefs.SetString("previousGameScene", sceneName);
        SceneManager.LoadScene("Character");
    }

    //언어 선택 함수
    public void ChooseLanguage(int languageNum) {

        //선택한 언어 저장
        PlayerPrefs.SetInt("languageNum", languageNum);

        //언어 번호에 맞는 언어 텍스트로 교체
        switch (languageNum)
        {
            case 0: languageText.text = "한국어"; break;
            case 1: languageText.text = "English"; break;
        }

    }

  
    //라운드 선택 관련 함수
    public void SelectRound(int roundNum) {

        Debug.Log("SelectRound " + roundNum+" clicked");

        //언어번호 저장
        selectedLanguageNum = PlayerPrefs.GetInt("languageNum");

        //라운드 넘버 저장 
        PlayerPrefs.SetInt("roundNum", roundNum);

        string[] roundKorean = { "동물", "색깔", "과일", "자연", "탈 것", "음악", "가족", "운동", "내 방" };
        string[] roundEnglish = { "Animal", "Colour", "Fruits", "Nature", "Vehicle", "Music" , "Family", "Sports", "My Room"};

        //메뉴 디스플레이의 프리팹 언어 텍스트

        switch (selectedLanguageNum)
        { case 0: roundText.text = roundKorean[roundNum - 1]; break;
          case 1: roundText.text = roundEnglish[roundNum - 1]; break;
        }

        if(selectActivity) selectActivity.SetActive(true);

    }

  


    //퀴즈 내에서 언어바꾸기 관련(언어 선택 디스플레이 활성화)
    public void ShowDisplay(GameObject display) {
        display.SetActive(true);
    }

    //퀴즈 내에서 언어바꾸기 관련(언어 선택 디스플레이 비활성화)
    public void HideDisplay(GameObject display)
    {
        display.SetActive(false);
    }
    
    //퀴즈 내의 언어 바꾸기 함수
    public void ChangeLanguage(GameObject display) {

        selectedLanguageNum = PlayerPrefs.GetInt("languageNum");
        int roundNum = PlayerPrefs.GetInt("roundNum");

        switch (selectedLanguageNum) {
            case 0: selectedLanguageNum = 1; break;
            case 1: selectedLanguageNum = 0; break;
        }
    
        ChooseLanguage(selectedLanguageNum);
        SelectRound(roundNum);

        GoToStudy();

    }

    


	
}
