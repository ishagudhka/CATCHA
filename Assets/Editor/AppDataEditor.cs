using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor; //editor 생성에 필요
using System.IO;   //Json파일

public class AppDataEditor : EditorWindow {

    Vector2 scrollPos;
    public ApplicationData allData;

    //파일 경로
    private string allDataProjectFilePath = "/StreamingAssets/data.json";

    //윈도우 메뉴에 게임데이터 에디터 생성
    [MenuItem("Window/Application Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(AppDataEditor)).Show();
    }


    
    void OnGUI()
    {

        //scrollView 시작점
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(550), GUILayout.Height(800));
   


        if (allData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("allData");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                SaveAppData();
            }
        }

        if (GUILayout.Button("Load data"))
        {
            LoadAppData();
        }

        //Scrollview 끝점
        EditorGUILayout.EndScrollView();

    }

    //데이터 로드
    private void LoadAppData()
    {
        
        string filePath = Application.dataPath + allDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            allData = JsonUtility.FromJson<ApplicationData>(dataAsJson);
 
        }
        else
        {
            allData = new ApplicationData();
        }

        
    }

    //데이터 저장
    private void SaveAppData()
    {

        string dataAsJson = JsonUtility.ToJson(allData);

        string filePath = Application.dataPath + allDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }

}
