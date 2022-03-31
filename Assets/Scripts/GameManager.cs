using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Enum] GameStatus
 * 게임의 현재 상태를 나타냅니다.
 */
public enum GameStatus
{
	Started,
	Ready,
	Wait,
	Recording,
	Pause,
	Finish
}

/*
 * [Class] GameManager
 * 게임의 전반적인 실행을 관리합니다.
 */
public class GameManager : MonoBehaviour
{
	public Text message;

	private Camera mainCamera;
	private GameStatus gameStatus = GameStatus.Started;

	private Color colorDefault = new Color(0.8066038f, 0.9113393f, 1f);
	private Color colorWait = new Color(1f, 1f, 0.6839622f);
	private Color colorRecord = new Color(1f, 0.3915094f, 0.3915094f);
	private Color colorPause = new Color(0.8485615f, 0.7311321f, 1f);

	private string messageDefault = "화면을 클릭해 반응속도를 테스트해보세요!\n(총 {0}번 시행하여 평균값을 출력해요.)";
	private string messageWait = "화면이 <color=#FF6464>빨간색</color>이 되면 화면을 클릭하세요!";
	private string messageWaitNot = "아직 아니에요. 조금만 기다리세요...";
	private string messageRecord = "지금!";
	private string messageResult = "[{0}/{1}회] 측정 결과: <color=#FFFFAE>{2:F2}ms</color>\n화면을 클릭하여 측정을 계속 할 수 있어요.";
	private string messageFinish = "최종 결과: <color=#FFFFAE>{0:F2}ms</color>\n화면을 클릭하여 측정을 다시 할 수 있어요.";

	private int currentRound = 1;
	[SerializeField] private int maxRound = 5; // outlet connection

	private float changeWait = 0f; // 화면 전환까지 소요될 시간
	private float elapsedTime = 0f;
	private List<float> records = new List<float>();

	private void Start()
	{
		Debug.Log("[GameManger.Start()] 게임이 시작되었습니다. 필요한 내용을 초기화합니다.");

		mainCamera = Camera.main;
		mainCamera.backgroundColor = colorDefault;

		message.text = string.Format(messageDefault, maxRound);

		Debug.Log("[GameManger.Start()] 초기화가 완료되었습니다. 게임을 시작합니다.");
	}

	private void Update()
	{
		/*
		 * [Note: Method] Input.GetMouseButtonDown(int button): bool
		 * 마우스 클릭을 인식합니다.
		 * 
		 * <int button>
		 * 인식할 버튼을 지정합니다.
		 * 0 - 좌클릭
		 * 1 - 우클릭
		 * 2 - 휠클릭
		 */

		/*
		 * [Note: Method] Random.Range(int minInclusive, int maxInclusive): int
		 * a 이상 b *미만*의 수를 반환합니다.
		 * 
		 * [Note: Method] Random.Range(float minInclusive, float maxInclusive): float
		 * a 이상 b *이하*의 수를 반환합니다.
		 */

		/*
		 * [Note: Variable] Time.deltaTime: float
		 * 직전 프레임으로부터의 경과 시간을 us로 반환합니다.
		 * (s < ms < us < ns)
		 */

		switch (gameStatus)
		{
			case GameStatus.Started:
				if (Input.GetMouseButtonDown(0))
				{
					Debug.Log("[GameManger.Update()] 사용자가 왼쪽 마우스를 클릭했습니다.");
					gameStatus = GameStatus.Ready;
				}
				break;
			case GameStatus.Ready:
				mainCamera.backgroundColor = colorWait;
				message.text = messageWait;

				changeWait = Random.Range(1.5f, 5.0f);

				gameStatus = GameStatus.Wait;

				break;
			case GameStatus.Wait:
				if (changeWait <= 0f)
				{
					Debug.Log("[GameManger.Update()] 화면이 전환되었습니다. 측정을 시작합니다.");

					mainCamera.backgroundColor = colorRecord;
					message.text = messageRecord;

					elapsedTime = 0f;

					gameStatus = GameStatus.Recording;
				}
				else
					changeWait -= Time.deltaTime;

				if (Input.GetMouseButtonDown(0))
				{
					message.text = messageWaitNot;
				}

				break;
			case GameStatus.Recording:
				elapsedTime += Time.deltaTime;

				if (Input.GetMouseButtonDown(0))
				{
					float result = elapsedTime * 1000f;

					Debug.Log("[GameManger.Update()] 사용자가 왼쪽 마우스를 클릭했습니다. 화면 전환 후 " + result + "ms가 지났습니다.");

					mainCamera.backgroundColor = colorPause;
					message.text = string.Format(messageResult, currentRound, maxRound, result);

					records.Add(result);
					currentRound++;

					gameStatus = GameStatus.Pause;
				}
				break;
			case GameStatus.Pause:
				if (currentRound <= maxRound)
				{
					if (Input.GetMouseButtonDown(0))
					{
						gameStatus = GameStatus.Ready;
					}
				}
				else
				{
					float averageTime = 0f;

					for (int i = 0; i < records.Count; i++)
						averageTime += records[i];

					averageTime /= records.Count;

					Debug.Log("[GameManger.Update()] 게임이 종료되었습니다. 평균 반응 속도는" + averageTime + "ms 입니다.");

					message.text = string.Format(messageFinish, averageTime);
					gameStatus = GameStatus.Finish;
				}
				break;
			case GameStatus.Finish:
				if (Input.GetMouseButtonDown(0))
				{

					Debug.Log("[GameManger.Update()] 사용자가 왼쪽 마우스를 클릭했습니다. 게임을 다시 시작합니다.");

					records.Clear();
					currentRound = 1;

					mainCamera.backgroundColor = colorDefault;
					message.text = string.Format(messageDefault, maxRound);

					gameStatus = GameStatus.Started;
				}
				break;
		}
	}
}
