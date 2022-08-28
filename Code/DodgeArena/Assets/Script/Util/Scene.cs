using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 이름 enum화 (오입력 방지)
/// </summary>
public class SceneEnum
{
    public static readonly SceneEnum GameScene = new SceneEnum("GameScene");
    public static readonly SceneEnum MainScene = new SceneEnum("TempMenuScene");

    public readonly string name;

    public SceneEnum(string name) {
        this.name = name;
    }
}
