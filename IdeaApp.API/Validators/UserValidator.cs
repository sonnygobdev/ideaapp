using FluentValidation;
using IdeaApp.API.Dtos;

namespace IdeaApp.API.Validators
{
    public class UserValidator:AbstractValidator<UserForRegisterDto>
    {
        public UserValidator()
        {
            RuleFor(_=>_.UserName).NotEmpty();
            RuleFor(_=>_.Password).NotEmpty().MinimumLength(4).MaximumLength(8).WithMessage("You must specify password between 4 and 8 characters");
            
        }
    }
}