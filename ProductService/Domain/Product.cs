namespace ProductService.Domain;

public class Product
{
    private Product()
    {
    }

    private Product(string code, string name, string image, string description, int maxNumberOfInsured,
        string productIcon)
    {
        Code = code;
        Name = name;
        Status = ProductStatus.Draft;
        Image = image;
        Description = description;
        Covers = new List<Cover>();
        Questions = new List<Question>();
        MaxNumberOfInsured = maxNumberOfInsured;
        ProductIcon = productIcon;
    }

    private Product(long id, string code, string name, string image, string description,
        IList<Cover> covers, IList<Question> questions, int maxNumberOfInsured, string productIcon)
    {
        Id = id;
        Code = code;
        Name = name;
        Status = ProductStatus.Active;
        Image = image;
        Description = description;
        Covers = covers;
        Questions = questions;
        MaxNumberOfInsured = maxNumberOfInsured;
        ProductIcon = productIcon;
    }

    public long Id { get; }
    public string Code { get; }
    public string Name { get; }
    public ProductStatus Status { get; private set; }
    public string Image { get; }
    public string Description { get; }
    public IList<Cover> Covers { get; }
    public IList<Question> Questions { get; }
    public int MaxNumberOfInsured { get; }
    public string ProductIcon { get; }

    public static Product CreateDraft(string code, string name, string image, string description,
        int maxNumberOfInsured, string productIcon)
    {
        return new Product(code, name, image, description, maxNumberOfInsured, productIcon);
    }

    public static Product UpdateProduct(long id, string code, string name, string image, string description,
        IList<Cover> covers, IList<Question> questions, int maxNumberOfInsured, string productIcon)
    {
        return new Product(id, code, name, image, description, covers, questions, maxNumberOfInsured, productIcon);
    }

    public void Activate()
    {
        EnsureIsDraft();
        Status = ProductStatus.Active;
    }

    public void Discontinue()
    {
        Status = ProductStatus.Discontinued;
    }

    public void AddCover(string code, string name, string description, bool optional, decimal? sumInsured)
    {
        EnsureIsDraft();
        Covers.Add(new Cover(code, name, description, optional, sumInsured));
    }

    public void UpdateCover(IList<Cover> cover)
    {
        foreach (var coverItem in cover.ToList())
            Covers.Add(coverItem);
    }

    public void RemoveCovers(IEnumerable<Cover> covers)
    {
        var coversToRemove = covers.ToList();

        foreach (var cover in coversToRemove)
            Covers.Remove(cover);
    }

    public void AddQuestions(IEnumerable<Question> questions)
    {
        EnsureIsDraft();
        foreach (var q in questions)
            Questions.Add(q);
    }

    public void UpdateQuestion(IList<Question> questions)
    {
        foreach (var question in questions.ToList())
            Questions.Add(question);
    }

    public void RemoveQuestions(IEnumerable<Question> questions)
    {
        var questionToRemove = questions.ToList();

        foreach (var q in questionToRemove)
            Questions.Remove(q);
    }

    private void EnsureIsDraft()
    {
        if (Status != ProductStatus.Draft)
            throw new ApplicationException("Only draft version can be modified and activated");
    }
}

public enum ProductStatus
{
    Draft,
    Active,
    Discontinued
}