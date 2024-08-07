using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogaConstants
{
    #region Public members

    /// <summary>
    /// Duration of fade for executing icon displayed beside blocks & commands.
    /// </summary>
    public const float ExecutingIconFadeTime = 0.5f;

    /// <summary>
    /// The current version of the Flowchart. Used for updating components.
    /// </summary>
    public const int CurrentVersion = 1;

    /// <summary>
    /// The name of the initial node in a new flowchart.
    /// </summary>
    public const string DefaultNodeName = "New Block";

    /// <summary>
    /// The default choice node color.
    /// </summary>
    public static Color DefaultChoiceNodeTint = new Color32(101, 176, 255, 255);

    /// <summary>
    /// The default event node color.
    /// </summary>
    public static Color DefaultEventNodeTint = new Color32(69, 237, 205, 255);

    /// <summary>
    /// The default process node color.
    /// </summary>
    public static Color DefaultProcessNodeTint = new Color32(255, 180, 101, 255);

    /// <summary>
    /// The default key used for storing save game data in PlayerPrefs.
    /// </summary>
    public const string DefaultSaveDataKey = "save_data";

    /// <summary>
    /// The default wait time for a node to be considered hovered.
    /// </summary>
    public const float NodeHoverTime = 0.5f;

    #endregion
}
