namespace ProductService.Domain;

public class Choice
{
    public Choice()
    {
    }

    public Choice(string code, string label)
    {
        Code = code;
        Label = label;
    }
    public long Id { get; }
    public string Code { get; }
    public string Label { get; }

    public ChoiceQuestion Question { get; private set; }
}