using UnityEngine;

/// <summary>
/// This Script is used by all of the buttons to play SFX for clicking 
/// </summary>
public class PlaySFX : MonoBehaviour
{
    //The Audio Source that will click on the button 
    private AudioSource _clickSfx;
    
    //==================================================================================================================
    // Button Spawning 
    //==================================================================================================================

    /// <summary>
    /// Connects the AudioSource to the component 
    /// </summary>
    private void Start() { _clickSfx = GetComponent<AudioSource>(); }

    /// <summary>
    /// The Method that will be used to play the SFX 
    /// </summary>
    public void PlaySfx() { _clickSfx.Play(); }
}
