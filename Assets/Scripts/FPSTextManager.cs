using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Class] FPSTextManager
 * 화면에 현재 FPS를 표시합니다.
 */
public class FPSTextManager : MonoBehaviour
{
	public Text fpsText;
	private float deltaTime = 0.0f;

	private void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		fpsText.text = Mathf.Round(fps).ToString() + " FPS";
	}
}
