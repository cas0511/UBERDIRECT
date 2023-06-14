namespace PH.UberConnect.Core.Interface
{
    public interface IReadOne<T>
    {
        List<T> Get(int id);
    }
}
