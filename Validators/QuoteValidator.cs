using FluentValidation;
using QuotesApi.Models;

namespace QuotesApi.Validators
{
    public class QuoteValidator : AbstractValidator<Quote>
    {
        public QuoteValidator()
        {
            RuleFor(x => x.Author).NotEmpty().WithMessage("Please specify an Author").Length(3, 30);
            RuleFor(x => x.Title).NotEmpty().WithMessage("Please specify a Title").Length(3, 30);
            RuleFor(x => x.Description).NotEmpty().WithMessage("Please specify a Description").Length(3, 250);
            RuleFor(x => x.Type).NotEmpty().WithMessage("Please specify a Type").Length(3, 30).Must(BeAValidType).WithMessage("'Type' must start with 'T-'");
            //RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);
        }

        private bool BeAValidType(string type)
        {
            if (type.Length < 2)
            {
                return false;
            }
            var res = type.Substring(0, 2).ToLower();
            if(res.Equals("t-"))
            {
                return true;
            }
            return false;
        }
    }
}
