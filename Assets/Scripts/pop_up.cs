using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pop_up : MonoBehaviour
{
	[SerializeField] float delay = (float)0.05;
	[SerializeField] float page_delay = (float)10.0;
	[SerializeField] GameObject heal_prompt;
	public Text textBox;
	private string currentText;
	[SerializeField] GameObject text_stuff;

	void OnTriggerEnter(Collider Player)
	{
		text_stuff.SetActive(true);
		string[] lines = new string[] { "Oh, apologies…! It appears we haven’t started on the right foot.", "I’m making way towards the nearest medical facility.", "It’s a tad difficult given my predicament, but I’ll be just fine!", "Could you spare some of that morphine?"};
		StartCoroutine(SplitPopUpText(lines, GameObject.Find("Holder")));
	}

    private void OnTriggerExit(Collider Player)
    {
		print("exited trigger");
		GameObject.Find("Holder").SetActive(false);
		heal_prompt.SetActive(false);
	}

    IEnumerator PopUpText(string line)
	{
		Debug.Log("pop up text started");

		for (int i = 0; i < line.Length; i++)
		{

			Debug.Log(i);
			Debug.Log(line.Length);
			float delayTemp = delay;
			string check = line.Substring(i, 1);
			string check2 = " ";
			Debug.Log(check);
			if (check == check2)
			{
				delay = 0;
			}

			currentText = line.Substring(0, i + 1);
			textBox.text = currentText;
			yield return new WaitForSeconds(delay);
			delay = delayTemp;
		}

		yield return new WaitForSeconds(delay);

	}

	IEnumerator SplitPopUpText(string[] lines, GameObject holder)
	{
		foreach(string line in lines)
        {
			StartCoroutine(PopUpText(line));
			yield return new WaitForSeconds(page_delay);
		}
		holder.SetActive(false);
		heal_prompt.SetActive(true);
	}
}
