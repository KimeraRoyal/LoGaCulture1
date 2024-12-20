using UnityEngine;

/// Implement this interface to be notified about events relating to writing text (text writer)
public interface IWriterListener
{
    //called when the text writer writes a new character glyph
    void OnGlyph();
}
