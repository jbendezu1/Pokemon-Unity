using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	public float moveSpeed;

	private bool isMoving;
	private Vector2 input;

	private Animator animator;

	private void Awake(){
		animator = GetComponent<Animator>();
	}

	private void Update(){

		if(!isMoving){
			input.x = Input.GetAxisRaw("Horizontal");
			input.y = Input.GetAxisRaw("Vertical");
			if(input.x != 0) input.y = 0;
			if(input != Vector2.zero){
				animator.SetFloat("MoveX", input.x);
				animator.SetFloat("MoveY", input.y);

				var targetPosition = transform.position;
				targetPosition.x += input.x;
				targetPosition.y += input.y;
				StartCoroutine(Move(targetPosition));
			}
		}
		animator.SetBool("isMoving", isMoving);

	}

	IEnumerator Move(Vector3 targetPosition){

		isMoving = true;

		while((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon){
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}
		transform.position = targetPosition;
		isMoving = false;
	}

}
