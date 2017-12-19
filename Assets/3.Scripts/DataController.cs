using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataController : MonoBehaviour {


    private string appDataFileName = "data.json";

    //전체 게임 데이터
    private GameData gameKorean;
    private GameData gameEnglish;


    //데이터를 얻기 위한 유저가 선택한 정보
    private int languageNum;
    private int roundNum;


    // Use this for initialization
    void Start()
    {

        LoadGameData();
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Main");

    }


    //게임데이터 로드
    private void LoadGameData()
    {
        //Application.streamingAssetsPath = C:/Users/Yoojin Ko/Documents/Unity/CATCHA/Assets/StreamingAssets
        string filePath = Path.Combine(Application.streamingAssetsPath, appDataFileName);
        
        if (File.Exists(filePath))
        {
            //지정한 경로에 있는 파일 읽어오기
            string dataAsJson = File.ReadAllText(filePath);

            //읽어온 제이슨 문자열을 ApplicationData형식으로 변환
            ApplicationData loadedAppData = JsonUtility.FromJson<ApplicationData>(dataAsJson);

            //읽어온 ApplicationData에서 각 언어 별 게임데이터 저장하기 
            gameKorean = loadedAppData.allGameData[0];
            gameEnglish = loadedAppData.allGameData[1];
           

        }

        else
        {
            Debug.LogError("Fail to load all data!");
        }
       
    }

   

    public RoundData GetCurrentRoundData() {

        GetUserInfo();

        switch (languageNum) {
            case 0: return gameKorean.allRounds[roundNum - 1];
            case 1: return gameEnglish.allRounds[roundNum - 1];
            default: return null;
        }

    }

    public List<AudioClip> GetCurrentRoundAudio() {

        GetUserInfo();

        //선택한 라운드에 맞는 데이터
        RoundData currentRoundData = GetCurrentRoundData();
        QuestionData[] questionPool = currentRoundData.allQuestions;
        List<AudioClip> allWordsSound = new List<AudioClip>();
        AudioClip wordSound;
        string audioFIlePath;


        for (int i = 0; i < questionPool.Length; i++){

            switch (languageNum){
                case 0: audioFIlePath = "Audio/Round1/" + questionPool[i].answerImageTarget + "_Korean"; break;
                case 1: audioFIlePath = "Audio/Round1/" + questionPool[i].answerImageTarget + "_English"; break;
                default:  audioFIlePath = ""; break;
            }

            wordSound = Resources.Load<AudioClip>(audioFIlePath);
            allWordsSound.Add(wordSound);

        };
        
        return allWordsSound;

    }

    public void GetUserInfo() {
        languageNum = PlayerPrefs.GetInt("languageNum");
        roundNum = PlayerPrefs.GetInt("roundNum");
    } 
	
	// Update is called once per frame
	void Update () {
		
	}
}
