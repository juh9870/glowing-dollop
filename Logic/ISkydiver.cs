namespace Logic
{
    public interface ISkydiver
    {
        public abstract string Dive();
        bool CanDive { get; set; }
    }
}