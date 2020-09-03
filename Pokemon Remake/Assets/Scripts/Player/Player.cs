using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
	public float moveSpeed;

	public LayerMask Foreground;
	public LayerMask Grass;

	private bool isMoving;
	private Vector2 input;

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{

		if (!isMoving)
		{
			input.x = Input.GetAxisRaw("Horizontal");
			input.y = Input.GetAxisRaw("Vertical");
			if (input.x != 0) input.y = 0;
			if (input != Vector2.zero)
			{
				animator.SetFloat("MoveX", input.x);
				animator.SetFloat("MoveY", input.y);

				var targetPosition = transform.position;
				targetPosition.x += input.x;
				targetPosition.y += input.y;
				if(isWalkable(targetPosition))
					StartCoroutine(Move(targetPosition));
			}
		}
		animator.SetBool("isMoving", isMoving);

	}

	IEnumerator Move(Vector3 targetPosition)
	{

		isMoving = true;

		while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}
		transform.position = targetPosition;
		isMoving = false;

		checkForEncounter();
	}

	private bool isWalkable(Vector3 targetposition){
		if(Physics2D.OverlapCircle(targetposition, 0.2f, Foreground) != null){
			return false;
		}
		return true;
	}

	private void checkForEncounter(){
		if(Physics2D.OverlapCircle(transform.position, 0.2f, Grass) != null){
			if(Random.Range(1,101)%5 == 0){
				Debug.Log("Encountered a wild Pokemon");
			}
		}
	}
}
