using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Infra.Data.Mappings
{
    public class PerfilMap : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            //Nome da tabela
            builder.ToTable("PERFIL");

            //Chave primária
            builder.HasKey(p => p.Id);

            //Mapeamentos dos campos
            builder.Property(p => p.Id).HasColumnName("ID");
            builder.Property(p => p.Nome).HasColumnName("NOME").HasMaxLength(50).IsRequired();

            //Definindo os campos com valor único na tabela
            builder.HasIndex(p => p.Nome).IsUnique();
        }
    }
}
