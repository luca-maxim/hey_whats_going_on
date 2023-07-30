using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    private void Start()
    {
        //Umgebungsgeräusche starten
        Play("CitySound");
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    
    public void Play(string name)
    {
        if (GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().isAudioManagerActivated == true 
            || name == "AudioansageAn"  
            || name == "AudioansageAus"
            || GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().isKeyPressed == true)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            foreach (Sound sound in sounds)
            {
                if (sound.name != "CitySound" && sound.name != "CarSound")
                    sound.source.Stop();
            }

            s.source.Play();
        }
    }
}
