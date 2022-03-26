using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Class] GameManager
 * 게임의 전반적인 실행을 관리합니다.
 */
public class GameManager : MonoBehaviour
{
	// Editor에서 지정 할 수 있게 public으로 지정 (outlet connection)
	public Text message;

	private Camera camera;

	private Color defaultColor = new Color(0.8066038f, 0.9113393f, 1f);

	private string defaultMessage = "화면을 클릭해 반응속도를 테스트해보세요!\n(총 5번 시행하여 평균값을 출력합니다.)";

	private void Start()
	{
		message.text = defaultMessage;
	}

	private void Update()
	{
		/*
		 * [Note: Method] Input.GetMouseButtonDown(int button)
		 * 마우스 클릭을 인식합니다.
		 * 
		 * <int button>
		 * 0 - 좌클릭
		 * 1 - 우클릭
		 * 2 - 휠클릭
		 */
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("왼쪽 마우스가 클릭되었습니다.");
		}
	}
}
