using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data.Configurations;

public sealed class TaskItemConfiguration
    : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(
        EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("tasks");

        builder.HasKey(task => task.Id);

        builder.Property(task => task.Id)
            .HasColumnName("id");

        builder.Property(task => task.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(task => task.Title)
            .HasColumnName("title")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(task => task.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(task => task.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(task => task.Priority)
            .HasColumnName("priority")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(task => task.DueDate)
            .HasColumnName("due_date");

        builder.Property(task => task.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(task => task.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(task => task.CompletedAt)
            .HasColumnName("completed_at");

        builder.HasIndex(task => task.UserId);

        builder.HasIndex(task => task.Status);

        builder.HasIndex(task => task.Priority);

        builder.HasIndex(task => task.DueDate);
    }
}