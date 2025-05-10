namespace PerceptionSystem
{
    public interface IPerciever
    {
        public void StartPercieving(APercievedObject percievedObject);
        public void StopPercieving(APercievedObject percievedObject);
    }
}
