﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RDF.GL.Data;

#nullable disable

namespace RDF.GL.Migrations
{
    [DbContext(typeof(ProjectGLDbContext))]
    [Migration("20240819021905_AdjustUserEntity")]
    partial class AdjustUserEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RDF.GL.Domain.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AddedBy")
                        .HasColumnType("integer")
                        .HasColumnName("added_by");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("integer")
                        .HasColumnName("modified_by");

                    b.Property<List<string>>("Permissions")
                        .HasColumnType("text[]")
                        .HasColumnName("permissions");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserRoleName")
                        .HasColumnType("text")
                        .HasColumnName("user_role_name");

                    b.HasKey("Id")
                        .HasName("pk_user_roles");

                    b.HasIndex("AddedBy")
                        .HasDatabaseName("ix_user_roles_added_by");

                    b.HasIndex("ModifiedBy")
                        .HasDatabaseName("ix_user_roles_modified_by");

                    b.ToTable("user_roles", (string)null);
                });

            modelBuilder.Entity("RDF.GL.Domain.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("IdNumber")
                        .HasColumnType("text")
                        .HasColumnName("id_number");

                    b.Property<string>("IdPrefix")
                        .HasColumnType("text")
                        .HasColumnName("id_prefix");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text")
                        .HasColumnName("middle_name");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Sex")
                        .HasColumnType("text")
                        .HasColumnName("sex");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.Property<int?>("UserRoleId")
                        .HasColumnType("integer")
                        .HasColumnName("user_role_id");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("UserRoleId")
                        .HasDatabaseName("ix_users_user_role_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("RDF.GL.Domain.UserRole", b =>
                {
                    b.HasOne("RDF.GL.Domain.Users", "AddedByUser")
                        .WithMany()
                        .HasForeignKey("AddedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_users_added_by");

                    b.HasOne("RDF.GL.Domain.Users", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_users_modified_by");

                    b.Navigation("AddedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("RDF.GL.Domain.Users", b =>
                {
                    b.HasOne("RDF.GL.Domain.UserRole", "UserRole")
                        .WithMany("Users")
                        .HasForeignKey("UserRoleId")
                        .HasConstraintName("fk_users_user_roles_user_role_id");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("RDF.GL.Domain.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
