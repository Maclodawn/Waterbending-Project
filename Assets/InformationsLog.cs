using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class InformationsLog : MonoBehaviour {

	private GUIText text = null;
	private Queue<string> todo = null;

	private string[] onscreen = null;
	private int firstonscreen = 0;

	private readonly static int MAX_NB_MSGS = 10;

	public void Start() {
		//retrieving useful component
		text = GetComponent<GUIText>();

		//todo contains the list of elements to wait before printed
		todo = new Queue<string>();

		//onscreen contains the list of elements printed on screen
		onscreen = new string[MAX_NB_MSGS];
	}

	//adding message to wait before printed on screen
	public void log(string msg) {
		todo.Enqueue(msg);
	}

	//adding msg to be printed on screen
	private void addMessageOnScreen(string msg) {
		onscreen[firstonscreen] = msg;
		firstonscreen = (firstonscreen+1)%onscreen.Length;
	}

	//FIXME problem with updates...
	public void OnGUI() {
		//adapting text position with screen size
		text.pixelOffset = new Vector2(Screen.width-10, Screen.height-10);

		//updating onscreen messages list
		int j = 0;
		while (j < Mathf.Min(onscreen.Length, 3) && todo.Count > 0) {
			addMessageOnScreen(todo.Dequeue());
			++j;
		}

		//building new text to print
		StringBuilder builder = new StringBuilder();
		int i = firstonscreen;
		do {
			builder.AppendLine(onscreen[i]);
			i = (i+1)%onscreen.Length;
		} while (i != firstonscreen);
		text.text = builder.ToString();
	}
}
