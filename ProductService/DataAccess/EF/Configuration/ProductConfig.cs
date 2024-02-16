using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain;

namespace ProductService.DataAccess.EF.Configuration;

internal class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.ToTable("Product");
        entity.HasKey(q => q.Id);
        entity.Property(q => q.Id)
            .UseIdentityColumn();

        entity.Property(q => q.Code).IsRequired();
        entity.Property(q => q.Name).IsRequired();
        entity.Property(q => q.Status).HasConversion<string>();
        entity.Property(q => q.Image);
        entity.Property(q => q.MaxNumberOfInsured).IsRequired();
        entity.Property(q => q.ProductIcon);
        entity.Property(q => q.Description);

        entity.HasMany(q => q.Covers)
            .WithOne(x => x.Product)
            .IsRequired(true);

        entity.HasMany(q => q.Questions)
            .WithOne(x => x.Product)
            .IsRequired(true);
    }
}

internal class CoverConfig : IEntityTypeConfiguration<Cover>
{
    public void Configure(EntityTypeBuilder<Cover> entity)
    {
        entity.ToTable("Cover");
        entity.HasKey(q => q.Id);
        entity.Property(q => q.Id)
            .UseIdentityColumn();
        entity.Property(c => c.Code).IsRequired();
        entity.Property(c => c.Name).IsRequired();
        entity.Property(c => c.Description);
        entity.Property(c => c.SumInsured);
        entity.Property(c => c.Optional).IsRequired();
        entity.HasOne(c => c.Product)
            .WithMany(c => c.Covers);
    }
}

internal class QuestionConfig : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> entity)
    {
        entity.ToTable("Question");
        entity.HasKey(p => p.Id);
        entity.Property(q => q.Id)
            .UseIdentityColumn();
        entity
            .HasDiscriminator<int>("QuestionType")
            .HasValue<Question>(0)
            .HasValue<NumericQuestion>(1)
            .HasValue<DateQuestion>(2)
            .HasValue<ChoiceQuestion>(3);
        entity.HasOne(p => p.Product)
            .WithMany(p => p.Questions);
        entity.Property(q => q.Code).IsRequired();
        entity.Property(q => q.Index).IsRequired();
        entity.Property(q => q.Text).IsRequired();

    }
}

internal class ChoiceQuestionConfig : IEntityTypeConfiguration<ChoiceQuestion>
{
    public void Configure(EntityTypeBuilder<ChoiceQuestion> entity)
    {
        entity.HasBaseType<Question>();
        entity.HasMany(q => q.Choices);
    }
}


internal class ChoiceConfig : IEntityTypeConfiguration<Choice>
{
    public void Configure(EntityTypeBuilder<Choice> entity)
    {
        entity.ToTable("Choice");
        entity.HasKey(p => p.Id);
        entity.Property(q => q.Id)
            .UseIdentityColumn();
        entity.Property(x => x.Code);
        entity.Property(x => x.Label);
        entity.HasOne(q => q.Question).WithMany(c => c.Choices);

    }
}