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
    public int tripletChance;
    public int chance;

    [Header("Gameplay")]
    public int bonks;
    public int[] rngChain;
    bool isChaineable;
    int fase;


    [Header("Sound Manager")]
    public GameObject[] ar_bonkSnd;
    public int rngAudio;

    void Start() {
        tripletChance = 10;
        fase = 0;
        rng = new int[4];
        isChaineable = false; ;
        bonks = 0;
        player = GameObject.Find("player");
        state = 0;
        isAdviced = false;
        isDeployed = false;
        isTouched = false;
        rng[0] = 0;
        rng[1] = 0;
        rng[2] = 0;
        rng[3] = 0;
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
        isTouched = ar_pistons[rng[0]].GetComponent<src_pistonreset>().isTouched;

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
    void Advice()
    {
        if (!isChaineable) {
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
        if (isChaineable) {
            Compare();  
        }

        Debug.Log("Advised");
    }
    private void Compare() {
        RNGMaker();

        if(fase == 0) {
            //Posibilidades con arriba
            //Arriba + Derecha
            if ((rng[0] == 0 && rng[1] == 1) || (rng[0] == 1 && rng[1] == 0))
            {
                spriteRenderAdviseUp.sprite = adviseSpr;
                spriteRenderAdviseRight.sprite = adviseSpr;
            }
            //Arriba + Abajo
            else if ((rng[0] == 0 && rng[1] == 2) || (rng[0] == 2 && rng[1] == 0))
            {
                spriteRenderAdviseDown.sprite = adviseSpr;
                spriteRenderAdviseUp.sprite = adviseSpr;
            }
            //Arriba + Izquierda
            else if ((rng[0] == 0 && rng[1] == 3) || (rng[0] == 3 && rng[1] == 0))
            {
                spriteRenderAdviseLeft.sprite = adviseSpr;
                spriteRenderAdviseUp.sprite = adviseSpr;
            }
            //Posibilidades con derecha
            //Derecha + Abajo
            else if ((rng[0] == 1 && rng[1] == 2) || (rng[0] == 2 && rng[1] == 1))
            {
                spriteRenderAdviseDown.sprite = adviseSpr;
                spriteRenderAdviseRight.sprite = adviseSpr;
            }
            //Derecha + Izquierda
            else if ((rng[0] == 1 && rng[1] == 3) || (rng[0] == 3 && rng[1] == 1))
            {
                spriteRenderAdviseRight.sprite = adviseSpr;
                spriteRenderAdviseLeft.sprite = adviseSpr;
            }
            //Posibilidad con Abajo
            //Abajo + Izquierda
            else if ((rng[0] == 2 && rng[1] == 3) || (rng[0] == 3 && rng[1] == 2))
            {
                spriteRenderAdviseDown.sprite = adviseSpr;
                spriteRenderAdviseLeft.sprite = adviseSpr;
            }
        }else if (fase == 1) {
            //Arriba + Derecha + Abajo
            if ((rng[0] == 0 && rng[1] == 1 && rng[2] == 2) || (rng[0] == 0 && rng[1] == 2 && rng[2] == 1) || (rng[0] == 1 && rng[1] == 0 && rng[2] == 2) || (rng[0] == 1 && rng[1] == 2 && rng[2] == 0) || (rng[0] == 2 && rng[1] == 0 && rng[2] == 1) || (rng[0] == 2 && rng[1] == 1 && rng[2] == 0)) {
                spriteRenderAdviseUp.sprite = adviseSpr;
                spriteRenderAdviseRight.sprite = adviseSpr;
                spriteRenderAdviseDown.sprite = adviseSpr;
            }
            //Arriba + Abajo + Izquierda
            else if ((rng[0] == 0 && rng[1] == 2 && rng[2] == 3) || (rng[0] == 0 && rng[1] == 3 && rng[2] == 2) || (rng[0] == 2 && rng[1] == 0 && rng[2] == 3) || (rng[0] == 2 && rng[1] == 3 && rng[2] == 0) || (rng[0] == 3 && rng[1] == 0 && rng[2] == 2) || (rng[0] == 3 && rng[1] == 2 && rng[2] == 0))
            {
                spriteRenderAdviseLeft.sprite = adviseSpr;
                spriteRenderAdviseUp.sprite = adviseSpr;
                spriteRenderAdviseDown.sprite = adviseSpr;
            }
            //Arriba + Derecha + Izquierda
            else if ((rng[0] == 0 && rng[1] == 1 && rng[2] == 3) || (rng[0] == 0 && rng[1] == 3 && rng[2] == 1) || (rng[0] == 1 && rng[1] == 0 && rng[2] == 3) || (rng[0] == 1 && rng[1] == 3 && rng[2] == 0) || (rng[0] == 3 && rng[1] == 0 && rng[2] == 1) || (rng[0] == 3 && rng[1] == 1 && rng[2] == 0))
            {
                spriteRenderAdviseLeft.sprite = adviseSpr;
                spriteRenderAdviseRight.sprite = adviseSpr;
                spriteRenderAdviseUp.sprite = adviseSpr;
            }
            //Derecha + Abajo + Izquierda
            else if ((rng[0] == 2 && rng[1] == 1 && rng[2] == 3) || (rng[0] == 2 && rng[1] == 3 && rng[2] == 1) || (rng[0] == 1 && rng[1] == 2 && rng[2] == 3) || (rng[0] == 1 && rng[1] == 3 && rng[2] == 2) || (rng[0] == 3 && rng[1] == 2 && rng[2] == 1) || (rng[0] == 3 && rng[1] == 1 && rng[2] == 2))
            {
                spriteRenderAdviseLeft.sprite = adviseSpr;
                spriteRenderAdviseRight.sprite = adviseSpr;
                spriteRenderAdviseDown.sprite = adviseSpr;
            }

        }



    }

    void Deployed() {
        if(!isChaineable) {
            if (rng[0] == 0)
            {
                spriteRenderAdviseUp.sprite = blackSpr;
                piston_up.Play("piston_up_push", 0, 0.0f);
            }
            else if (rng[0] == 1)
            {
                spriteRenderAdviseRight.sprite = blackSpr;
                piston_right.Play("piston_right_push", 0, 0.0f);
            }
            else if (rng[0] == 2)
            {
                spriteRenderAdviseDown.sprite = blackSpr;
                piston_down.Play("piston_down_push", 0, 0.0f);
            }
            else if (rng[0] == 3)
            {
                spriteRenderAdviseLeft.sprite = blackSpr;
                piston_left.Play("piston_left_push", 0, 0.0f);
            }
        }
        else if (isChaineable) {

            if (fase == 0) {
                //Arriba + Derecha
                if ((rng[0] == 0 && rng[1] == 1) || (rng[0] == 1 && rng[1] == 0))
                {
                    spriteRenderAdviseUp.sprite = blackSpr;
                    spriteRenderAdviseRight.sprite = blackSpr;
                    piston_up.Play("piston_up_push", 0, 0.0f);
                    piston_right.Play("piston_right_push", 0, 0.0f);
                }
                //Arriba + Abajo
                else if ((rng[0] == 0 && rng[1] == 2) || (rng[0] == 2 && rng[1] == 0))
                {
                    spriteRenderAdviseDown.sprite = blackSpr;
                    spriteRenderAdviseUp.sprite = blackSpr;
                    piston_up.Play("piston_up_push", 0, 0.0f);
                    piston_down.Play("piston_down_push", 0, 0.0f);
                }
                //Arriba + Izquierda
                else if ((rng[0] == 0 && rng[1] == 3) || (rng[0] == 3 && rng[1] == 0))
                {
                    spriteRenderAdviseLeft.sprite = blackSpr;
                    spriteRenderAdviseUp.sprite = blackSpr;
                    piston_up.Play("piston_up_push", 0, 0.0f);
                    piston_left.Play("piston_left_push", 0, 0.0f);
                }
                //Posibilidades con derecha
                //Derecha + Abajo
                else if ((rng[0] == 1 && rng[1] == 2) || (rng[0] == 2 && rng[1] == 1))
                {
                    spriteRenderAdviseDown.sprite = blackSpr;
                    spriteRenderAdviseRight.sprite = blackSpr;
                    piston_right.Play("piston_right_push", 0, 0.0f);
                    piston_down.Play("piston_down_push", 0, 0.0f);
                }
                //Derecha + Izquierda
                else if ((rng[0] == 1 && rng[1] == 3) || (rng[0] == 3 && rng[1] == 1))
                {
                    spriteRenderAdviseRight.sprite = blackSpr;
                    spriteRenderAdviseLeft.sprite = blackSpr;
                    piston_right.Play("piston_right_push", 0, 0.0f);
                    piston_left.Play("piston_left_push", 0, 0.0f);
                }
                //Posibilidad con Abajo
                //Abajo + Izquierda
                else if ((rng[0] == 2 && rng[1] == 3) || (rng[0] == 3 && rng[1] == 2))
                {
                    spriteRenderAdviseDown.sprite = blackSpr;
                    spriteRenderAdviseLeft.sprite = blackSpr;
                    piston_down.Play("piston_down_push", 0, 0.0f);
                    piston_left.Play("piston_left_push", 0, 0.0f);
                }
            }
            else if (fase == 1) {
                //Arriba + Derecha + Abajo
                if ((rng[0] == 0 && rng[1] == 1 && rng[2] == 2) || (rng[0] == 0 && rng[1] == 2 && rng[2] == 1) || (rng[0] == 1 && rng[1] == 0 && rng[2] == 2) || (rng[0] == 1 && rng[1] == 2 && rng[2] == 0) || (rng[0] == 2 && rng[1] == 0 && rng[2] == 1) || (rng[0] == 2 && rng[1] == 1 && rng[2] == 0)) {
                    spriteRenderAdviseUp.sprite = blackSpr;
                    spriteRenderAdviseRight.sprite = blackSpr;
                    spriteRenderAdviseDown.sprite = blackSpr;
                    piston_up.Play("piston_up_push",0,0.0f);
                    piston_right.Play("piston_right_push", 0, 0.0f);
                    piston_down.Play("piston_down_push",0, 0.0f);
                }
                //Arriba + Abajo + Izquierda
                else if ((rng[0] == 0 && rng[1] == 2 && rng[2] == 3) || (rng[0] == 0 && rng[1] == 3 && rng[2] == 2) || (rng[0] == 2 && rng[1] == 0 && rng[2] == 3) || (rng[0] == 2 && rng[1] == 3 && rng[2] == 0) || (rng[0] == 3 && rng[1] == 0 && rng[2] == 2) || (rng[0] == 3 && rng[1] == 2 && rng[2] == 0))
                {
                    spriteRenderAdviseLeft.sprite = blackSpr;
                    spriteRenderAdviseUp.sprite = blackSpr;
                    spriteRenderAdviseDown.sprite = blackSpr;
                    piston_up.Play("piston_up_push", 0, 0.0f);
                    piston_down.Play("piston_down_push", 0, 0.0f);
                    piston_left.Play("piston_left_push", 0, 0.0f);
                }
                //Arriba + Derecha + Izquierda
                else if ((rng[0] == 0 && rng[1] == 1 && rng[2] == 3) || (rng[0] == 0 && rng[1] == 3 && rng[2] == 1) || (rng[0] == 1 && rng[1] == 0 && rng[2] == 3) || (rng[0] == 1 && rng[1] == 3 && rng[2] == 0) || (rng[0] == 3 && rng[1] == 0 && rng[2] == 1) || (rng[0] == 3 && rng[1] == 1 && rng[2] == 0))
                {
                    spriteRenderAdviseLeft.sprite = blackSpr;
                    spriteRenderAdviseRight.sprite = blackSpr;
                    spriteRenderAdviseUp.sprite = blackSpr;
                    piston_up.Play("piston_up_push", 0, 0.0f);
                    piston_right.Play("piston_right_push", 0, 0.0f);
                    piston_left.Play("piston_left_push", 0, 0.0f);
                }
                //Derecha + Abajo + Izquierda
                else if ((rng[0] == 2 && rng[1] == 1 && rng[2] == 3) || (rng[0] == 2 && rng[1] == 3 && rng[2] == 1) || (rng[0] == 1 && rng[1] == 2 && rng[2] == 3) || (rng[0] == 1 && rng[1] == 3 && rng[2] == 2) || (rng[0] == 3 && rng[1] == 2 && rng[2] == 1) || (rng[0] == 3 && rng[1] == 1 && rng[2] == 2))
                {
                    spriteRenderAdviseLeft.sprite = blackSpr;
                    spriteRenderAdviseRight.sprite = blackSpr;
                    spriteRenderAdviseDown.sprite = blackSpr;
                    piston_down.Play("piston_down_push", 0, 0.0f);
                    piston_right.Play("piston_right_push", 0, 0.0f);
                    piston_left.Play("piston_left_push", 0, 0.0f);
                }
            }
           
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
        isChaineable = true;
    }
    void RNGMaker() {
        chance = Random.Range(0, 101);
        if (tripletChance <= chance) {
            fase = 0;
            do {
                rng[0] = Random.Range(0, ar_pistons.Length);
                rng[1] = Random.Range(0, ar_pistons.Length);
            } while (rng[0] == rng[1] || rng[1] == rng[0]);
            chance += 5;
        }
        else {
            fase = 1;
            do
            {
                rng[0] = Random.Range(0, ar_pistons.Length);
                rng[1] = Random.Range(0, ar_pistons.Length);
                rng[2] = Random.Range(0, ar_pistons.Length);
            } while (rng[0] == rng[1] || rng[0] == rng[2] || rng[1] == rng[0] || rng[1] == rng[2] || rng[2] == rng[1] || rng[2] == rng[0]);
            chance -= 2;
        }
        
    }
}
