using UnityEngine;
using UnityEngine.TextCore.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChangeDirection : MonoBehaviour
{
    public GameObject character;
    public GameObject flash;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChangeDirection))]
class ChangeDirectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var changeDirection = (ChangeDirection)target;
        if (changeDirection == null || changeDirection.character == null) return;

        Undo.RecordObject(changeDirection, "Change changeDirection");

        string direction = "[ERROR]";
        UpdateDirection();

        GUILayout.Label("\nFacing direction: " + direction);

        if (GUILayout.Button("Change Direction"))
        {
            changeDirection.character.transform.localScale = new Vector2(-changeDirection.character.transform.localScale.x, changeDirection.character.transform.localScale.y);
            changeDirection.flash.transform.localScale = new Vector2(-changeDirection.flash.transform.localScale.x, changeDirection.flash.transform.localScale.y);
            UpdateDirection();
        }

        void UpdateDirection()
        {
            if (changeDirection.character.transform.localScale.x > 0)
            {
                direction = "Right";
            }
            else
            {
                direction = "Left";
            }
        }
    }
}
#endif