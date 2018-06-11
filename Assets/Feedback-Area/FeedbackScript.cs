using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackScript : MonoBehaviour {
    private string file;

    private List<string> questions;
    private List<string> answers;

    private GameObject wall;

    private GameObject feedbackPanel;

    private GameObject scaleButtons;
    private GameObject yesNoButtons;

    private GameObject backPanel;
    private int currentQuestion;

    // Use this for initialization
    void Start () {
        file = Path.Combine(Application.streamingAssetsPath, "feedback.csv");
        questions = new List<string>();
        answers = new List<string>();
        currentQuestion = 0;
        feedbackPanel = GameObject.Find("R_FeedbackPanel");
        backPanel = GameObject.Find("BackPanel");
        backPanel.SetActive(false);
        wall = GameObject.Find("R_InstructionsWall");
        scaleButtons = GameObject.Find("ScaleButtons");
        yesNoButtons = GameObject.Find("YesNoButtons");
        scaleButtons.SetActive(false);
        LoadCSV(file);
        FeedbackButtonScript.FeedbackButtonEvent += HandleFeedback;
    }

	//TODO: Handle commas in questions.
    private void LoadCSV(string file)
    {
        if(!File.Exists(file))
            FileDoesntExist();

		using(StreamReader reader = new StreamReader(file))
		{
			if(!reader.EndOfStream)
			{
                string questionsFromCSV = reader.ReadLine();
                questions.AddRange(questionsFromCSV.Split(','));
                DisplayOnWall();
            }
			else
			{
                Debug.Log("There are no questions in feedback.csv!");
            }
		}
    }

	private void DisplayOnWall()
	{
        if(currentQuestion >= questions.Count)
        {
			wall.GetComponentInChildren<TextMeshPro>().text = "Thanks for your feedback!";
            FeedbackDone();
			return;
        }
        wall.GetComponentInChildren<TextMeshPro>().text = questions[currentQuestion];
    }

    private void FeedbackDone()
    {
        
		SaveCSV();
        ShowBackButton();
    }

    private void ShowBackButton()
    {
        feedbackPanel.SetActive(false);
        backPanel.SetActive(true);
    }

    private void SaveCSV()
    {
        File.AppendAllText(file, Environment.NewLine + string.Join(",", answers.ToArray()));
    }

    private void FileDoesntExist()
    {
        Debug.Log("feedback.csv does not exist! Please create one with questions!");
    }

    void HandleFeedback(string value)
    {
        SaveFeedback(value);
        currentQuestion++;
        if(currentQuestion == 1)
        {
            yesNoButtons.SetActive(false);
            scaleButtons.SetActive(true);
        }
        DisplayOnWall();
    }

    private void SaveFeedback(string value)
    {
        answers.Add(value);
    }

    // Update is called once per frame

	private void OnDestroy()
	{
		FeedbackButtonScript.FeedbackButtonEvent -= HandleFeedback;
	}

    void Update()
    {
        
    }
}
