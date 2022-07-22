namespace Api.Services;

public interface IFibService<T>
{
    T RecursiveFib(int index);
    T RecursiveFibWithCache(int index, bool cache = true);
    T CancelableFib(int index, bool cache, CancellationToken token);
    List<T> ListFibWithCache(int startIndex, int endIndex, bool cache, CancellationToken token);
}
