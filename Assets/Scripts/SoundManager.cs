using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;
    public List<AudioClip> soundEffects;
    public GameObject sfxPrefab, musicPrefab;
    private Dictionary<string, AudioClip> sfxDict;
	// Use this for initialization
	void Awake () {
	    if(Instance == null)
        {
            Instance = null;

        }
        else
        {
            Destroy(this);
            return;
        }
	}

    void Start()
    {
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip aud in soundEffects)
        {
            string name = aud.name;
            sfxDict.Add(name, aud);
        }
        GameObject musicObject = Instantiate(musicPrefab, Vector3.zero, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [PunRPC]
    public void PlaySFX(string name, int volume)
    {
        GameObject sfx = Instantiate(sfxPrefab, Vector3.zero, Quaternion.identity);
        AudioSource src = sfx.GetComponent<AudioSource>();
        AudioClip clip;
        if (sfxDict.ContainsKey(name)) {
            clip = sfxDict[name];
            src.volume = ((float)volume / 100f);
            src.PlayOneShot(clip);
            Destroy(sfx, clip.length);
        }
        else
        {
            Debug.LogWarning("Unable to locate sfx: " + name);
        }
    }
}
