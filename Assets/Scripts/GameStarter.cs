using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    // Start is called before the first frame update
    Character[] characters;
    void Start()
    {
        characters = FindObjectsOfType<Character>();
    }
    public void AttackAll()
    {
        foreach(Character c in characters)
        {
            c.AttackEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
