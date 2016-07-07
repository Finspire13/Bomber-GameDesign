using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {

	public static AudioPlayer instance = null;

	public AudioSource BGMSource;
	public AudioSource SFXSource;

	public AudioClip idleBGM;
	public AudioClip bgm1;
	public AudioClip bgm2;
	public AudioClip bgm3;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);

		BGMSource.loop = true;
		BGMSource.playOnAwake = false;
		BGMSource.Pause ();

		SFXSource.loop = false;
		SFXSource.playOnAwake = false;
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
