using Save;

public class MenuOnLoad : Saver
{
    public override void Save()
    {
        
    }

    public override void Load()
    {
        GetComponentInChildren<SaveMenu>(true).gameObject.SetActive(true);
    }
}
