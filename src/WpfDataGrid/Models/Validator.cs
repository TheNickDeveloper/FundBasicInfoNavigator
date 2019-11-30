using FluentValidation;

//todo, not in use yet
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
