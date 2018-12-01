using UnityEngine;

public interface IMonoBehaviour
{
	Transform Transform();
	GameObject GameObject();
	bool IsDestroyed();
}