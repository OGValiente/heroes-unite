using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private Vector2 inputPos;
	private float holdTimer = 0f;
	private bool holding;
	private bool panelOpened;
	public Action InputForThreeSeconds;
	public Action InputCancelled;

	void Update()
    {
		if (holding)
		{
			holdTimer += Time.deltaTime;
		}
		
		if (holdTimer > 3f && !panelOpened)
		{
			InputForThreeSeconds?.Invoke();
			panelOpened = true;
		}
		
/*
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
		if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
		{
			inputPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			CheckRaycastHit(inputPos);
		}
#else
		if (Input.GetMouseButtonDown(0))
		{
			inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			CheckRaycastHit(inputPos);
		}
#endif
*/
	}

	private void CheckRaycastHit(Vector2 inputPos2D)
	{
		RaycastHit2D hitInfo = Physics2D.Raycast(inputPos2D, Camera.main.transform.forward);
		if (hitInfo.collider != null)
		{
			if (hitInfo.collider.CompareTag("Hero"))
			{
				Debug.Log("Hero tapped");
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		holding = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		InputCancelled?.Invoke();
		holding = false;
		holdTimer = 0f;
		panelOpened = false;
	}
}
