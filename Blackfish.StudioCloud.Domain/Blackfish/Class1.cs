namespace Blackfish;

public class Class1
{
#if StudioCloud
    public string GetMessage()
    {
        return "Hello from StudioCloud Domain!";
    }
#endif
}