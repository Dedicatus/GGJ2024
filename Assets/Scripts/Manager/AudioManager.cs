using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
	public AudioSource BackgroundMusic;
	public AudioSource testEffectMusic;

    protected override void Awake()
	{
		base.Awake();
		BackgroundMusic.ignoreListenerPause = true;
	}

	void Start()
	{
		BackgroundMusic.Play();
	}

	// Call this method to switch to the effect sound
	public void PlayTestEffectSound()
	{
		testEffectMusic.Play();
	}
}
