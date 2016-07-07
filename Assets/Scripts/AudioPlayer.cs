using UnityEngine;
using System.Collections;

public enum SFX
{
	Explosion,Buff,PlayerDamaged,EnermyDamaged,Debuff,PlayerKilled,EnermyKilled,SetBomb,Start,Victory
}

public class AudioPlayer : MonoBehaviour {

	public static AudioPlayer instance = null;

	public AudioSource BGMSource;
	public AudioSource SFXSource;

	public AudioClip idleBGM;
	public AudioClip bgm1;
	public AudioClip bgm2;
	public AudioClip bgm3;

	public AudioClip explosion;
	public AudioClip buff;
	public AudioClip playerDamaged;
	public AudioClip enermyDamaged;
	public AudioClip debuff;
	public AudioClip playerKilled;
	public AudioClip enermyKilled;
	public AudioClip setBomb;
	public AudioClip start;
	public AudioClip victory;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);

		BGMSource.loop = true;
		BGMSource.volume = 0.55f;
		BGMSource.playOnAwake = false;
		BGMSource.Pause ();

		SFXSource.loop = false;
		SFXSource.playOnAwake = false;
		SFXSource.volume = 1f;
		SFXSource.Pause ();
	}
	// Use this for initialization
	void Start () {
		
	}

	public void playBGM(int bgmIndex)
	{
		switch (bgmIndex) {
		case 0:
			BGMSource.Pause ();
			BGMSource.clip = idleBGM;
			BGMSource.Play ();
			break;
		case 1:
			BGMSource.Pause ();
			BGMSource.clip = bgm1;
			BGMSource.Play ();
			break;
		case 2:
			BGMSource.Pause ();
			BGMSource.clip = bgm2;
			BGMSource.Play ();
			break;
		case 3:
			BGMSource.Pause ();
			BGMSource.clip = bgm3;
			BGMSource.Play ();
			break;
		default:
			break;
		}
	}

	public void stopBGM()
	{
		BGMSource.Pause ();
	}

	public void playSFX(SFX sfx)
	{
		switch (sfx) {
		case SFX.Buff:
			SFXSource.Pause ();
			SFXSource.volume = 0.6f;
			SFXSource.clip = buff;
			SFXSource.Play ();
			SFXSource.volume = 1f;
			break;
		case SFX.Debuff:
			SFXSource.Pause ();
			SFXSource.clip = debuff;
			SFXSource.Play ();
			break;
		case SFX.EnermyDamaged:
			SFXSource.Pause ();
			SFXSource.clip = enermyDamaged;
			SFXSource.Play ();
			break;
		case SFX.EnermyKilled:
			SFXSource.Pause ();
			SFXSource.clip = enermyKilled;
			SFXSource.Play ();
			break;
		case SFX.Explosion:
			SFXSource.Pause ();
			SFXSource.volume = 0.9f;
			SFXSource.clip = explosion;
			SFXSource.Play ();
			SFXSource.volume = 1f;
			break;
		case SFX.PlayerDamaged:
			SFXSource.Pause ();
			SFXSource.clip = playerDamaged;
			SFXSource.Play ();
			break;
		case SFX.PlayerKilled:
			SFXSource.Pause ();
			SFXSource.clip = playerKilled;
			SFXSource.Play ();
			break;
		case SFX.SetBomb:
			SFXSource.Pause ();
			SFXSource.clip = setBomb;
			SFXSource.Play ();
			break;
		case SFX.Victory:
			SFXSource.Pause ();
			SFXSource.clip = victory;
			SFXSource.Play ();
			break;
		case SFX.Start:
			SFXSource.Pause ();
			SFXSource.clip = start;
			SFXSource.Play ();
			break;
		default:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
