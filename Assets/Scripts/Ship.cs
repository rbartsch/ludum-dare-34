using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour 
{
  public string shipName;
  public int shields; public int maxShields;
  public int armor; public int maxArmor;
  public int hull; public int maxHull;
  public int damageOutput;
  public int fuel;
  public int ordnance;
  public int food;
  public int repairEquipment;

  public bool destroyed = false;

  string[] names = new string[] { "Spectrum",
                                  "Termite",
                                  "Kryptoria",
                                  "The Gladiator",
                                  "Analyser",
                                  "Thunder",
                                  "Maria",
                                  "Assassin",
                                  "Armageddon",
                                  "Philadelphia",
                                  "Totale",
                                  "Zeus",
                                  "The Serpent",
                                  "Gibraltar",
                                  "Black Viper",
                                  "Carbonaria",
                                  "Endeavor",
                                  "Thebes",
                                  "Vampire",
                                  "Alexander",
                                  "Paramount",
                                  "The Watcher",
                                  "Detection",
                                  "Merkava",
                                  "Xerxes",
                                  "Javelin",
                                  "Nero",
                                  "Achilles",
                                  "Centipede",
                                  "Dauntless",
                                  "Dispatcher",
                                  "Pathfinder",
                                  "Knossos",
                                  "Gremlin",
                                  "The Titan",
                                  "Saragossa",
                                  "Pursuit",
                                  "Phobetor",
                                  "The Bedlam Blade",
                                  "The Chameleon's Intellect",
                                  "The Charity",
                                  "The Destiny Drum",
                                  "The Diviner",
                                  "The First Victor",
                                  "The Handsome Vision",
                                  "The Happiness",
                                  "The Hellish Wanderer",
                                  "The Insane Bradley",
                                  "The Carolyn",
                                  "The Lovely Marie",
                                  "The Northwestern Derrick",
                                  "The Prophet's Skull",
                                  "The Savior Jon",
                                  "The Savior",
                                  "The Universal Seraphim",
                                  "The Iermpistai",
                                  "The Splo",
                                  "The Orion Scourge",
                                  "The Ivarurmpo",
                                  "The Vengeful Druvreenki",
                                  "The Vit",
                                  "The Bleeding Xoitu",
                                  "The Strodree's Cliufirmern",
                                  "The Clilka",
                                  "The Gaupa"};

  void Start ()
  {
    if (initialized)
    {
      return;
    }

    shipName = names[ Random.Range( 0, names.Length ) ];

    if (name == "PlayerShip")
    {
      SetupPlayerShip();
    }
    else
    {
      SetupAIShip();
    }
  }

  bool initialized = false;
  public void Initialize ()
  {
    shipName = names[ Random.Range( 0, names.Length ) ];

    if (name == "PlayerShip")
    {
      SetupPlayerShip();
    }
    else
    {
      SetupAIShip();
    }

    initialized = true;
  }

  void SetupPlayerShip ()
  {
    shields = maxShields = 30;
    armor = maxArmor = 15;
    hull = maxHull = 7;
    damageOutput = 5;
    fuel = 7;
    ordnance = 22;
    food = 5;
    repairEquipment = 5;
  }

  void SetupAIShip ()
  {
    shields = maxShields = Random.Range( 5, 21 );
    armor = maxArmor = Random.Range( 5, 11 );
    hull = maxHull = Random.Range( 5, 6 );
    damageOutput = Random.Range( 4, 7 );
    ordnance = 9000;
    fuel = 9000;
    food = 9000;
    repairEquipment = 9000;
  }

  bool Roll ( int value )
  {
    int x = Random.Range( 0, 101 );

    return x <= value ? true : false;
  }

  public int DoDamage (Ship ship)
  {
    int dmgModifier = 0;

    if (ordnance > 0)
    {
      ordnance -= 1;

      if (Roll( 80 ))
      {
        dmgModifier = 0;
      }
      if (Roll( 60 ))
      {
        dmgModifier = 2;
      }
      if (Roll( 40 ))
      {
        dmgModifier = 3;
      }

      if (damageOutput - dmgModifier < 0)
      {
        dmgModifier = damageOutput;
      }
      ship.TakeDamage( damageOutput - dmgModifier );
    }
    else
    {
      dmgModifier = damageOutput;
    }

    return damageOutput - dmgModifier;
  }

  public void TakeDamage (int damage)
  {
    if (shields > 0)
    {
      shields -= damage;
    }
    else if (armor > 0)
    {
      armor -= damage;
    }
    else
    {
      hull -= damage;
    }

    if (hull <= 0)
    {
      if (gameObject.name != "Player")
      {
        Destroy( this.gameObject );
      }
      else
      {
        destroyed = true;
      }
    }
  }
}