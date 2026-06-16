using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music Tracks 🎶")]
    public AudioSource musicSource;
    public AudioSource uiMusicSource; // ДЛЯ ФОНОВОЙ МУЗЫКИ ПАУЗЫ/ПОБЕДЫ 🎵🏆
    public AudioClip menuMusic;
    public AudioClip[] levelMusicTracks; 

    [Header("SFX (Звуковые эффекты) 🔊")]
    public AudioSource sfxSource;
    public AudioClip catchFruitSound;
    public AudioClip catchSlimeSound;
    public AudioClip powerUpSound;
    public AudioClip buttonClickSound;
    public AudioClip pauseSound;
    public AudioClip victorySound;
    public AudioClip gameOverSound;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        } else {
            Destroy(gameObject);
        }
    }

    private void SetupAudioSources()
    {
        // ИЩЕМ КОМПОНЕНТЫ И ВЫКРУЧИВАЕМ ИХ НА ГРОМКОСТЬ! ✅🦾🔊
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length >= 3) {
            musicSource = sources[0];
            sfxSource = sources[1];
            uiMusicSource = sources[2];
        } else {
            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
            if (uiMusicSource == null) uiMusicSource = gameObject.AddComponent<AudioSource>();
        }

        // НАСТРОЙКА: ПРИГЛУШАЕМ МУЗЫКУ (80%), ЧТОБЫ SFX ЗВУЧАЛИ ЧЕТЧЕ! ✅🔊🛰✨🚀
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.8f; // Музыка на 80% ✨📈
        musicSource.ignoreListenerPause = true; // АБСОЛЮТНАЯ ЗАЩИТА! ✨🛡️ Отключение SFX не вырубит музыку!
        
        uiMusicSource.loop = true; // Возвращаем Зацикливание! Раз это полноценные мелодии, они должны играть на фоне меню паузы бесконечно 🎵🌌
        uiMusicSource.playOnAwake = false;
        uiMusicSource.volume = 0.8f; // UI Музыка тоже на 80%
        uiMusicSource.ignoreListenerPause = true;
        
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = 1.0f; // НА ВСЮ МОЩНОСТЬ! ✨📈
    }

    public void ApplyAudioSettings()
    {
        bool isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        bool isSFXOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;

        if (musicSource != null) musicSource.mute = !isMusicOn;
        if (uiMusicSource != null) uiMusicSource.mute = !isMusicOn;
        AudioListener.pause = !isSFXOn; // Выключает/включает ВСЕ звуки мира (кроме тех, у кого ignoreListenerPause=true, то есть Музыки!)
    }

    void Start()
    {
        ApplyAudioSettings(); // СИНХРОНИЗИРУЕМ С НАСТРОЙКАМИ ИГРОКА ПРИ СТАРТЕ! ✅🎶🏎️💨🚀
        PlayMainMenuMusic();
    }

    public void PlayMainMenuMusic() { PlayMusic(menuMusic); }

    public void PlayLevelMusic(int index)
    {
        if (levelMusicTracks != null && index >= 0 && index < levelMusicTracks.Length)
            PlayMusic(levelMusicTracks[index]);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        
        if (uiMusicSource != null) uiMusicSource.Stop(); // Все UI мелодии выключаем!

        // Переключаем музыку только если она реально ДРУГАЯ! ✨🛰
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play(); 
    }

    // SFX (Оставил без изменений) 🔊
    public void PlaySFX(AudioClip clip) { if (clip != null && sfxSource != null) sfxSource.PlayOneShot(clip); }
    public void PlayCatchFruit() => PlaySFX(catchFruitSound);
    public void PlayCatchSlime() => PlaySFX(catchSlimeSound);
    public void PlayPowerUp() => PlaySFX(powerUpSound);
    public void PlayClick() => PlaySFX(buttonClickSound);

    // НОВЫЕ ФОНОВЫЕ МЕЛОДИИ (взамен старых коротких звуков) 🎶
    public void PlayPauseMusic()
    {
        if (musicSource != null) musicSource.Pause();
        if (uiMusicSource != null && pauseSound != null) { uiMusicSource.clip = pauseSound; uiMusicSource.Play(); }
    }
    public void UnpauseMusic()
    {
        if (uiMusicSource != null) uiMusicSource.Stop();
        if (musicSource != null) musicSource.UnPause();
    }
    public void PlayVictoryMusic()
    {
        if (musicSource != null) musicSource.Stop();
        if (uiMusicSource != null && victorySound != null) { uiMusicSource.clip = victorySound; uiMusicSource.Play(); }
    }
    public void PlayGameOverMusic()
    {
        if (musicSource != null) musicSource.Stop();
        if (uiMusicSource != null && gameOverSound != null) { uiMusicSource.clip = gameOverSound; uiMusicSource.Play(); }
    }
}
