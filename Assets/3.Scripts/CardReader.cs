using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;

public class CardReader : MonoBehaviour, ITrackableEventHandler {

    private TrackableBehaviour trackableBehaviour;

    StudySceneController studySceneController;
    GameSceneController gameSceneController;

    private string currentScene;

    private AudioClip[] allEnglishWordsSound;

    // Use this for initialization
    void Start()
    {
        //gameObject에서 컴포넌트 초기화      
        trackableBehaviour = GetComponent<TrackableBehaviour>();
        //컴포넌트가 존재하면, vuforia의 이미지 인식 이벤트에 등록
        if (trackableBehaviour) { trackableBehaviour.RegisterTrackableEventHandler(this); }

        currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene) {
            case "Study": studySceneController = FindObjectOfType<StudySceneController>(); break;
            case "Quiz": gameSceneController = FindObjectOfType<GameSceneController>(); break;

        }

    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {


        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {

            switch (currentScene) {
                case "Study": studySceneController.SendMessage("GetCardName", trackableBehaviour.TrackableName); break;
                case "Quiz": gameSceneController.SendMessage("GetUserAnswer", trackableBehaviour.TrackableName); break;

            }
            

        }
    }

  
   
}
