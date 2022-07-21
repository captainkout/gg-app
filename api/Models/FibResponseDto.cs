namespace Api.Models;

public class FibResponseDto
{
    public FibResponseDto()
    {
        Values = new List<int>();
    }

    public bool Completed { get; set; }
    public List<int> Values { get; set; }
}
