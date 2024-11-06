using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{
	[SerializeField]
	private Text questionText;

	[SerializeField]
	private Button[] answerButtons;

	private Action<bool> _onAnswered;

	private int correctAnswerIndex;

	public void ShowQuestion(string question, string[] answers, int correctIndex, Action<bool> onAnswered)
	{
		questionText.text = question;
		correctAnswerIndex = correctIndex;
		for (int i = 0; i < answerButtons.Length; i++)
		{
			int index = i;
			answerButtons[i].GetComponentInChildren<Text>().text = answers[i];
			answerButtons[i].onClick.RemoveAllListeners();
			answerButtons[i].onClick.AddListener(delegate
			{
				AnswerSelected(index == correctAnswerIndex);
			});
		}
		_onAnswered = onAnswered;
		base.gameObject.SetActive(value: true);
		Time.timeScale = 0f;
	}

	private void AnswerSelected(bool isCorrect)
	{
		base.gameObject.SetActive(value: false);
		Time.timeScale = 1f;
		_onAnswered?.Invoke(isCorrect);
	}
}
