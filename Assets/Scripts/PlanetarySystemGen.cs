using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Decompiled this game? That's fine, just be aware all of the code is atrocious due to its spaghetti nature and throwing out programming practices for a game jam. Clean it up if you do use it!
/// </summary>
public class PlanetarySystemGen : MonoBehaviour 
{
  public GameObject star;
  public GameObject planet;
  public GameObject moon;
  public GameObject orbit;
  public GameObject player;

  int nStars;
  int nPlanets;
  int nMoons;

  bool Roll ( int value )
  {
    int x = Random.Range( 0, 101 );

    return x <= value ? true : false;
  }

  // idea for naming:
  // either give a system a name, then name all planets by roman numerals such as 
  // Sol, Sol I, Sol II, Sol III, Sol IV, etc
  // or give all bodies random names like Vuth and Toone

  string[] systemNames = new string[]
  { 
    "Loskua",
    "Vuthia",
    "Wasmillon",
    "Debriri",
    "Qaetov",
    "Koagawa",
    "Bluferus",
    "Swodarus",
    "Struna",
    "Cruna",
    "Ciags",
    "Grue",
    "Odu",
    "Ashiu",
    "Vrauns",
    "Gigsue",
    "Zoobros",
    "Jondeo",
    "Weobde",
    "Acreen",
    "Lex",
    "Wih",
    "Aked",
    "Want",
    "Graeg",
    "Esruhliu",
    "Egeurdad",
    "Bovre",
    "Ocheccish",
    "Wiagoks",
    "Itoi",
    "Fiax",
    "Yeft",
    "Druact",
    "Osiuh",
    "Crumdiuts",
    "Womsuakt",
    "Wushe",
    "Drarr",
    "Atricsep",
    "Esah",
    "Ekiafs",
    "Noiph",
    "Chaots",
    "Streef",
    "Buphoacz",
    "Aphezteoc",
    "Vraexub",
    "Dricloi",
    "Croiwi",
    "Egleer",
    "Taoy",
    "Zrays",
    "Iflo",
    "Kas",
    "Ozlizla",
    "Vualdu",
    "Plaekdas",
    "Vracde",
    "Ukeevle",
    "Jatrout",
    "Yosmoy",
    "Febrolla",
    "Jeflao",
    "Coawei",
    "Hoiruta",
    "Clecogan",
    "Stodobos",
    "Spone",
    "Snypso"
  };

  string[] romanNumerals = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };

  // for now this is how many planets only, moons are unaffected
  void Generate ( int nPlanetaryBodies )
  {
    string systemName = systemNames[ Random.Range( 0, systemNames.Length ) ];

    GameObject starClone = (GameObject) Instantiate( star, new Vector3( 0, 0, 0 ), Quaternion.identity );
    nStars++;
    float lastPos = starClone.transform.position.x;
    float lastRadius = starClone.GetComponent<CircleCollider2D>().radius;

    starClone.transform.FindChild( "Canvas/Text" ).GetComponent<Text>().text = systemName + " Star";

    int homePlanetN = Random.Range( 0, nPlanetaryBodies );

    for (int i = 0; i < nPlanetaryBodies; i++)
    {
      GameObject planetOrbitClone = (GameObject) Instantiate( orbit, starClone.transform.position, Quaternion.identity );
      nPlanets++;

      GameObject planetClone = (GameObject) Instantiate( planet, new Vector3( 0, 0, 0 ), Quaternion.identity );
      float rX = Random.Range( lastPos + lastRadius + planetClone.GetComponent<CircleCollider2D>().radius, lastPos + lastRadius + planetClone.GetComponent<CircleCollider2D>().radius + 35f );
      planetClone.transform.position = new Vector3( rX, 0, 0 );
      planetClone.transform.parent = planetOrbitClone.transform;
      lastPos = planetClone.transform.position.x;
      lastRadius = planetClone.GetComponent<CircleCollider2D>().radius;

      string planetName = systemName + " " + romanNumerals[ i ];
      planetClone.transform.FindChild( "Canvas/Text" ).GetComponent<Text>().text = planetName;

      if (homePlanetN == i)
      {
        planetClone.GetComponent<Planet>().Generate( true );
        planetClone.transform.FindChild( "Canvas/Text" ).GetComponent<Text>().text = planetName + " [Home]";
      }
      else
      {
        planetClone.GetComponent<Planet>().Generate( false );
        planetClone.transform.FindChild( "Canvas/HomeImage" ).GetComponent<Image>().color = new Color( 255, 255, 255, 0 );
      }

      // create moon for planet, 50% chance
      if (Roll( 50 ))
      {
        GameObject moonOrbitClone = (GameObject) Instantiate( orbit, planetClone.transform.position, Quaternion.identity );

        GameObject moonClone = (GameObject) Instantiate( moon, new Vector3( 0, 0, 0 ), Quaternion.identity );
        nMoons++;
        float rXX = Random.Range( lastPos + lastRadius + moonClone.GetComponent<CircleCollider2D>().radius, lastPos + lastRadius + moon.GetComponent<CircleCollider2D>().radius + 25f );
        moonClone.transform.position = new Vector3( rXX, 0, 0 );
        moonClone.transform.parent = moonOrbitClone.transform;
        lastPos = moonClone.transform.position.x;
        lastRadius = moonClone.GetComponent<CircleCollider2D>().radius;

        moonOrbitClone.transform.parent = planetClone.transform;
        moonOrbitClone.transform.localEulerAngles = new Vector3( 0, 0, Random.Range( 0f, 360f ) );
        moonOrbitClone.GetComponent<Orbiter>().rotateSpeed = Random.Range( 5f, 11f );
      }

      planetOrbitClone.transform.parent = starClone.transform;
      planetOrbitClone.transform.localEulerAngles = new Vector3( 0, 0, Random.Range( 0f, 360f ) );
      planetOrbitClone.GetComponent<Orbiter>().rotateSpeed = Random.Range( 1f, 4f );

      if (homePlanetN == i)
      {
        player.transform.position = planetClone.transform.position;
      }
    }
  }

  void Start ()
  {
    Time.timeScale = 1.0f;
    // options for game length, 3 for quick, 6 for medium, 8 for long
    int planetaryBodies = Random.Range( 8, 13 );
    Debug.Log( "Generating " + planetaryBodies + " planetary bodies" );
    Generate( planetaryBodies );

    PlanetarySystem planetarySystem = GetComponent<PlanetarySystem>();
    planetarySystem.nStars = nStars;
    planetarySystem.nPlanets = nPlanets;
    planetarySystem.nMoons = nMoons;
    GameObject.Destroy( this );
  }
}