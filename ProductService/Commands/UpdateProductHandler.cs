using MediatR;
using ProductService.Api.Commands;
using ProductService.Api.Commands.Dtos;
using ProductService.Domain;

namespace ProductService.Commands;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository productRepository;
    public UpdateProductHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await productRepository.FindOne(request.ProductDraft.Code);
        if (existingProduct is null)
        {
            return new UpdateProductResult();
        }
        existingProduct.RemoveCovers(existingProduct.Covers);
        existingProduct.RemoveQuestions(existingProduct.Questions);

        var covers = new List<Cover>();
        foreach (var cover in request.ProductDraft.Covers)
        {
            covers.Add(Cover.CreateCover(cover.Code, cover.Name, cover.Description, cover.Optional, cover.SumInsured));
        }
        existingProduct.UpdateCover(covers);

        var questions = new List<Question>();
        foreach (var question in request.ProductDraft.Questions)
        {
            switch (question)
            {
                case NumericQuestionDto numericQuestion:

                    questions.Add(new NumericQuestion(numericQuestion.QuestionCode, numericQuestion.Index,
                        numericQuestion.Text));
                    break;
                case DateQuestionDto dateQuestion:
                    questions.Add(new DateQuestion(dateQuestion.QuestionCode, dateQuestion.Index,
                        dateQuestion.Text));
                    break;
                case ChoiceQuestionDto choiceQuestion:
                    questions.Add(new ChoiceQuestion(choiceQuestion.QuestionCode, choiceQuestion.Index,
                        choiceQuestion.Text, choiceQuestion.Choices.Select(c => new Choice(c.Code, c.Label)).ToList()));
                    break;
            }
        }

        existingProduct.UpdateQuestion(questions);

        var newProduct = Product.UpdateProduct(existingProduct.Id, request.ProductDraft.Code, request.ProductDraft.Name, request.ProductDraft.Image,
            request.ProductDraft.Description, covers, questions, request.ProductDraft.MaxNumberOfInsured, request.ProductDraft.Icon);
        await productRepository.Update(newProduct);

        return new UpdateProductResult
        {
            ProductId = existingProduct.Id,
        };
    }
}
