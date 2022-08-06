using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� �̸� enumȭ (���Է� ����)
/// </summary>
public class Scene
{
    public static readonly Scene GameScene = new Scene("GameScene");
    public static readonly Scene MainScene = new Scene("TempMenuScene");

    public readonly string name;

    public Scene(string name) {
        this.name = name;
    }
}
