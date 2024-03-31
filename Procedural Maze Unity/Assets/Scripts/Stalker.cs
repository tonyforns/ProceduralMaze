using UnityEngine;

public class Stalker : MonoBehaviour
{
    [SerializeField] private Character character;


    private void Update()
    {
        StalkCharacter();
    }
    private void StalkCharacter()
    {
        Vector2Int dungeonPosition = Dungeon.Instance.WorldPositionToDungeon(character.transform.position);
        Vector2Int newPosition = dungeonPosition + new Vector2Int(Mathf.RoundToInt(character.transform.forward.x), Mathf.RoundToInt(character.transform.forward.z)) * 2;
        transform.position = Dungeon.Instance.DungeonPositionToWorld(newPosition);
    }
}
