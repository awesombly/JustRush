using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public List<GameObject> spawnCharacters;
    public int spawnCount;

    public BoxCollider2D spawnArea;

    protected void Start()
    {
        if ( spawnArea == null || spawnCharacters.Count == 0 )
        {
            return;
        }

        for ( int i = 0; i < spawnCount; ++i )
        {
            GameObject character = spawnCharacters[ Random.Range( 0, spawnCharacters.Count ) ];

            Vector3 spawnPosition = new Vector3(
                Random.Range( spawnArea.bounds.min.x, spawnArea.bounds.max.x )
                , Random.Range( spawnArea.bounds.min.y, spawnArea.bounds.max.y )
                , 0.0f );

            GameObject instance = Instantiate( character, spawnPosition, Quaternion.identity, gameObject.transform );
            Character enemy = instance?.GetComponent<Character>();
            if ( enemy == null )
            {
                return;
            }

            instance.layer = LayerMask.NameToLayer( "Enemy" );
            enemy.transform.localScale = Vector3.one * Random.Range( 0.9f, 1.1f );
            enemy.TeamType = TeamType.Enemy;
            enemy.Health = Random.Range( 50.0f, 150.0f );
            enemy.strength = Random.Range( 5.0f, 25.0f );
            enemy.moveSpeed = Random.Range( 2.0f, 6.0f );
        }
    }
}
