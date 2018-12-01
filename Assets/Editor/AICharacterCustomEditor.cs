
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AICharacter))]
public class AICharacterCustomEditor : Editor
{
	public void OnSceneGUI()
	{
		AICharacter aiCharacter = (AICharacter)target;
		if (aiCharacter.target != null)
		{
			Handles.color = Color.yellow;
			Handles.DrawLine(aiCharacter.transform.position, aiCharacter.target.Transform().position);
		}
	}
}