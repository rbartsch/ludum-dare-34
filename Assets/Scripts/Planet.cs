using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//public enum PlanetType
//{
//  Desert,
//  Lush,
//  Oceanic,
//  Gas
//}

//public enum Resources
//{
//  ConstructionMaterials,
//  NuclearFuel,
//  Food,
//  LuxuryItems
//}

//public class Planet : MonoBehaviour
//{
//  // type
//  // amount of tiles
//  // colonizable
//  // resources
//  // etc
//  public bool homePlanet;
//  public PlanetType type;
//  public int tiles;
//  public List<Resources> resources = new List<Resources>();
//  public int constructionMaterialsCount = 0;
//  public int nuclearFuelCount = 0;
//  public int foodCount = 0;
//  public int luxuryItemsCount = 0;

//  public void Generate ()
//  {
//    type = (PlanetType) Random.Range( 0, 4 );
//    tiles = Random.Range( 1, 5 );
//    int resource = -1;
//    int nResources = homePlanet ? 4 : Random.Range( 1, 3 );
//    int j = 0;

//    Debug.Log( "Resources " + nResources );
//    for (int i = j; i < nResources; i++)
//    {
//      resource = Random.Range( 0, 4 );
//      if (resources.Contains( (Resources) resource ))
//      {
//        Debug.Log( "Found dupe resource. " + (Resources) resource + " already exists. Trying again for a new resource" );
//        j = i; // start counting from where we run into dupe
//        i--; // go back one so we try again and not skip
//      }
//      else
//      {
//        resources.Add( (Resources) resource );
//        Debug.Log( "Adding " + (Resources) resource );
//      }
//    }

//    if (!homePlanet)
//    {
//      for (int i = 0; i < resources.Count; i++)
//      {
//        switch (resources[ i ])
//        {
//          case Resources.ConstructionMaterials:
//            constructionMaterialsCount = Random.Range( 1, 25 ); break;
//          case Resources.Food:
//            foodCount = Random.Range( 1, 25 ); break;
//          case Resources.LuxuryItems:
//            luxuryItemsCount = Random.Range( 1, 25 ); break;
//          case Resources.NuclearFuel:
//            nuclearFuelCount = Random.Range( 1, 25 ); break;
//          default:
//            break;
//        }
//      }
//    }
//    else
//    {
//      constructionMaterialsCount = 5;
//      foodCount = 5;
//      luxuryItemsCount = 5;
//      nuclearFuelCount = 5;
//    }
//  }
//}

public enum PlanetType
{
  Desert,
  Lush,
  Oceanic,
  Gas
}

public class Planet : MonoBehaviour
{
  // type
  // amount of tiles
  // colonizable
  // resources
  // etc
  public bool homePlanet;
  public PlanetType type;
  public int fuelCompounds = 0;
  public int structuralCompounds = 0;
  public int ordnanceCompounds = 0;
  public int edibleFood = 0;
  public bool explored = false;

  bool Roll ( int value )
  {
    int x = Random.Range( 0, 101 );

    return x <= value ? true : false;
  }

  public void Generate (bool _homePlanet)
  {
    homePlanet = _homePlanet;

    if (homePlanet)
    {
      type = (PlanetType) Random.Range( 0, 2 );
      explored = true;
    }
    else
    {
      type = (PlanetType) Random.Range( 0, 4 );
    }

    if (homePlanet)
    {
      return;
    }

    switch (type)
    {
      case PlanetType.Desert:
        if (Roll( 90 ))
        {
          fuelCompounds = Random.Range( 6, 15 );
        }
        if (Roll( 60 ))
        {
          structuralCompounds = Random.Range( 10, 15 );
        }
        if (Roll( 60 ))
        {
          ordnanceCompounds = Random.Range( 15, 25 );
        }
        if (Roll( 38 ))
        {
          edibleFood = Random.Range( 3, 12 );
        }
        break;

      case PlanetType.Lush:
        if (Roll( 90 ))
        {
          edibleFood = Random.Range( 11, 15 );
        }
        if (Roll( 70 ))
        {
          fuelCompounds = Random.Range( 5, 10 );
        }
        if (Roll( 60 ))
        {
          structuralCompounds = Random.Range( 3, 7 );
        }
        if (Roll( 60 ))
        {
          ordnanceCompounds = Random.Range( 11, 25 );
        }
        break;

      case PlanetType.Oceanic:
        if (Roll( 80 ))
        {
          edibleFood = Random.Range( 11, 15 );
        }
        if (Roll( 40 ))
        {
          fuelCompounds = Random.Range( 3, 9 );
        }
        break;

      case PlanetType.Gas:
        if (Roll( 75 ))
        {
          fuelCompounds = Random.Range( 7, 10 );
        }
        if (Roll( 50 ))
        {
          ordnanceCompounds = Random.Range( 11, 25 );
        }
        if (Roll( 45 ))
        {
          structuralCompounds = Random.Range( 7, 12 );
        }
        break;

      default:
        break;
    }
  }

  GameUI gameUI;
  Player player;

  void Awake ()
  {
    gameUI = GameObject.Find( "GameUI" ).GetComponent<GameUI>();
    player = GameObject.Find( "PlayerShip" ).GetComponent<Player>();
  }

  void OnMouseDown ()
  {
    if (explored)
    {
      gameUI.targetStr = transform.FindChild( "Canvas/Text" ).GetComponent<Text>().text + " — Explored";
      gameUI.planetTypeStr = type.ToString() + " Type";
      gameUI.fuelCompoundsStr = "Fuel Compounds\t\t\t" + fuelCompounds.ToString();
      gameUI.structuralCompoundsStr = "Structural Compounds\t" + structuralCompounds.ToString();
      gameUI.ordnanceCompoundsStr = "Ordnance Compounds\t" + ordnanceCompounds.ToString();
      gameUI.edibleFoodStr = "Edible Food\t\t\t\t\t" + edibleFood.ToString();
    }
    else
    {
      gameUI.targetStr = transform.FindChild( "Canvas/Text" ).GetComponent<Text>().text;
      gameUI.planetTypeStr = "Unknown Planet Type";
      gameUI.fuelCompoundsStr = "Unknown Resource 0";
      gameUI.structuralCompoundsStr = "Unknown Resource 0";
      gameUI.ordnanceCompoundsStr = "Unknown Resource 0";
      gameUI.edibleFoodStr = "Unknown Resource 0";
    }
    player.target = this.gameObject;
  }
}