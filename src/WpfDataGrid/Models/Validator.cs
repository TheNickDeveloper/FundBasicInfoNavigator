using FluentValidation;

//todo, may need to used for dynamic input data
namespace FundBasicInfoNavigator.ProcessLogic
{
    public class Validator:AbstractValidator<BasicInfo>
    {
        public Validator()
        {
            RuleFor(b => b.Name).NotEmpty();
        }
    }
}
