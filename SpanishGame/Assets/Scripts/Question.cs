using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour {
    public string question;
    public List<string> choices;
    public int correntAns;
	// Use this for initialization
	void Start () {
        
	}
    public Question(string q, List<string> a, int c)
    {
        question = q;
        choices = a;
        correntAns = c;//correct answer is from 0-2 one for every person's button
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
