using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    public Button p1B1;
    public Button p1B2;
    public Button p1B3;

    public Button p2B1;
    public Button p2B2;
    public Button p2B3;

    public GameMain gm;

    // Use this for initialization
    void Start () {
        Button btn = p1B1.GetComponent<Button>();
        p1B1.onClick.AddListener(delegate { TaskOnClick(0); });

        Button btn2 = p1B2.GetComponent<Button>();
        p1B2.onClick.AddListener(delegate { TaskOnClick(1); });

        Button btn3 = p1B3.GetComponent<Button>();
        p1B3.onClick.AddListener(delegate { TaskOnClick(2); });

        Button btn4 = p2B1.GetComponent<Button>();
        p2B1.onClick.AddListener(delegate { TaskOnClick(3); });

        Button btn5 = p2B2.GetComponent<Button>();
        p2B2.onClick.AddListener(delegate { TaskOnClick(4); });

        Button btn6 = p2B3.GetComponent<Button>();
        p2B3.onClick.AddListener(delegate { TaskOnClick(5); });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TaskOnClick(int i)
    {
        gm.getInput(i);
    }
}
