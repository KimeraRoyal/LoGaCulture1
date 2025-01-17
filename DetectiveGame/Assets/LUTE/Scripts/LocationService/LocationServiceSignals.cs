namespace LoGaCulture.LUTE
{
    public static class LocationServiceSignals
    {
        public static event LocationFailedHandler OnLocationFailed;
        public delegate void LocationFailedHandler(FailureMethod failureMethod, Node relatedNode);

        public static event LocationCompleteHandler OnLocationComplete;
        public delegate void LocationCompleteHandler(LocationVariable location);

        public static void DoLocationFailed(FailureMethod failureMethod, Node relatedNode)
        {
            OnLocationFailed?.Invoke(failureMethod, relatedNode);
        }

        public static void DoLocationComplete(LocationVariable location)
        {
            OnLocationComplete?.Invoke(location);
        }
    }
}
