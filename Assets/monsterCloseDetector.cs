using UnityEngine;
using System.Collections;

public class monsterCloseDetector : MonoBehaviour {

    public GameObject Hero;
    private bool CloseToHero;
    private float distance;
	// Use this for initialization
	void Start () {
        CloseToHero = false;
    }
	
	// Update is called once per frame
	void Update () {
        distance = (Hero.transform.position - this.transform.position).sqrMagnitude;

        if (distance < 5.0f)
            CloseToHero = true;
    }

    public bool monsterCloseToHero()
    {
        //print(CloseToHero);
        return CloseToHero;
    }
}
