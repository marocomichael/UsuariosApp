using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Infra.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            //Nome da tabela no banco de dados
            builder.ToTable("USUARIO");

            //Chave primária
            builder.HasKey(u => u.Id);

            //Campos da tabela
            builder.Property(u => u.Id).HasColumnName("ID");
            builder.Property(u => u.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            builder.Property(u => u.Email).HasColumnName("EMAIL").HasMaxLength(50).IsRequired();
            builder.Property(u => u.Senha).HasColumnName("SENHA").HasMaxLength(100).IsRequired();
            builder.Property(u => u.DataHoraCriacao).HasColumnName("DATAHORACRIACAO").IsRequired();
            builder.Property(u => u.PerfilId).HasColumnName("PERFILID").IsRequired();

            //Campos com valor unico
            builder.HasIndex(u => u.Email).IsUnique();

            //Relacionamento de usuário com Perfil
            builder.HasOne(u => u.Perfil) //Usuário TEM 1 Perfil
                .WithMany(p => p.Usuarios) //Perfil TEM MUITOS Usuários
                .HasForeignKey(u => u.PerfilId); //Chave estrangeira

        }
    }
}
