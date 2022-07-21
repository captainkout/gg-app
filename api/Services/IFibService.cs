namespace api.Services;

public interface IFibService<T>
{
    T RecursiveFib(int index);
    T RecursiveFibWithCache(int index, bool cache = true);
}
