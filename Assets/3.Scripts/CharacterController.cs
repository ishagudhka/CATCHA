using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController: MonoBehaviour {

    //캐릭터 이름 관련
    private string[] characterNames = { "마야", "루루", "코코", "오레오", "펌킨", "나나" };
    private Text characterName;

    //캐릭터 아이콘 관련
    private List<Image> buttonImages = new List<Image>();

    //캐릭터 이미지 관련
    private List<Sprite> characterImages = new List<Sprite>();
    private List<Texture> iconImages = new List<Texture>();
    private  Image characterImage;

    private int selectedCharacterNum;

    private RawImage characterIcon;
    private RawImage currentSceneCharacterIcon;

    private GameObject startGameDisplay;

    private string previousGameScene;

    //캐릭터 확정 디스플레이 관련
    private Text selectedCharacterText;
    private RawImage selectedCharacterIcon;
   
    // Use this for initialization
    void Start () {

        //캐릭터 가림 아이콘 배열 생성
        for (int i = 1; i < 7; i++) { 
        Image blockButton = GameObject.Find("Block0"+i).GetComponentInChildren<Image>();
        buttonImages.Add(blockButton);
        }

        //캐릭터 아이콘 배열 생성
        for (int i = 1; i < 7; i++)
        {
            Texture iconTexture = Resources.Load<Texture>("Characters/cat0" + i +"_icon");
            iconImages.Add(iconTexture);
        }


        //캐릭터 이미지 배열 생성
        for (int i = 1; i < 7; i++)
        {
            Sprite characterSprite = Resources.Load<Sprite>("Characters/cat0"+i);
            characterImages.Add(characterSprite);
        }

        //캐릭터 이미지 오브젝트 초기화
        characterImage = GameObject.Find("Character").GetComponent<Image>();

        //캐릭터 이름 오브젝트 초기화
        characterName = GameObject.Find("CharacterName").GetComponent<Text>();

        //메뉴디스플레이 프리팹 내의 캐릭터 아이콘 초기화
        characterIcon = ((GameObject)Resources.Load("MenuDisplay/MenuDisplay")).GetComponentsInChildren<RawImage>()[1];

        //플레이어 프리팹에서 캐릭터 넘버 호출
        selectedCharacterNum = PlayerPrefs.GetInt("characterNum");
        //사용자가 아무 번호도 선택하지 않은 경우 1번으로 호출
        if (selectedCharacterNum == 0) selectedCharacterNum++;
 

        //호출된 캐릭터 넘버로 아이콘 및 이미지 교체
        CharacterButtonClicked(selectedCharacterNum);

        //캐릭터 확정 디스플레이 초기화
        startGameDisplay = GameObject.Find("StartGameDisplay");
        selectedCharacterText = GameObject.Find("SelectedCharacterText").GetComponent<Text>();
        selectedCharacterIcon = GameObject.Find("SelectedCharacterIcon").GetComponent<RawImage>();

        startGameDisplay.SetActive(false);

        previousGameScene = PlayerPrefs.GetString("previousGameScene");

        

    }


    public void CharacterButtonClicked(int characterNum)
    {

        //캐릭터 넘버 전역변수에 저장
        selectedCharacterNum = characterNum;

        //클릭한 버튼 외에는 버튼을 가려주는 아이콘 활성화
        foreach (Image img in buttonImages) {img.enabled = true;}
        buttonImages[characterNum - 1].enabled = false;

        //클릭한 캐릭터로 클릭 시 이미지 교체
        characterImage.sprite = characterImages[characterNum - 1];

        //클릭한 캐릭터로 클릭 시 이름 교체
        characterName.text = characterNames[characterNum - 1];
        
    }


    public void SaveButtonClicked()
    {
        //버튼 클릭시 코루틴 함수 호출
        StartCoroutine(SaveCharacter());
    }


    

    IEnumerator SaveCharacter() {

        //다른 화면에서도 보여지도록 프리팹의 아이콘 교체
        characterIcon.texture = iconImages[selectedCharacterNum - 1];
    
        //캐릭터 확정 디스플레이의 이름 교체
        selectedCharacterText.text = characterNames[selectedCharacterNum-1]+"를 선택하셨습니다!";

        //캐릭터 확정 디스플레이의 아이콘 교체
        selectedCharacterIcon.texture = characterIcon.texture;

        //다른 씬의 애니메이션 호출을 위해 캐릭터 넘버 저장
        PlayerPrefs.SetInt("characterNum", selectedCharacterNum);

        //캐릭터 확정 디스플레이 활성화
        startGameDisplay.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        //씬 전환
        SceneManager.LoadScene(previousGameScene);

    }


   
}
