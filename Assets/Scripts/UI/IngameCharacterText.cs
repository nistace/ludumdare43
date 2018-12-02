using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class IngameCharacterText : MonoBehaviour
{
	public HumanCharacter target;
	private float timeBeforeHide;
	private bool active;
	private Text text;
	private Color invisibleColor;
	private Color visibleColor;

	public void Awake()
	{
		this.text = this.GetComponent<Text>();
		this.invisibleColor = new Color(this.text.color.r, this.text.color.g, this.text.color.b, 0);
		this.visibleColor = new Color(this.text.color.r, this.text.color.g, this.text.color.b, 1);
		this.target.OnSaySomething += this.ShowText;
		this.active = false;
		this.text.color = this.invisibleColor;
	}

	public void Update()
	{
		if (this.target == null || this.target.IsDestroyed())
			Destroy(this.gameObject);
		else if (active)
		{
			this.timeBeforeHide -= Time.deltaTime;
			if (this.timeBeforeHide <= 0)
			{
				this.text.color = this.invisibleColor;
				this.active = false;
			}
			else
			{
				this.transform.position = Camera.main.WorldToScreenPoint(this.target.transform.position + Vector3.up * .3f);
			}
		}
	}

	public void ShowText(string text, float duration)
	{
		this.timeBeforeHide = duration;
		this.text.color = this.visibleColor;
		this.text.text = text;
		this.active = true;
	}


}
