using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour {

    //데이터 호출 관련 변수
    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;
    private List<AudioClip> allWordsSound = new List<AudioClip>();

    //질문 디스플레이 관련 변수
    private int questionIndex = 1;
    private Text questionText;
    private Text questionIndexText;

    //질문 오디오 관련 변수
    private AudioSource audioSource;

    //답변 관련 변수
    private string userAnswer;

    //채점 UI 관련 변수
    private GameObject signGrade;
    private Texture signCorrect;
    private Texture signIncorrect;

    //채점 오디오 관련 변수
    public AudioClip correctSound;
    public AudioClip incorrectSound;


    //채점 후 점수 관련 변수
    private int correctCount = 0;
    private int incorrectCount = 0;
    private Text correctCountText;
    private Text incorrectCountText;

    //게임 종료 디스플레이
    private GameObject gameOverDisplay;

    private Text finalCorrectCount;
    private Text finalInorrectCount;

    private RawImage characterOddNum;
    private RawImage characterEvenNum;


    // Use this for initialization
    void Start()
    {

        //데이터 불러오기
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.allQuestions;
        //라운드 사운드 데이터 불러오기
        allWordsSound = dataController.GetCurrentRoundAudio();


        //질문 관련 변수 초기화
        questionText = GameObject.Find("QuestionText").GetComponent<Text>();
        questionIndexText = GameObject.Find("QuestionIndexText").GetComponent<Text>();

        questionIndex = 0;

        //오디오 플레이어 초기화
        audioSource = gameObject.GetComponents<AudioSource>()[0];
 

        //채점 관련 변수 초기화
        signGrade = GameObject.Find("Sign");
        signCorrect = Resources.Load<Texture>("GameUI/SignCorrect");
        signIncorrect = Resources.Load<Texture>("GameUI/SignIncorrect");
        signGrade.SetActive(false);

        //채점 후 점수 관련 변수 초기화
        correctCountText = GameObject.Find("CorrectCount").GetComponent<Text>();
        incorrectCountText = GameObject.Find("IncorrectCount").GetComponent<Text>();

        //게임 종료 디스플레이 관련 변수 초기화
        gameOverDisplay = GameObject.Find("GameOverDisplay");
        finalCorrectCount = GameObject.Find("FinalCorrectCount").GetComponent<Text>();
        finalInorrectCount = GameObject.Find("FinalIncorrectCount").GetComponent<Text>();

        characterOddNum = GameObject.Find("CharacterOddNum").GetComponent<RawImage>();
        characterEvenNum = GameObject.Find("CharacterEvenNum").GetComponent<RawImage>();

        int characterNum = PlayerPrefs.GetInt("characterNum");

        if (characterNum % 2 != 0) {
            characterOddNum.texture = Resources.Load<Texture>("Characters/cat0" + characterNum);
            characterEvenNum.enabled = false; }
        else{
            characterEvenNum.texture = Resources.Load<Texture>("Characters/cat0" + characterNum);
            characterOddNum.enabled = false; }
        
        gameOverDisplay.SetActive(false);

        //질문 보여주기
        ShowQuestion();


    }

    //질문 보여주는 함수
    public void ShowQuestion()
    {
        //질문 텍스트
        questionText.text = questionPool[questionIndex].questionText;
        //몇 번째 질문인지 확인
        questionIndexText.text = "[" + (questionIndex + 1) + "/" + currentRoundData.totalQuestions + "]";

        //질문에 맞는 오디오 플레이
        audioSource.PlayOneShot(allWordsSound[questionIndex]);
        

    }


    //사용자의 답변 받아오기
    public void GetUserAnswer(string sentUserAnswer)
    {
        userAnswer = sentUserAnswer;
        StartCoroutine(GradeAnswer(userAnswer));
    }

    //답변 채점하기
    IEnumerator GradeAnswer(string userAnswer)
    {
        string correctAnswer = questionPool[questionIndex].answerImageTarget;

        //정답인 경우
        if (userAnswer.Equals(correctAnswer))
        {
            //채점 사인 보여주기
            signGrade.GetComponentInChildren<RawImage>().texture = signCorrect;
            //정답 사운드 플레이
            audioSource.PlayOneShot(correctSound);

            //맞은 개수 증가
            correctCount++;
            //채점 후 점수 보여주기
            correctCountText.text = correctCount.ToString();
            
        }
        //오답인 경우
        else {

            //채점 사인 보여주기
            signGrade.GetComponentInChildren<RawImage>().texture = signIncorrect;
            //오답 사운드 플레이
            audioSource.PlayOneShot(incorrectSound);

            //틀린 개수 증가
            incorrectCount++;
            //채점 후 점수 보여주기
            incorrectCountText.text = incorrectCount.ToString();

        }

        signGrade.SetActive(true);

        //정답 표시 2초 후 다음 문제로 넘어감
        yield return new WaitForSeconds(1f);
        NextQuestion();

    }

    //다음 질문 보여주기
    public void NextQuestion()
    {
        if (questionIndex == questionPool.Length-1) { GameOver(); return; }

        signGrade.SetActive(false);
        questionIndex++;
        ShowQuestion();

    }

    //퀴즈가 종료되었을 때 
    public void GameOver()
    {
        gameOverDisplay.SetActive(true);
        finalCorrectCount.text = correctCount.ToString();
        finalInorrectCount.text = incorrectCount.ToString();

    }


    //다시 듣기
    public void RepeatCardSound() {

        audioSource.PlayOneShot(allWordsSound[questionIndex]);

    }

    //퀴즈 그만두기 버튼 클릭 시
    public void WantToQuitQuiz(GameObject quitDisplay)
    {
        quitDisplay.SetActive(true);
    }

    public void QuitQuiz(GameObject quitDisplay) {
        quitDisplay.SetActive(false);
        GameOver();
    }

    //퀴즈로 돌아가기 버튼 클릭 시
    public void BackToQuiz(GameObject quitDisplay)
    {
        quitDisplay.SetActive(false);
    }


}
