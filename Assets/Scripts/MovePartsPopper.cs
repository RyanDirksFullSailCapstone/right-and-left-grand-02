using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;

public class MovePartsPopper : MonoBehaviour
{
    private List<MovePart> MyMovePartsQueue = new List<MovePart>();
    public int nextPart = -1;
    public int currentPart = -1;
    public int lastPartIndex;
    public bool isMoving;
    private SimpleSampleCharacterControl MyMover;

    public string lastCall;
    public string doingMovePart;

// Start is called before the first frame update
    void Start()
    {
        MyMover = gameObject.GetComponent<SimpleSampleCharacterControl>();

    }

    // Update is called once per frame
    void Update()
    {
        int LastPartIndex = MyMovePartsQueue.Count - 1;
        lastPartIndex = LastPartIndex;
        //if (gameObject.name == "Dancer1Right")
        //{
        //    Debug.Log($"lastpartindex:{LastPartIndex}");
        //}
        if (!(LastPartIndex < 0))
        {
            //if (gameObject.name == "Dancer1Right")
            //{
            //    Debug.Log($"nextpart:{nextPart} currentpart:{currentPart}");
            //}
            if (LastPartIndex > nextPart && currentPart == nextPart)
            {
                nextPart++;
            }

            isMoving = nextPart > currentPart;

            if (isMoving)                                                               
            {
                if (MyMover.IsReadyForNextMove())
                {
                    if (nextPart > currentPart)
                    {
                        currentPart++;
                        MovePart thisMovePart = MyMovePartsQueue[currentPart];
                        doingMovePart = thisMovePart.Name;
                        if (doingMovePart == "Reset The Square")
                        {
                            Floor thisFloor = GameObject.Find("floor").GetComponent<Floor>();
                            thisFloor.ResetSquare();
                        }
                        else
                        {
                            MyMover.CompleteCondition = thisMovePart.CompleteCondition;
                            MyMover.thisMovePart = doingMovePart;
                            MyMover.setFacingTarget(thisMovePart.Target);
                            MyMover.setTargetPosition(thisMovePart.Target);
                            MyMover.isFacing = thisMovePart.IsChangeRotationInPlace;
                            MyMover.isMoving = (!thisMovePart.IsBackingUp && thisMovePart.IsPositionChange);
                            MyMover.isMovingBackwards = thisMovePart.IsBackingUp;
                            MyMover.setMovingAs(thisMovePart.DoMovePartAs);
                            MyMover.leftHandPosition = thisMovePart.LeftHandPosition;
                            MyMover.leftHandTarget = thisMovePart.LeftHandTarget;
                            MyMover.doUpdateTargetPosition = thisMovePart.DoUpdateTargetPosition;
                            MyMover.targetGameObject = thisMovePart.TargetGameObject;
                        }
                    }
                    else
                        isMoving = false;
                }
            }
        }
    }
        
    private void OnEnable()
    {
        //Start listening for game events
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    private void OnDisable()
    {
        //Stop listening for game events
        Message.RemoveListener<GameEventMessage>(OnMessage);
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message == null) return;
        if (message.EventName.Contains("SquareDanceMove"))
            AddMovePartsFor(message.EventName.Replace("SquareDanceMove",""));
    }

    public void AddMovePartsFor(string squareDanceMoveName) { 
        lastCall = squareDanceMoveName;
        switch (squareDanceMoveName)
        {
            case "ALLEMANDE LEFT":
                //face corner
                MyMovePartsQueue.Add(new MovePart("face your corner", gameObject.GetComponent<Dancer>().Corner.transform.position, MoveAs.Dancer, false, false, true));
                // Motion Pinwheel around left forearm grip
                // walk around them 
                MyMovePartsQueue.Add(new MovePart("walk around a left forearm grip", gameObject.GetComponent<Dancer>().Corner.GetComponent<DancerTargets>().ForwardSpaceTarget.transform.position, MoveAs.Dancer, false, true, false, HandPosition.ForearmGrip, gameObject.GetComponent<Dancer>().Corner.GetComponent<Dancer>().LeftHandTarget.transform.position, gameObject.GetComponent<Dancer>().Corner.GetComponent<DancerTargets>().LeftSpaceTarget, true,CompleteConditionType.TargetMet));
                MyMovePartsQueue.Add(new MovePart("until you see your partner",Vector3.positiveInfinity, MoveAs.Dancer, false, true, false, HandPosition.ForearmGrip, gameObject.GetComponent<Dancer>().Corner.GetComponent<Dancer>().LeftHandTarget.transform.position, gameObject.GetComponent<Dancer>().Corner.GetComponent<DancerTargets>().BackwardSpaceTarget, true,CompleteConditionType.SeePartner));
                MyMovePartsQueue.Add(new MovePart("Go Home", gameObject.GetComponent<Dancer>().HomePosition.transform.position, MoveAs.Couple, false, true, false));
                MyMovePartsQueue.Add(new MovePart("Face in", gameObject.GetComponent<Dancer>().FacingInTarget.transform.position, MoveAs.Couple, false, false, true));
                MyMovePartsQueue.Add(new MovePart("Reset The Square", gameObject.GetComponent<DancerTargets>().BackwardSpaceTarget.transform.position, MoveAs.Couple, true, true, false));
                break;
            case "FACE CORNER":
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName,gameObject.GetComponent<Dancer>().Corner.transform.position,MoveAs.Dancer,false,false,true));
                break;
            case "FACE PARTNER":
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName,gameObject.GetComponent<Dancer>().Partner.transform.position, MoveAs.Dancer, false, false, true));
                break;
            case "FACE LEFT":
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName, gameObject.GetComponent<DancerTargets>().LeftSpaceTarget.transform.position, MoveAs.Dancer, false, false, true));
                break;
            case "FACE RIGHT":
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName, gameObject.GetComponent<DancerTargets>().RightSpaceTarget.transform.position, MoveAs.Dancer, false, false, true));
                break;
            case "FACE IN":
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName, gameObject.GetComponent<Dancer>().FacingInTarget.transform.position, MoveAs.Couple, false, false, true));
                break;
            case "SquareTheSet":
                MyMovePartsQueue.Add(new MovePart("Reset The Square", gameObject.GetComponent<Dancer>().HomePosition.transform.position, MoveAs.Couple, false, true, false));
                break;
            case "UP TO THE MIDDLE AND BACK":
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName, gameObject.GetComponent<DancerTargets>().ForwardSpaceTarget.transform.position, MoveAs.Couple, false, true, false));
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName, gameObject.GetComponent<DancerTargets>().ForwardSpaceTarget.transform.position, MoveAs.Couple, false, true, false));
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName, gameObject.GetComponent<DancerTargets>().BackwardSpaceTarget.transform.position, MoveAs.Couple, true, true, false));
                break;
            case "MOVE UP":
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName, gameObject.GetComponent<DancerTargets>().ForwardSpaceTarget.transform.position, MoveAs.Couple, false, true, false));
                break;
            case "MOVE BACK":
                MyMovePartsQueue.Add(new MovePart(squareDanceMoveName, gameObject.GetComponent<DancerTargets>().BackwardSpaceTarget.transform.position, MoveAs.Couple, true,true, false));
                break;
            case "RESET":
                MyMovePartsQueue.Add(new MovePart("Reset The Square", gameObject.GetComponent<Dancer>().HomePosition.transform.position, MoveAs.Couple, false, true, false));
                break;
            default:
                Debug.Log($"Move Parts Popper - No association for {squareDanceMoveName}");
                break;
        }
    }
}
