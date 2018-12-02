using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanelManager : MonoBehaviour
{
	public Transform textsPanel;
	public string[] characterNames;
	public Color[] colors;

	private TextAsset scriptAsset;
	private Text[] texts;
	private readonly Dictionary<string, Color> characterColors = new Dictionary<string, Color>();
	private bool skip;

	private int dialogLineIndex;

	public void Awake()
	{
		this.texts = new Text[this.textsPanel.childCount];
		for (int i = 0; i < this.textsPanel.childCount; ++i)
			this.texts[i] = this.textsPanel.GetChild(i).GetComponent<Text>();
		for (int i = 0; i < Math.Min(this.colors.Length, this.characterNames.Length); ++i)
			this.characterColors.Add(this.characterNames[i], this.colors[i]);
		GameController.Instance.OnDialogScript += this.StartDialog;
		this.gameObject.SetActive(false);
	}

	public void StartDialog(TextAsset script)
	{
		this.scriptAsset = script;
		this.skip = false;
		this.Clear();
		this.gameObject.SetActive(true);
		this.dialogLineIndex = 0;
		this.StartCoroutine(this.FollowScript());
	}

	public void Skip()
	{
		this.skip = true;
	}

	public IEnumerator FollowScript()
	{
		yield return new WaitForSeconds(.5f);
		foreach (string instruction in this.scriptAsset.text.Split("\n"[0]).Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)))
		{
			if (!this.skip)
			{
				string[] instructionData = instruction.Split(' ');
				switch (instructionData[0])
				{
					case "text": { yield return this.WriteText(instruction); } break;
					case "wait": { yield return new WaitForSeconds(float.Parse(instructionData[1])); } break;
					case "fade": { yield return this.FadeOut(float.Parse(instructionData[1])); } break;
					case "cls": { this.Clear(); } break;
				}
			}
		}
		this.gameObject.SetActive(false);
		GameController.Instance.OnEndOfWaveScript();
	}

	public IEnumerator WriteText(string intructionsLine)
	{
		string[] instructionData = intructionsLine.Split(new string[] { "-t:" }, StringSplitOptions.None)[0].Split(' ');
		string text = intructionsLine.Split(new string[] { "-t:" }, StringSplitOptions.None)[1];
		string characterName = "";
		float speed = .045f;
		Color color = Color.white;
		foreach (string instructionParam in instructionData)
		{
			if (instructionParam.StartsWith("-n:"))
			{
				characterName = instructionParam.Remove(0, 3);
				if (this.characterColors.ContainsKey(characterName))
					color = this.characterColors[characterName];
			}
			else if (instructionParam.StartsWith("-s:"))
			{
				speed = float.Parse(instructionParam.Remove(0, 3));
			}
		}
		// TODO manage the case when the last line is reached
		Text uiLine = this.texts[this.dialogLineIndex++];
		uiLine.text = characterName + " : ";
		uiLine.color = color;
		uiLine.gameObject.SetActive(true);
		foreach (char c in text.ToCharArray())
		{
			if (!this.skip)
			{
				UISoundManager.Instance.PlayTyping();
				yield return new WaitForSeconds(speed);
				uiLine.text += c;
			}
		}
	}

	private IEnumerator FadeOut(float seconds)
	{
		float waitPerStep = .1f;
		float alphaLossPerUnit = waitPerStep / seconds;
		for (int i = 0; i < seconds / waitPerStep; ++i)
		{
			if (!this.skip)
			{
				foreach (Text t in this.texts)
					t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - alphaLossPerUnit);
				yield return new WaitForSeconds(waitPerStep);
			}
		}
		this.Clear();
	}

	private void Clear()
	{
		foreach (Text t in this.texts)
			t.gameObject.SetActive(false);
		this.dialogLineIndex = 0;
	}

}
