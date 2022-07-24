using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class src_pistonbrain : MonoBehaviour {
    [Header("Sprite Render Settings")]
    public SpriteRenderer spriteRenderAdviseUp;
    public SpriteRenderer spriteRenderAdviseRight;
    public SpriteRenderer spriteRenderAdviseDown;
    public SpriteRenderer spriteRenderAdviseLeft;

    [Header("Animation settings")]
    [SerializeField] private Animator piston_up = null;
    [SerializeField] private Animator piston_right = null;
    [SerializeField] private Animator piston_down = null;
    [SerializeField] private Animator piston_left = null;

    [Header("Sprites")]
    public Sprite adviseSpr;
    public Sprite blackSpr;

    [Header("Pistons array")]
    public GameObject[] ar_pistons;

    GameObject player;

    [Header("Timers")]
    public float timer;
    public float advice_timer;
    public float pop_timer;
    public float advice_reaction_time;
   
    [Header("Debug")]
    public int[] rng;
    public bool isStarted;
    public bool isTouched;
    public bool isAdviced;
    public bool isDeployed;
    public int state;

    [Header("Gameplay")]
    public int bonks;
    public int[] rngChain;
    bool isCorrect;
    bool isChaineable;


    [Header("Sound Manager")]
    public GameObject[] ar_bonkSnd;
    public int rngAudio;
    void Start() {
        rng = new int[4];
        isCorrect = false;
        isChaineable = false; ;
        bonks = 0;
        player = GameObject.Find("player");
        state = 0;
        isAdviced = false;
        isDeployed = false;
        isTouched = false;
        rng[0] = 0;
        pop_timer = 3f;
        advice_reaction_time = 2f;

        spriteRenderAdviseUp = ar_pistons[0].GetComponent<SpriteRenderer>();
        spriteRenderAdviseRight = ar_pistons[1].GetComponent<SpriteRenderer>();
        spriteRenderAdviseDown = ar_pistons[2].GetComponent<SpriteRenderer>();
        spriteRenderAdviseLeft = ar_pistons[3].GetComponent<SpriteRenderer>();
    }
    void Update() {

        isStarted = player.GetComponent<src_gemchara>().isStarted;

        //Posibilidad de concatenación
        //if(!isChaineable) {
            
            isTouched = ar_pistons[rng[0]].GetComponent<src_pistonreset>().isTouched;
       /* }
        else if(isChaineable) {
            while(!isCorrect) {

            }
            //rngChain = Random.Range(0, ar_pistons.Length);
        } */

        //Esperar
        if (isStarted && state == 0) {
            timer += Time.deltaTime;
            if(timer >= pop_timer) {state = 1;}
        }
        //Avisa
        if(state == 1 && !isAdviced) {
            Advice();
            state = 2;
            isAdviced = true;
        }
        //Aviso tiempo reaccion
        if(state == 2 && isAdviced) {
            advice_timer += Time.deltaTime;
            if(advice_timer >= advice_reaction_time) {state = 3;}  
        }
        //Deploying
        if(state == 3 && !isDeployed){
            Deployed();
            isDeployed = true;
        }
        //Reset
        if(state == 3 && isTouched) {
            Reset();
            state = 0;
        }

    }
    void Advice() {
        if(!isChaineable) {
            rng[0] = Random.Range(0, ar_pistons.Length);
            if (rng[0] == 0)
            {
                spriteRenderAdviseUp.sprite = adviseSpr;
            }
            else if (rng[0] == 1)
            {
                spriteRenderAdviseRight.sprite = adviseSpr;
            }
            else if (rng[0] == 2)
            {
                spriteRenderAdviseDown.sprite = adviseSpr;
            }
            else if (rng[0] == 3)
            {
                spriteRenderAdviseLeft.sprite = adviseSpr;
            }
        }
        else if(isChaineable) {

        }
        
        Debug.Log("Adavised");
    }
    void Deployed() {
        if (rng[0] == 0) {
            spriteRenderAdviseUp.sprite = blackSpr;
            piston_up.Play("piston_up_push", 0, 0.0f);
        }
        else if (rng[0] == 1) {
            spriteRenderAdviseRight.sprite = blackSpr;
            piston_right.Play("piston_right_push", 0, 0.0f);
        }
        else if (rng[0] == 2) {
            spriteRenderAdviseDown.sprite = blackSpr;
            piston_down.Play("piston_down_push",0,0.0f);
        }
        else if (rng[0] == 3) {
            spriteRenderAdviseLeft.sprite = blackSpr;
            piston_left.Play("piston_left_push", 0, 0.0f);
        }
        Debug.Log("Deployed!");
    }
    void Reset() {
        //Plays Audio
        rngAudio = Random.Range(0, ar_bonkSnd.Length);
        ar_bonkSnd[rngAudio].GetComponent<AudioSource>().Play();  
        timer = 0;
        advice_timer = 0;
        //Play animations
        piston_up.Play("piston_up_idle", 0, 0.0f);
        piston_right.Play("piston_right_idle", 0, 0.0f);
        piston_down.Play("piston_down_idle", 0, 0.0f);
        piston_left.Play("piston_left_idle", 0, 0.0f);
        //Increase bonk
        bonks++;
        //Checks difficulty
        Difficulty();

        isAdviced = false;
        isDeployed = false;
        Debug.Log("Reseted!");
    }

    void Difficulty() {
        pop_timer = 1f;
        if (pop_timer == 1f && advice_reaction_time >= 1.1f) {
            advice_reaction_time -= 0.2f;
        }
        if (bonks > 15) {
            isChaineable = true;
        }
    }
}
