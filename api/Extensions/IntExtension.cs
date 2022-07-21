namespace api.Extensions;

public static partial class ApiExtensions
{
    public static int IterativeFib(this int index)
    {
        if (index == 0)
            return 0;
        if (index <= 2)
            return 1;
        var subTwo = 1;
        var subOne = 1;
        var current = subTwo + subOne;
        for (var i = 3; i < index; i++)
        {
            subTwo = subOne;
            subOne = current;
            current = subTwo + subOne;
            if (current < 0)
                throw new Exception("Error occured. Likely overflow.");
        }
        return current;
    }
}
