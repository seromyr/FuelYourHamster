﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpeedUp : MonoBehaviour
{
    private Text countDownText;
    private ParticleSystem speedUpTrail;

    private void Awake()
    {
        countDownText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(CoutDownSequence());
        speedUpTrail = FindObjectOfType<ParticleSystem>();
    }

    private IEnumerator CoutDownSequence()
    {
        countDownText.text = "Speed up in";
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "3";
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "2";
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "1";
        yield return new WaitForSeconds(0.5f);
        Player.main.PlaySound(SoundController.main.SoundLibrary[11]);
        speedUpTrail.Play();
        gameObject.SetActive(false);
    }
}
