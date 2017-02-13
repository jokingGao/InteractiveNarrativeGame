using UnityEngine;
using System.Collections;

public class DoorMotion : MonoBehaviour {

    public GameObject Door;
	public GameObject hero;
    Animator dooranimator;
	public ButtonController but;
    bool open;

    // Use this for initialization
    void Start () {
        dooranimator = Door.GetComponent<Animator>();
		hero = GameObject.FindGameObjectWithTag ("Hero");
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (but.buttonpressed == true && !open)
        {
            dooranimator.SetBool("Open", true);
            open = true;
            but.buttonpressed = false;
        }
        else if(but.buttonpressed == true && open)
        {
            dooranimator.SetBool("Open", false);
            open = false;
            but.buttonpressed = false;
        }
			
    }
}
