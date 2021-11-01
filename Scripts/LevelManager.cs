using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; set; }
    public  bool showCollider = true; // $$

    //Level Spawning
    private const float Distance_Before_Spawn = 100f;
    private const int initial_Segment = 10;
    private const int initial_Transition_Segment = 2;
    [SerializeField] private const int Max_Segment_onScreen = 15;
    private Transform cameraContainer;
   [SerializeField] private int amountOfActiveSegment;
    private int continiousSegment;
    private int currentSpawnZ;
    private int currentlevel;
    private int y1,y2,y3;


    //List of pieces
    public List<Piece> ramp = new List<Piece>();
    public List<Piece> longBlocks = new List<Piece>();
    public List<Piece> jump = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    public List<Piece> pieces = new List<Piece>(); // all the pieces in the pool

    //list of segment
    public List<Segment> availableSegment = new List<Segment>();
    public List<Segment> availableTransition = new List<Segment>();
    public List<Segment> segments = new List<Segment>();

    //gameplay
    private bool isMoving;

    private void Awake()
    {
        instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        currentlevel = 0;
    }

    private void Start()
    {
        for (int i = 0; i < initial_Segment; i++)
            if (i < initial_Transition_Segment)
            {
                ShowTransitionSegment();
            }
            else
            {

                 GenerateSegment();
            }
    }


    private void Update()
    {
        if(currentSpawnZ  - cameraContainer.position.z < Distance_Before_Spawn)
        {
            GenerateSegment();
        }

        if(amountOfActiveSegment >= Max_Segment_onScreen)
        {
    
            segments[amountOfActiveSegment - 1].DeSpawn();
        }
    }
    private void GenerateSegment()
    {
        ShowSegment();

        if(UnityEngine.Random.Range(0f, 1f ) < (continiousSegment * .25f))
        {
            //show transition segment 
            continiousSegment = 0;
            ShowTransitionSegment();
        }
        else
        {

            continiousSegment++;
        }
    }

    private void ShowTransitionSegment()
    {
        List<Segment> possibleTransition = availableTransition.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);

        int id = UnityEngine.Random.Range(0, possibleTransition.Count);

        Segment s = GetSegment(id, true);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegment++;
        s.Spawn();
    }

    private void ShowSegment()
    {
        List<Segment> possibleSegment = availableSegment.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);

        int id = UnityEngine.Random.Range(0, possibleSegment.Count);

        Segment s = GetSegment(id, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegment++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if(s == null)
        {
            GameObject go = Instantiate((transition) ? availableTransition[id].gameObject : availableSegment[id].gameObject) as GameObject;
            s = go.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;

            segments.Insert(0, s);
        }
        else
        {
            segments.Remove(s);
            segments.Insert(0, s);
        }

        return s;
    }

    public Piece GetPiece(PieceType pt, int visualIndex)
    {

        Piece p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if(p == null)
        {
            GameObject go = null;

            if(pt == PieceType.ramp)
            {
                go = ramp[visualIndex].gameObject;
            }

          else if (pt == PieceType.longBlock)
            {
                go = longBlocks[visualIndex].gameObject;
            }

            else if (pt == PieceType.jump)
            {
                go = jump[visualIndex].gameObject;
            }

            else if (pt == PieceType.slide)
            {
                go = slides[visualIndex].gameObject;
            }
          
            GameObject GO = Instantiate(go) as GameObject;
            p = GO.GetComponent<Piece>();
            pieces.Add(p);
            
        }

        return p;
    }
}
