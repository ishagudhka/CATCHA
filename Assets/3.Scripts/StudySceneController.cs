using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StudySceneController : MonoBehaviour {

    //배경음악 관련 변수
    private AudioController audioController;

    //데이터 호출 관련 변수
    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;
    private List<AudioClip> allWordsSound = new List<AudioClip>();


    //데이터 호출할 인덱스 변수
    int thisCardIndex = 0;

    //그림단어
    private GameObject wordText;
    private GameObject character;

    //단어 오디오
    private AudioSource audioSource;
    private AudioClip wordSound;

    //단어를 보여주는 캐릭터 관련 변수
    private int characterNum;


    //단어 위치
    public Transform WordPositionFrom;
    public Transform WordPositionTo;

    public Transform CharacterOddPositionFrom;
    public Transform CharacterOddPositionTo;

    public Transform CharacterEvenPositionFrom;
    public Transform CharacterEvenPositionTo;

    // Use this for initialization
    void Awake () {



        //사운드 컨트롤러 불러오기
        audioController = GameObject.FindObjectOfType<AudioController>();
        ChangeSoundVolume(0.4f);

        //라운드 데이터 불러오기
        dataController = GameObject.FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.allQuestions;
        //라운드 사운드 데이터 불러오기
        allWordsSound = dataController.GetCurrentRoundAudio();

        //오디오 플레이어 초기화
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = wordSound;

        //읽은 카드에 따라 바뀔 텍스트 및 이미지
        wordText = GameObject.Find("WordText");

        //캐릭터 넘버에 따라서 방향을 바꿔 초기화
        characterNum = PlayerPrefs.GetInt("characterNum");

        if (characterNum % 2 != 0)
        {
            character = GameObject.Find("CharacterOddNum");
        }
        else {
            character = GameObject.Find("CharacterEvenNum");
        }

        //캐릭터 넘버에 맞게 캐릭터 이미지 교체
        character.GetComponent<RawImage>().texture = Resources.Load<Texture>("Characters/cat0" + characterNum);

    }


    //카드 이름으로 인덱스 찾기
    public void GetCardName(string cardName)
    {

        for (int i = 0; i < questionPool.Length; i++)
        {
            if (cardName == questionPool[i].answerImageTarget)
            {
                thisCardIndex = i;
                wordText.GetComponent<Text>().text = questionPool[thisCardIndex].questionText;                
            }
   
        }

        wordSound = allWordsSound[thisCardIndex];

        StartCoroutine(ReadThisCard());
       

    }


    IEnumerator ReadThisCard() {

        iTween.MoveTo(wordText, WordPositionTo.position, 1);
        audioSource.PlayOneShot(wordSound);

       


        if (characterNum % 2 != 0)
        {
            iTween.MoveTo(character, CharacterOddPositionTo.position, 1);
        }
        else
        {
            iTween.MoveTo(character, CharacterEvenPositionTo.position, 1);
        }
        
        
        yield return new WaitForSeconds(2f);

        iTween.MoveTo(wordText, WordPositionFrom.position, 2);

        if (characterNum % 2 != 0)
        {
            iTween.MoveTo(character, CharacterOddPositionFrom.position, 2);
        }
        else
        {
            iTween.MoveTo(character, CharacterEvenPositionFrom.position, 2);
        }

        
    }


    public void ChangeSoundVolume(float volume) {
        audioController.GetComponent<AudioSource>().volume = volume;
    }

}
