using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
  void Start ()
  {
    Time.timeScale = 1.0f;
  }
  public void StartGame ()
  {
    Application.LoadLevelAsync( "Game" );
  }

  public void ExitGame ()
  {
    Application.Quit();
  }
}