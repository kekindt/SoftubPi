using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace softubWeb.Models;

public partial class ConfigContext : DbContext
{
    public ConfigContext()
    {
    }

    public ConfigContext(DbContextOptions<ConfigContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ConfigValue> ConfigValues { get; set; }

  //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlite("Data Source=c:\\projects\\softubConfig\\config.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfigValue>(entity =>
        {
            entity.HasKey(e => e.ConfigVersion);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
