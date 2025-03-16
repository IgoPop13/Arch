namespace SmartLinks.Interfaces
{
    public interface IDependencyResolver
    {
        public object Resolve(string dependency, object[] args);
    }
}
