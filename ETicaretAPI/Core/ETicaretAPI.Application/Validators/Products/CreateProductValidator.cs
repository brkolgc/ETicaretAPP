using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Ürün adı boş olamaz.")
                .MaximumLength(150)
                .MinimumLength(1)
                    .WithMessage("Ürün adı 1-150 karakter aralığında olmalıdır.");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Stok bilgisi boş olamaz.")
                .Must(p => p >= 0)
                    .WithMessage("Stok bilgisi negatif olamaz.");

            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Fiyat bilgisi boş olamaz.")
                .Must(p => p >= 0)
                    .WithMessage("Fiyat bilgisi negatif olamaz.");
        }
    }
}
