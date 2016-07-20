using UnityEngine;
using System.Collections;

public class SfxHandler : MonoBehaviour {

    public static SfxHandler Instance;

    public AudioClip[] clipHolder;

    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        // mark code
        // single audio source
        // load sound effects with resources.load once in Awake
        gameObject.AddComponent<AudioSource>();

        clipHolder = new AudioClip[3];
        clipHolder[0] = Resources.Load("SoundFX/pause") as AudioClip;
        clipHolder[1] = Resources.Load("SoundFX/ascending_tier") as AudioClip;
        clipHolder[2] = Resources.Load("SoundFX/descending_tier") as AudioClip;
    }

    // Use this for initialization
    void Start () {
    }

    public void playAudio(int clipNum)
    {
        AudioSource soundEffects = gameObject.GetComponent<AudioSource>();
        soundEffects.PlayOneShot(clipHolder[clipNum]);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
