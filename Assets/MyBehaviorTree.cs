//using UnityEngine;
//using System.Collections;
//using TreeSharpPlus;

//public class MyBehaviorTree : MonoBehaviour
//{
//	public Transform wander1;
//	public Transform wander2;
//	//public Transform wander3;
//    public GameObject MONSTER;
//    public GameObject HERO;
//    private BehaviorAgent behaviorAgent;
//    public GameObject button;
//    public GameObject Door;

//	// Use this for initialization
//	void Start ()
//	{
//		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
//		BehaviorManager.Instance.Register (behaviorAgent);
//		behaviorAgent.StartBehavior ();

//	}

//	// Update is called once per frame
//	void Update ()
//	{
//	}

//	protected Node ST_ApproachAndWait(Transform target)
//	{
//		Val<Vector3> position = Val.V (() => target.position);
//		return new Sequence( MONSTER.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
//	}

//    protected Node ApproachAndStare(Transform target)
//    {
//        Val<Vector3> position = Val.V(() => target.position);
//        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(1000));
//    }

//    protected Node ApproachAndStare2(Transform target)
//    {
//        Val<Vector3> position = Val.V(() => target.position);
//        return new Sequence(MONSTER.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(1000));
//    }

//    protected Node HandsUp(Transform target)
//    {
//        Val<Vector3> position = Val.V(() => target.position);
//        return new Sequence(MONSTER.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DYING", true), new LeafWait(1000));
//    }

//    /*
//    protected Node MonsterCrouchesNearHero(Transform target)
//    {
//        Val<Vector3> position = Val.V(() => target.position);
//        return new Sequence(MONSTER.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DYING", true), new LeafWait(1000));
//    }
//    */

//    protected Node HeroEngageMonster (Transform target)
//    {
//        Val<Vector3> position = Val.V(() => target.position);
//        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_HandAnimation("PISTOLAIM", true), new LeafWait(1000));
//    }

//    protected Node ApproachButton(Transform target)
//    {
//        Val<Vector3> position = Val.V(() => target.position);
//        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
//    }

//    protected Node PressButton(Transform target)
//    {
//        Val<Vector3> position = Val.V(() => target.position);
//        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT", true), new LeafWait(1000));
//    }

//    protected Node ReadWithRightHand(Transform target)
//    {
//        Val<Vector3> position = Val.V(() => target.position);
//        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_BodyAnimation("REACHRIGHT", true), new LeafWait(1000));
//    }

//    protected Node BuildTreeRoot()
//	{

//        return new DecoratorLoop(

//            new SequenceShuffle(
//                //this.ST_ApproachAndWait(this.wander1),

//               // this.ApproachAndStare2(this.HERO.transform)));
//                    //this.ApproachAndStare2(this.HERO.transform),
//                   // this.HeroEngageMonster(this.MONSTER.transform),
//                    //this.HandsUp(this.HERO.transform)));
//                    //this.ApproachButton(this.button.transform),
//                    //this.ApproachAndStare(this.MONSTER.transform),
//                   // this.PressButton(this.HERO.transform)));
//        this.ST_ApproachAndWait(this.wander1),
//        this.ST_ApproachAndWait(this.wander2)));

//    }



