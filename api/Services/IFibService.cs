namespace Api.Services;

public interface IFibService<T>
{
    T RecursiveFib(int index);
    T RecursiveFibWithCache(int index, bool cache = true);
    List<T> ListFibWithCache(int startIndex, int endIndex, bool cache = true);
}
