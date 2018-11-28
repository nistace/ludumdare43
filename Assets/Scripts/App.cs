using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class App : MonoBehaviour
{
	public static App Instance { get; private set; }

	public Canvas loadingCanvas;
	private Text loadingProgressText;
	private AsyncOperation sceneLoading;

	public void Awake()
	{
		if (Instance == null) Instance = this;
		if (Instance != this) Destroy(this);
		else
		{
			DontDestroyOnLoad(this.gameObject);
			DontDestroyOnLoad(this.loadingCanvas);
			this.loadingCanvas.gameObject.SetActive(false);
			this.loadingProgressText = this.loadingCanvas.transform.Find("Progress").GetComponent<Text>();
		}
	}

	public void OnDestroy()
	{
		if (Instance == this) Instance = null;
	}

	public void Update()
	{
		if (this.sceneLoading != null)
		{
			this.loadingProgressText.text = (100 * this.sceneLoading.progress) + "%";
			if (this.sceneLoading.isDone)
				this.loadingCanvas.gameObject.SetActive(false);
		}
	}

	public void StartGame()
	{
		this.loadingCanvas.gameObject.SetActive(true);
		this.sceneLoading = SceneManager.LoadSceneAsync("Game");
	}

	public void GotoMenu()
	{
		this.loadingCanvas.gameObject.SetActive(true);
		this.sceneLoading = SceneManager.LoadSceneAsync("Menu");
	}

}
