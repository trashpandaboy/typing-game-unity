using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.trashpandaboy.core;
using com.trashpandaboy.core.Utils;
using UnityEngine;
using static Utils;

public class SfxManager : MonoBehaviour
{
    [SerializeField]
    List<SfxEvent> _sfxEvents;

    [Serializable]
    public class SfxEvent
    {
        [SerializeField]
        public SfxSound name;
        [SerializeField]
        public AudioClip clip;
    }

    AudioSource _source;

    #region Unity

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EventDispatcher.StartListening(GameEvent.PlaySound.ToString(), OnPlaySound);
    }

    #endregion

    private void OnPlaySound(DataSet data)
    {
        string soundName = data.GetData<string>("sound");

        SfxSound soundEnumValue = Enum.Parse<SfxSound>(soundName);

        SfxEvent eventToPlay = _sfxEvents.Single(o => o.name == soundEnumValue);
        if(eventToPlay != null && eventToPlay.clip != null)
        {
            _source.PlayOneShot(eventToPlay.clip);
        }
    }
}
