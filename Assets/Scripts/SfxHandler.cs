using UnityEngine;
using System.Collections;

public class SfxHandler : MonoBehaviour {

    public static SfxHandler Instance;

    public AudioClip[] clipHolder;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        // mark code
        // single audio source
        // load sound effects with resources.load once in Awake
        gameObject.AddComponent<AudioSource>();

        clipHolder = new AudioClip[9];
        clipHolder[0] = Resources.Load("SoundFX/pause") as AudioClip;
        clipHolder[1] = Resources.Load("SoundFX/ascending_tier") as AudioClip;
        clipHolder[2] = Resources.Load("SoundFX/descending_tier") as AudioClip;
        clipHolder[3] = Resources.Load("SoundFX/duckling_pickup") as AudioClip;
        clipHolder[4] = Resources.Load("SoundFX/duckling_death") as AudioClip;
        clipHolder[5] = Resources.Load("SoundFX/duck_zapped") as AudioClip;
        clipHolder[6] = Resources.Load("SoundFX/level_completed") as AudioClip;
        clipHolder[7] = Resources.Load("SoundFX/current_water") as AudioClip;
        clipHolder[8] = Resources.Load("SoundFX/geyser_effect") as AudioClip;

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
