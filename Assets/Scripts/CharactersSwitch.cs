using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersSwitch : MonoBehaviour
{
    AudioListener warriorListener, archerListener, shieldListener, wizardListener;
    [SerializeField] CharacterController warrior, archer, shield, wizard;
    [SerializeField] WeaponPivot warriorPivot, archerPivot, shieldPivot, wizardPivot;
    [SerializeField] CharacterView warriorView, archerView, shieldView, wizardView;
    [SerializeField] Image warriorImage, archerImage, shieldImage, wizardImage;
    [SerializeField] Sprite warriorSelected, warriorUnselected, archerSelected, archerUnselected, shieldSelected, shieldUnselected, wizardSelected, wizardUnselected;
    [SerializeField] AudioSource audioManager;
    [SerializeField] AudioClip swapClip;
    void Start()
    {
        warriorListener = warrior.GetComponent<AudioListener>();
        archerListener = archer.GetComponent<AudioListener>();
        shieldListener = shield.GetComponent<AudioListener>();
        wizardListener = wizard.GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (warrior.isActiveAndEnabled) return;
            WarriorEnabled(true);
            ArcherEnabled(false);
            ShielderEnabled(false);
            WizardEnabled(false);
            audioManager.PlayOneShot(swapClip);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (archer.isActiveAndEnabled) return;
            WarriorEnabled(false);
            ArcherEnabled(true);
            ShielderEnabled(false);
            WizardEnabled(false);
            audioManager.PlayOneShot(swapClip);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (shield.isActiveAndEnabled) return;
            WarriorEnabled(false);
            ArcherEnabled(false);
            ShielderEnabled(true);
            WizardEnabled(false);
            audioManager.PlayOneShot(swapClip);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (wizard.isActiveAndEnabled) return;
            WarriorEnabled(false);
            ArcherEnabled(false);
            ShielderEnabled(false);
            WizardEnabled(true);
            audioManager.PlayOneShot(swapClip);
        }
    }

    public void WarriorEnabled(bool enabled)
    {
        warrior.enabled = enabled;
        warriorPivot.enabled = enabled;
        warriorView.enabled = enabled;
        warriorListener.enabled = enabled;

        if (enabled) warriorImage.sprite = warriorSelected;
        else warriorImage.sprite = warriorUnselected;
    }

    public void ArcherEnabled(bool enabled)
    {
        archer.enabled = enabled;
        archerPivot.enabled = enabled;
        archerView.enabled = enabled;
        archerListener.enabled = enabled;

        if (enabled) archerImage.sprite = archerSelected;
        else archerImage.sprite = archerUnselected;
    }

    public void ShielderEnabled(bool enabled)
    {
        shield.enabled = enabled;
        shieldPivot.enabled = enabled;
        shieldView.enabled = enabled;
        shieldListener.enabled = enabled;

        if (enabled) shieldImage.sprite = shieldSelected;
        else shieldImage.sprite = shieldUnselected;
    }

    public void WizardEnabled(bool enabled)
    {
        wizard.enabled = enabled;
        wizardPivot.enabled = enabled;
        wizardView.enabled = enabled;
        wizardListener.enabled = enabled;

        //if (enabled) wizardImage.sprite = wizardSelected;
        //else wizardImage.sprite = wizardUnselected;
    }
}
