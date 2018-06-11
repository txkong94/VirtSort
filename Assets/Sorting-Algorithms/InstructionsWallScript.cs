using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class InstructionsWallScript : MonoBehaviour {

    public VideoClip clip;
    public AudioSource audioSource;
	VideoPlayer videoPlayer;
    private R_Manager managerScript;
    // Use this for initialization

    void Start () {

        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
       	videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.source = VideoSource.VideoClip;
		videoPlayer.started += VideoStarted;
		videoPlayer.loopPointReached += VideoStopped;
        VideoButtonScript.VideoButtonEvent += HandlePlay;
    }

    void OnDestroy()
    {
        videoPlayer.started -= VideoStarted;
		videoPlayer.loopPointReached -= VideoStopped;
        VideoButtonScript.VideoButtonEvent -= HandlePlay;
    }
    private void VideoStarted(VideoPlayer source)
    {
        disableText();
    }

    private void VideoStopped(VideoPlayer source)
    {
        source.Stop();
		enableText();
    }

    // Update is called once per frame
    void Update () {

    }

	public void disableText()
	{
        foreach(TextMeshPro tmpro in GetComponentsInChildren<TextMeshPro>())
		{
            tmpro.enabled = false;
        }
    }
		public void enableText()
	{
        foreach(TextMeshPro tmpro in GetComponentsInChildren<TextMeshPro>())
		{
            tmpro.enabled = true;
        }
    }

    internal void playMovie()
    {
		if(!videoPlayer.isPlaying)
		{
            //Old videos located in Resources/old
			//Crappy code that works.	
			switch ((int)SortStateVariables.SortingAlgorithm)
			{
				case (int)ESortingAlgorithm.BubbleSort:
					videoPlayer.clip = Resources.Load("BubbleSort", typeof(VideoClip)) as VideoClip;
					break;
				case (int)ESortingAlgorithm.BubbleSortOptimized:
					videoPlayer.clip = Resources.Load("BubbleSort", typeof(VideoClip)) as VideoClip;
					break;
				case (int)ESortingAlgorithm.InsertionSort:
					videoPlayer.clip = Resources.Load("InsertionSort", typeof(VideoClip)) as VideoClip;
					break;
				case (int)ESortingAlgorithm.InsertionSortRecursive:
					videoPlayer.clip = Resources.Load("InsertionSort", typeof(VideoClip)) as VideoClip;
					break;
				case (int)ESortingAlgorithm.QuickSort:
					videoPlayer.clip = Resources.Load("QuickSort", typeof(VideoClip)) as VideoClip;
					break;
				default:
					videoPlayer.clip = Resources.Load("InsertionSort", typeof(VideoClip)) as VideoClip;
					break;
			}
			
            videoPlayer.Play();
        }
		else
		{
            VideoStopped(videoPlayer);
        }
    }

    //
    // Handle button events
    //

    void HandlePlay()
    {
        playMovie();
    }
}
