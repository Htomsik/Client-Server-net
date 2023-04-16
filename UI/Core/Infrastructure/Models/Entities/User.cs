﻿using System.Net.Mail;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Interfaces.Entities;
using Interfaces.Other;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace Core.Infrastructure.Models.Entities;

public class User : AuthUser, IUser
{
    [Reactive] public ITokens? Tokens { get; set; } = new Tokens();
}

public class AuthUser : ReactiveValidationObject, IAuthUser
{
    [Reactive] public int Id { get; set; }
    
    [Reactive] public string? Name { get; set; }
    
    [Reactive] public string? Email { get; set; }
    
    [XmlIgnore] [Reactive] public string? Password { get; set; }
    
    
    public AuthUser(bool withValidation = false)
    {
        if (withValidation)
            SetValidation();
    }

    #region Methods
    
    private void SetValidation()
    {
        this.ValidationRule(
            viewModel => viewModel.Name, 
            name => name?.Length is >= 3 and <= 10,
            "Name must be more than 3 and less than 10");
        
        this.ValidationRule(
            viewModel => viewModel.Name,
            name =>
            {
                if (name is null)
                    return false;

                return new Regex(@"^[A-Za-z]+$").IsMatch(name);
            },
            "Name must contain only English letters");

        this.ValidationRule(
            viewModel => viewModel.Email,
            email =>
            {
                if (email is null)
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

        this.ValidationRule(
            viewModel => viewModel.Password, 
            pass => pass?.Length is >= 8 and <= 16,
            "Password must be more than 8 and less than 16");
        
        
        this.ValidationRule(
            viewModel => viewModel.Password, 
            pass =>
            {
                if (pass is null)
                    return false;
                    
                return new Regex(@"[A-Z]+").IsMatch(pass);
            },
            "Password must have big letters");
        
        this.ValidationRule(
            viewModel => viewModel.Password, 
            pass =>
            {
                if (pass is null)
                    return false;
                    
                return new Regex(@"[0-9]+").IsMatch(pass);
            },
            "Password must have numbers");
        
        this.ValidationRule(
            viewModel => viewModel.Password, 
            pass =>
            {
                if (pass is null)
                    return false;
                    
                return new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]").IsMatch(pass);
            },
            "Password must have symbols");
    }
    
    #endregion
}