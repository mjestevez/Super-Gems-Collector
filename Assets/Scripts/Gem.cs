using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GemType
{
    Copper,Gold,Diamond
}
public class Gem : MonoBehaviour
{
    public bool multiplayer;
    public GemType type;
    public int value;
    public Sprite[] sprites;
    private SpriteRenderer sr;
    private CircleCollider2D cc;
    public bool cooldown=false;
    private float cooldownTime;
    private float counter=0f;
    private AudioSource audioSr;

    private void Start()
    {
        audioSr = GetComponentInChildren<AudioSource>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cc = GetComponent<CircleCollider2D>();
        SetValue();
        transform.DOMoveY(-0.5f, 1f).SetLoops(-1,LoopType.Yoyo).SetRelative().SetEase(Ease.InOutQuad).Play();
        transform.DOScale(new Vector3(1.5f,1.5f,1),1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).Play();
    }

    private void Update()
    {
        if (cooldown)
        {
            if (counter >= cooldownTime)
            {
                Activate();
                counter = 0;
                
            }else counter += Time.deltaTime;
        }
    }

    private void SetValue()
    {
        switch (type)
        {
            case GemType.Copper:
                value = 1;
                break;
            case GemType.Gold:
                value = 3;
                break;
            case GemType.Diamond:
                value = 5;
                break;
        }
        sr.sprite = sprites[(int)type];
    }

    private void Cooldown()
    {
        if (!multiplayer)
            cooldownTime = Random.Range(2, 5);
        else
            cooldownTime = 5f;
        
        cooldown = true;
        sr.enabled = false;
        cc.enabled = false;
    }

    private void Activate()
    {
        if(!multiplayer)
        {
            cooldown = false;
            int n = Random.Range(0, 101);
            if (n <= 50) type = GemType.Copper;
            else if (n <= 90) type = GemType.Gold;
            else type = GemType.Diamond;
            SetValue();
            sr.enabled = true;
            cc.enabled = true;
        }
        else
        {
            cooldown = false;
            sr.enabled = true;
            cc.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!multiplayer)
        {
            if (collision.gameObject.tag == "Player 1")
            {
                GameManager.instance.AddPlayer1Value(value);
            }
            else if (collision.gameObject.tag == "Player 2")
            {
                GameManager.instance.AddPlayer2Value(value);
            }
            else if (collision.gameObject.tag == "IA")
            {
                GameManager.instance.AddPlayer1Value(value);
            }
            else if (collision.gameObject.tag == "IA2")
            {
                GameManager.instance.AddPlayer2Value(value);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player 1")
            {
                GameManagerMultiplayer.instance.AddPlayer1Value(value);
            }
            else if (collision.gameObject.tag == "Player 2")
            {
                GameManagerMultiplayer.instance.AddPlayer2Value(value);
            }
            else if (collision.gameObject.tag == "IA")
            {
                GameManagerMultiplayer.instance.AddPlayer1Value(value);
            }
            else if (collision.gameObject.tag == "IA2")
            {
                GameManagerMultiplayer.instance.AddPlayer2Value(value);
            }
        }
        
        audioSr.Play();
        Cooldown();
    }
}
