using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Interfaces.Entities;
using Interfaces.Other;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace Core.Infrastructure.Models.Entities;

public class User : RegUser, IUser
{
    [Reactive] public ITokens Tokens { get; set; } = new Tokens();

    [Reactive, XmlIgnore] public bool IsAuthorized { get; set; } = false;
    
    public User() : this(false){}

    public User(bool withValidation = false) : base(withValidation)
    {
        this
            .WhenAnyValue(x => x.Tokens.Token)
            .Subscribe(_=>this.RaisePropertyChanged(nameof(Tokens)));
    }
}

public class RegUser : AuthUser, IRegUser
{
    #region Properties

    [Reactive] public string? Email { get; set; }

    #endregion

    #region Constructors

    public RegUser() : this(false){}
    
    public RegUser(bool withValidation = false) : base(withValidation){}

    #endregion
    
    #region Methods

    protected override void SetValidation()
    {
        this.ValidationRule(
            viewModel => viewModel.Email,
            email =>
            {
                if (string.IsNullOrEmpty(email))
                    return false;
                try
                {
                    new MailAddress(email);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            },
            "invalid email format");
        
        base.SetValidation();
    }

    #endregion
}

public class AuthUser : ReactiveValidationObject, IAuthUser
{
    #region Properties

    [Reactive] public int Id { get; set; }
    
    [Reactive] public string? Name { get; set; }
    
    [XmlIgnore] [Reactive] public string? Password { get; set; }

    #endregion
    
    #region Constructors

    public AuthUser() : this(false){}
    
    public AuthUser(bool withValidation = false)
    {
        if (withValidation)
            SetValidation();
    }

    #endregion
    
    #region Methods
    
    protected virtual void SetValidation()
    {
        this.ValidationRule(
            viewModel => viewModel.Name, 
            name => name?.Length is >= 3 and <= 10,
            "Name must be more than 3 and less than 10");
        
        this.ValidationRule(
            viewModel => viewModel.Name,
            name =>
            {
                if (string.IsNullOrEmpty(name))
                    return false;

                return new Regex(@"^[A-Za-z]+$").IsMatch(name);
            },
            "Name must contain only English letters");

        

        this.ValidationRule(
            viewModel => viewModel.Password, 
            pass => pass?.Length is >= 8 and <= 16,
            "Password must be more than 8 and less than 16");
        
        
        this.ValidationRule(
            viewModel => viewModel.Password, 
            pass =>
            {
                if (string.IsNullOrEmpty(pass))
                    return false;
                    
                return new Regex(@"[A-Z]+").IsMatch(pass);
            },
            "Password must have big letters");
        
        this.ValidationRule(
            viewModel => viewModel.Password, 
            pass =>
            {
                if (string.IsNullOrEmpty(pass))
                    return false;
                    
                return new Regex(@"[0-9]+").IsMatch(pass);
            },
            "Password must have numbers");
        
        this.ValidationRule(
            viewModel => viewModel.Password, 
            pass =>
            {
                if (string.IsNullOrEmpty(pass))
                    return false;
                    
                return new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]").IsMatch(pass);
            },
            "Password must have symbols");
    }
    
    #endregion
}