using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Life.Domain;
using Life.Domain.Identity;

namespace Life.Repository
{
    public class LifeContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        public LifeContext(DbContextOptions<LifeContext> options) : base(options) {}

        //Cria as tabelas do banco de dados com o DbSet
        //Se precisar criar outra tabela é só repetir o processo para as classes que você desejar que tenham uma tabela
        public DbSet<Consulta> Consultas {get; set;}

        public DbSet<Medico> Doctors {get; set;}

        public DbSet<DoctorConsulta> DoctorsConsultas {get; set;}
    
        public DbSet<Contato> Contatos {get; set;}

        public DbSet<RedeSocial> RedesSociais {get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole => {
                //Cria a relação entre os usuários e os seus cargos
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                //Cria um relacionamento N pra N entre os cargos(Roles) e os usuários(Users)
                userRole.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();

                //Cria um relacionamento de N pra N entre os usuários (Users) e os cargos (Roles)
                userRole.HasOne(ur => ur.User).WithMany(r => r.UserRoles).HasForeignKey(r => r.UserId).IsRequired();
            });

            //Criando relacionamento entre a Consulta e o Médico
            modelBuilder.Entity<DoctorConsulta>().HasKey(DC => new {DC.ConsultaId, DC.DoctorId});
        }
    }    
}