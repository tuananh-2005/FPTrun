using UnityEngine;

public class ItemMovement : MonoBehaviour
{
	public float speed = 5f;

	private void Start()
	{
		Object.Destroy(base.gameObject, 10f);
	}

	private void Update()
	{
		base.transform.position += Vector3.left * speed * Time.deltaTime;
	}
}
