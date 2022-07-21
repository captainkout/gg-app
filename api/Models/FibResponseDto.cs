namespace Api.Models;

public class FibResponseDto<T>
{
    public bool Completed { get; set; }
    public List<T> Values { get; set; }

    public FibResponseDto()
    {
        Values = new List<T>();
    }
}
