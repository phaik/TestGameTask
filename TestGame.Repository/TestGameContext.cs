using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestGame.Repository.DAOs;

namespace TestGame.Repository
{
    public class TestGameContext : DbContext, ITestGameContext
    {
        public TestGameContext(DbContextOptions<TestGameContext> options) : base(options)
        {
            
        }

        public DbSet<ClientDAO> Clients { get; set; }

        public DbSet<LobbyDAO> Lobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientDAO>(c => BuildClients(c));
            modelBuilder.Entity<LobbyDAO>(l => BuildLobbies(l));
        }

        private void BuildClients(EntityTypeBuilder<ClientDAO> entity)
        {
            entity.ToTable("Clients")
               .HasKey(p => p.Id).HasName("PK_Clients_Id");
        }

        private void BuildLobbies(EntityTypeBuilder<LobbyDAO> entity)
        {
            entity.ToTable("Lobbies")
               .HasKey(p => p.Id).HasName("PK_Lobbies_Id");

            entity.HasOne(d => d.Host)
                .WithMany(p => p.LobbyHostIdNavigations)
                .HasForeignKey(d => d.HostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lobbies_HostId");

            entity.HasOne(d => d.SecondClient)
                .WithMany(p => p.LobbySecondClientIdNavigations)
                .HasForeignKey(d => d.SecondClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lobbies_SecondClientId");

            entity.HasOne(d => d.Winner)
                .WithMany(p => p.LobbyWinnerIdNavigations)
                .HasForeignKey(d => d.WinnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lobbies_WinnerId");

            entity.Property(e => e.HostHealth).HasDefaultValueSql("10");

            entity.Property(e => e.SecondClientHealth).HasDefaultValueSql("10");

            entity.Property(e => e.MovesCount).HasDefaultValueSql("0");
        }

        public void Migrate()
        {
            this.Database.Migrate();
        }
    }
}