//}
using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
    public GameObject Player;
    public GameObject NPC;
    public GameObject Princess;
    public GameObject Monster1;
    public GameObject Monster2;
    public GameObject Monster3;

    private Val<GameObject> CopClicked;

    public Transform Wander1Start;
    public Transform Wander1End;
    public Transform Wander2Start;
    public Transform Wander2End;
    public Transform Wander3Start;
    public Transform Wander3End;
    public Transform PlayerStartPoint;
    public Transform SwordPos;
    public Transform PrincePos;

    public monsterCloseDetector monsterDetector1;
    public monsterCloseDetector monsterDetector2;
    public monsterCloseDetector monsterDetector3;

    //public PlayerControls Controls;
    public CameraController Camera;
    public textBoxController TextBox;
    //public TreasureBoxMovement Treasure;
    public GameObject Poster;
    //public PoliceCollisionAggregator PCA;

    private BehaviorAgent behaviorAgent;

    Val<Vector3> PlayerPos;
    // Use this for initialization
    void Start()
    {
        CopClicked = Val.V(() => new GameObject());
        behaviorAgent = new BehaviorAgent(this.BehaviorTree());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool returnTrue()
    {
        return true;
    }

    protected Node PrisonerFollow(GameObject Player, GameObject Prisoner)
    {
        Val<Vector3> PlayerPos = Val.V(() => Player.transform.position);
        return new Sequence(
            Prisoner.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(PlayerPos, 2.0f));
    }

    protected Node InitialConversation(GameObject Player, GameObject NPC)
    {
        PlayerPos = Val.V(() => Player.transform.position);
        Val<Vector3> NPCPos = Val.V(() => NPC.transform.position);
        Val<Vector3> StartPos = Val.V(() => PlayerStartPoint.position);
        Val<Vector3> Sword_Pos = Val.V(() => SwordPos.position);
        Val<Vector3> Prince_Pos = Val.V(() => PrincePos.position);
        //Val<Vector3> StartOrient = Val.V(() => PlayerStartOrientation.position);

            return new Sequence(new LeafWait(1000),
                Player.GetComponent<BehaviorMecanim>().Node_GoTo(StartPos),
                new Sequence(NPC.GetComponent<BehaviorMecanim>().Node_OrientTowards(PlayerPos), new LeafWait(1000)),
                NPC.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", true),
                new LeafInvoke((Func<RunStatus>)TextBox.startCounter),
                new SequenceParallel(Player.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ACKNOWLEDGE", 2000),
                                     new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
                new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                new SequenceParallel(NPC.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("WAVE", (long)2000),
                                     new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
                new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                new SequenceParallel(Player.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BEINGCOCKY", 2000),
                                     new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
                new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                new SequenceParallel(NPC.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 2000),
                                     new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
                new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),

                Player.GetComponent<BehaviorMecanim>().Node_GoTo(Sword_Pos),
                Player.GetComponent<BehaviorMecanim>().Node_BodyAnimationSwordWithoutInteract("PICKUP_SWORD", true),
                Player.GetComponent<BehaviorMecanim>().Node_GoTo(Wander2End.transform.position),
                new LeafWait(3000),
                Player.GetComponent<BehaviorMecanim>().Node_GoTo(Wander3End.transform.position),
                new LeafWait(3000),
                Player.GetComponent<BehaviorMecanim>().Node_GoTo(Wander1End.transform.position),
                new LeafWait(3000),
                Player.GetComponent<BehaviorMecanim>().Node_GoTo(Prince_Pos),
                new Sequence(Princess.GetComponent<BehaviorMecanim>().Node_OrientTowards(Prince_Pos), new LeafWait(1000)),
                new SequenceParallel(Princess.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", true),
                new LeafInvoke((Func<RunStatus>)TextBox.PrincessDialog)),
                new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                new LeafWait(2000),
                Princess.GetComponent<BehaviorMecanim>().Node_BodyAnimationKissWithoutInteract("KISSING", true)
                );
    }

    protected Node InitialConversation2(GameObject Player, GameObject NPC)
    {
        PlayerPos = Val.V(() => Player.transform.position);
        Val<Vector3> NPCPos = Val.V(() => NPC.transform.position);
        Val<Vector3> StartPos = Val.V(() => PlayerStartPoint.position);
        Val<Vector3> Sword_Pos = Val.V(() => SwordPos.position);
        Val<Vector3> Prince_Pos = Val.V(() => PrincePos.position);
        //Val<Vector3> StartOrient = Val.V(() => PlayerStartOrientation.position);

        return new Sequence(new LeafWait(1000),
            Player.GetComponent<BehaviorMecanim>().Node_GoTo(StartPos),
            new Sequence(NPC.GetComponent<BehaviorMecanim>().Node_OrientTowards(PlayerPos), new LeafWait(1000)),
            NPC.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", true),
            new LeafInvoke((Func<RunStatus>)TextBox.startCounter),
            new SequenceParallel(Player.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ACKNOWLEDGE", 2000),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            new SequenceParallel(NPC.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("WAVE", (long)2000),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            new SequenceParallel(Player.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BEINGCOCKY", 2000),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            new SequenceParallel(NPC.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 2000),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),

            //Player.GetComponent<BehaviorMecanim>().Node_GoTo(Sword_Pos),
            //Player.GetComponent<BehaviorMecanim>().Node_BodyAnimationSwordWithoutInteract("PICKUP_SWORD", true),
            Player.GetComponent<BehaviorMecanim>().Node_GoTo(Wander2End.transform.position),
            new LeafWait(3000)
            //Player.GetComponent<BehaviorMecanim>().Node_GoTo(Wander3End.transform.position),
            //new LeafWait(5000),
            //Player.GetComponent<BehaviorMecanim>().Node_GoTo(Wander1End.transform.position),
            //new LeafWait(5000),
            //Player.GetComponent<BehaviorMecanim>().Node_GoTo(Prince_Pos),
            //new Sequence(Princess.GetComponent<BehaviorMecanim>().Node_OrientTowards(Prince_Pos), new LeafWait(1000)),
            //new SequenceParallel(Princess.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", true),
            //new LeafInvoke((Func<RunStatus>)TextBox.PrincessDialog)),
            //new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            //new LeafWait(2000),
            //Princess.GetComponent<BehaviorMecanim>().Node_BodyAnimationKissWithoutInteract("KISSING", true)
            );
    }

    protected Node CopOrient(GameObject Cop, GameObject Player)
    {
        Val<Vector3> player = Val.V(() => Player.transform.position);
        return new SequenceParallel(Cop.GetComponent<BehaviorMecanim>().Node_OrientTowards(player),
                                 Cop.GetComponent<BehaviorMecanim>().Node_HandAnimation("PISTOLAIM", true));
    }

    protected RunStatus LostGame()
    {
        Time.timeScale = 0.0f;
        behaviorAgent.StopBehavior();
        return RunStatus.Success;
    }

    protected Node MonsterWander(GameObject Player, GameObject Monster, Transform WanderBegin, Transform WanderEnd, monsterCloseDetector monsterDetector)
    {
        Val<Vector3> begin = Val.V(() => WanderBegin.position);
        Val<Vector3> end = Val.V(() => WanderEnd.position);
        return new Sequence(
                    new Race(
                        new DecoratorLoop(
                            new Sequence(Monster.GetComponent<BehaviorMecanim>().Node_GoTo(begin), new LeafWait(1000),
                                 Monster.GetComponent<BehaviorMecanim>().Node_GoTo(end), new LeafWait(1000))),
                        new Sequence(new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(monsterDetector.monsterCloseToHero)))))),
                            new LeafWait(500))),
                    //new LeafInvoke((Func<RunStatus>)Controls.DisableControls),
                    new Sequence(
                          //new Sequence(new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                          //    new LeafInvoke((Func<RunStatus>)TextBox.CaughtDialog),
                          //    new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished)))))),
                          //    new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                          //    new LeafInvoke((Func<RunStatus>)LostGame)),
                          //Player.GetComponent<BehaviorMecanim>().Node_OrientTowards(Monster.transform.position),
                          //new LeafWait(500),
                          Player.GetComponent<BehaviorMecanim>().Node_BodyAnimationKillMonsterWithoutInteract("KILL_MONSTER", true),
                          Monster.GetComponent<BehaviorMecanim>().Node_BodyAnimationMonsterWithoutInteract("DYING", true)
                        ));
    }

    protected Node MonsterWander2(GameObject Player, GameObject Monster, Transform WanderBegin, Transform WanderEnd, monsterCloseDetector monsterDetector)
    {
        Val<Vector3> begin = Val.V(() => WanderBegin.position);
        Val<Vector3> end = Val.V(() => WanderEnd.position);
        return new Sequence(
                    new Race(
                        new DecoratorLoop(
                            new Sequence(Monster.GetComponent<BehaviorMecanim>().Node_GoTo(begin), new LeafWait(1000),
                                 Monster.GetComponent<BehaviorMecanim>().Node_GoTo(end), new LeafWait(1000))),
                        new Sequence(new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(monsterDetector.monsterCloseToHero)))))),
                            new LeafWait(500))),
                    //new LeafInvoke((Func<RunStatus>)Controls.DisableControls),
                    new Sequence(
                          //new Sequence(new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                          //    new LeafInvoke((Func<RunStatus>)TextBox.CaughtDialog),
                          //    new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished)))))),
                          //    new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                          //    new LeafInvoke((Func<RunStatus>)LostGame)),
                          //Player.GetComponent<BehaviorMecanim>().Node_OrientTowards(Monster.transform.position),
                          //new LeafWait(500),
                          Monster.GetComponent<BehaviorMecanim>().Node_BodyAnimationKillMonsterWithoutInteract("KILL_MONSTER", true),
                          Player.GetComponent<BehaviorMecanim>().Node_BodyAnimationMonsterWithoutInteract("DYING", true)
                        ));
    }

    protected Node InitialConversationTree()
    {
        return new DecoratorLoop(
            new DecoratorForceStatus(RunStatus.Failure, InitialConversation(Player, NPC)));
    }

    protected Node InitialConversationTree2()
    {
        return new DecoratorLoop(
            new DecoratorForceStatus(RunStatus.Failure, InitialConversation2(Player, NPC)));
    }

    protected Node BehaviorTree()
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f)
        {
            return new DecoratorLoop(
            new SequenceParallel(
                InitialConversationTree(),
                MonsterWander(Player, Monster1, Wander1Start, Wander1End, monsterDetector1),
                MonsterWander(Player, Monster2, Wander2Start, Wander2End, monsterDetector2),
                MonsterWander(Player, Monster3, Wander3Start, Wander3End, monsterDetector3)
                ));
        }
        return new DecoratorLoop(
            new SequenceParallel(
                InitialConversationTree2(),
                MonsterWander2(Player, Monster1, Wander1Start, Wander1End, monsterDetector1),
                MonsterWander2(Player, Monster2, Wander2Start, Wander2End, monsterDetector2),
                MonsterWander2(Player, Monster3, Wander3Start, Wander3End, monsterDetector3)
                ));
    }

}

