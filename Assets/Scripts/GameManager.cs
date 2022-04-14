using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Summary: Manage player hp and audio

    //For audio
    private AudioSource sfxAudioSource;
    private AudioSource bgmAudioSource;
    public GameObject mainCamera;
    public Slider bgmVolumnSlider;
    public Slider sfxVolumnSlider;
    public AudioClip sfxJump;
    public AudioClip sfxCook;
    public AudioClip sfxDamage;
    public AudioClip sfxEat;
    public AudioClip sfxGameClear;
    public AudioClip sfxGameOver;

    //For game ending
    private BackgroundController scriptBackgroundController;
    private float nextHpDecreaseTime;
    private float hpDecreaseTimeGap;
    public static float maxHp = 100f;
    public bool isGameEnd;
    public bool isEndOfRoad;
    public bool isGameSuccess;
    public float currentHp;
    public GameObject objBackRoad;
    public GameObject objSignBoard;
    public Image imgHpFill;
    public TextMeshProUGUI txtHp;
    public GameObject objGameClear;

    public void ShowGameResult()
    {
        //Show game clear on success
        if (isGameSuccess)
        {
            Debug.Log("Game Clear!");
            //PlaySound("GAMECLEAR");
            objGameClear.SetActive(true);
        }
        //Show game over on fail
        else
        {
            Debug.Log("Game Over!");
        }
    }
    private void ControlAudioVolumn()
    {
        //Update audio volumn
        bgmAudioSource.volume = bgmVolumnSlider.value;
        sfxAudioSource.volume = sfxVolumnSlider.value;

        //Save audio volumn
        PlayerPrefs.SetFloat("bgmVolumn", bgmVolumnSlider.value);
        PlayerPrefs.SetFloat("sfxVolumn", sfxVolumnSlider.value);
    }

    private void DecreaseHp()
    {
        //Decrease hp continuously
        if (Time.time > nextHpDecreaseTime)
        {
            nextHpDecreaseTime = Time.time + hpDecreaseTimeGap;
            currentHp -= 0.01f;
        }
    }

    private void Awake()
    {
        //Initialize variables
        currentHp = 100f;
        nextHpDecreaseTime = 0.0f;
        hpDecreaseTimeGap = 0.01f;
        bgmVolumnSlider.value = 1f;
        sfxVolumnSlider.value = 1f;
        isGameEnd = false;
        isEndOfRoad = false;
        isGameSuccess = false;
    }

    private void Start()
    {
        objSignBoard.SetActive(false);
        objGameClear.SetActive(false);
        sfxAudioSource = GetComponent<AudioSource>();
        bgmAudioSource = mainCamera.GetComponent<AudioSource>();
        scriptBackgroundController = objBackRoad.GetComponent<BackgroundController>();

        bgmVolumnSlider.value = PlayerPrefs.GetFloat("bgmVolumn", 1f);
        sfxVolumnSlider.value = PlayerPrefs.GetFloat("sfxVolumn", 1f);
        bgmAudioSource.volume = bgmVolumnSlider.value;
        sfxAudioSource.volume = sfxVolumnSlider.value;
    }

    private void Update()
    {
        ControlAudioVolumn();

        //Display hp
        txtHp.text = currentHp.ToString();
        imgHpFill.fillAmount = (currentHp / maxHp);

        if (!isGameEnd)
        {
            DecreaseHp();
            
            //Game over if zero hp
            if (currentHp <= 0)
            {
                isGameEnd = true;
                ShowGameResult();
            }

            //Show signboard(end point) if game end is near
            if (scriptBackgroundController.scrollCount == 49)
            {
                objSignBoard.SetActive(true);
                objSignBoard.transform.position += Vector3.left * 4 * Time.deltaTime;
            }

            //End game if player walked enough distance
            if (scriptBackgroundController.scrollCount == 50)
            {
                isGameEnd = true;
                isEndOfRoad = true;
            }
        }
    }

    public void PlaySound(string soundName)
    {
        //Play sfx once
        switch (soundName)
        {
            case "JUMP":
                sfxAudioSource.PlayOneShot(sfxJump);
                break;
            case "COOK":
                sfxAudioSource.PlayOneShot(sfxCook);
                break;
            case "EAT":
                sfxAudioSource.PlayOneShot(sfxEat);
                break;
            case "DAMAGE":
                sfxAudioSource.PlayOneShot(sfxDamage);
                break;
            case "GAMEOVER":
                sfxAudioSource.PlayOneShot(sfxGameOver);
                break;
            case "GAMECLEAR":
                sfxAudioSource.PlayOneShot(sfxGameClear);
                break;
        }
    }

    public void AdminChangeCurrentScore()
    {
        //Admin option
    }
}
