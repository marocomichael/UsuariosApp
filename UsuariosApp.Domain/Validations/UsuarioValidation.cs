using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Validations
{
    /// <summary>
    /// Classe de validação para a entidade Usuário
    /// </summary>
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            RuleFor(u => u.Nome)
                .NotEmpty()
                .WithMessage("O nome do usuário é obrigatório.")
                .MinimumLength(6)
                .WithMessage("O nome deve ter pelo menos 6 caracteres.")
                .MaximumLength(100)
                .WithMessage("O nome deve ter no máximo 100 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("O email do usuário é obrigatório.")
                .EmailAddress()
                .WithMessage("Informe um endereço de email válido");

            RuleFor(u => u.Senha)
                .NotEmpty()
                .WithMessage("A senha do usuário é obrigatória")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
                .WithMessage("A senha deve ter pelo menos 1 letra maiúscula, 1 letra minúscula, 1 número, 1 caractere especial e no mínimo 8 caracteres.");
        }
    }
}
