using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour {
    public string question;
    public List<string> choices;
    public int type;
    public int correntAns;
    public string example;
	// Use this for initialization
	void Start () {
        
	}
    public Question(string q, List<string> a, int c, int t)
    {
        question = q;
        choices = a;
        correntAns = c;//correct answer is from 0-2 one for every person's button
        example = "";
        type = t;//1 = fill in the blank, 2 = scrambled word and categorize, 3 = correct congegated word
    }
    public Question(string q, List<string> a, int c, int t, string s)
    {
        question = q;
        choices = a;
        example = s;
        correntAns = c;//correct answer is from 0-2 one for every person's button
        type = t;//1 = fill in the blank, 2 = scrambled word and categorize, 3 = correct congegated word
    }

    // Update is called once per frame
    void Update () {
		
	}
}
