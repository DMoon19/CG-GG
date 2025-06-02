using System;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class Toggle : MonoBehaviour
{
    [Header("Directors")]
    [SerializeField] private PlayableDirector director1;
    [SerializeField] private PlayableDirector director2;
    [SerializeField] private PlayableDirector director3;
    [SerializeField] private PlayableDirector director4;

    [Header("Dropdown")]
    [SerializeField] private TMP_Dropdown dropdown;

    [Header("EffectsGMParent")]
    [SerializeField] private GameObject effect1;
    [SerializeField] private GameObject effect2;
    [SerializeField] private GameObject effect3;
    [SerializeField] private GameObject effect4;

    private PlayableDirector[] directors;
    private GameObject[] effects;

    private int currentIndex = 0;
    private bool isPaused = false;
    private PlayableDirector currentDirector;

    void Awake()
    {
        directors = new PlayableDirector[] { director1, director2, director3, director4 };
        effects = new GameObject[] { effect1, effect2, effect3, effect4 };

        ChooseDirector(currentIndex);
    }

    public void ChooseDirector(int index)
    {
        foreach (var e in effects)
            e.SetActive(false);

        foreach (var d in directors)
        {
            d.Stop();
        }

        currentIndex = Mathf.Clamp(index, 0, directors.Length - 1);
        effects[currentIndex].SetActive(true);
        currentDirector = directors[currentIndex];
        currentDirector.Play();
        isPaused = false;
        Debug.Log("Playing director: " + currentDirector.name);
    }

    public void TogglePause()
    {
        if (currentDirector == null) return;

        if (isPaused)
        {
            currentDirector.Play();
            isPaused = false;
            Debug.Log("Reanudado: " + currentDirector.name);
        }
        else if (currentDirector.state == PlayState.Playing)
        {
            currentDirector.Pause();
            isPaused = true;
            Debug.Log("Pausado: " + currentDirector.name);
        }
    }

    public void NextEffect()
    {
        int nextIndex = (currentIndex + 1) % directors.Length;
        ChooseDirector(nextIndex);
    }

    public void PreviousEffect()
    {
        int previousIndex = (currentIndex - 1 + directors.Length) % directors.Length;
        ChooseDirector(previousIndex);
    }
}
