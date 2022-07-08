using Jay.Core.Application.Helpers;
using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Common;
using Jay.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jay.Infrastructure.Persistence.Contexts
{
    public class JayContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContext;

        public JayContext(DbContextOptions<JayContext> options, IHttpContextAccessor httpContext) : base(options)
        {
            _httpContext = httpContext;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = _httpContext.HttpContext.Session.Get<UserViewModel>("user").UserName;
                        break;

                    case EntityState.Modified:
                        entry.Entity.Modified = DateTime.Now;
                        entry.Entity.ModifiedBy = _httpContext.HttpContext.Session.Get<UserViewModel>("user").UserName;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User> Users { get; set; }
        //public DbSet<Friend> Friends { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            #region tables
            model.Entity<User>().ToTable("Users");
            //model.Entity<Friend>().ToTable("Friends");
            model.Entity<Post>().ToTable("Posts");
            model.Entity<Comment>().ToTable("Comments");
            #endregion

            #region keys
            model.Entity<User>().HasKey(t => t.Id);
            model.Entity<Friend>().HasKey(t => t.Id);
            model.Entity<Post>().HasKey(t => t.Id);
            model.Entity<Comment>().HasKey(t => t.Id);
            #endregion

            #region relations
            //User-Publications
            model.Entity<User>()
                .HasMany<Post>(user=>user.Posts)
                .WithOne(pub=>pub.User)
                .HasForeignKey(pub=>pub.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Publication-Comments
            model.Entity<Post>()
                .HasMany<Comment>(pub => pub.Comments)
                .WithOne(com => com.Posts)
                .HasForeignKey(com => com.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            //User-Comments
            model.Entity<User>()
                .HasMany<Comment>(user => user.Comments)
                .WithOne(com => com.User)
                .HasForeignKey(com => com.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Friends
            model.Entity<User>()
                .HasMany(user => user.Friends)
                .WithMany(fr => fr.Users)
                .UsingEntity<Friend>(
                    j => j
                        .HasOne(uf => uf.JUser)
                        .WithMany(u => u.UserFriends)
                        .HasForeignKey(uf => uf.UserId),
                    j => j
                        .HasOne(fu => fu.JFriend)
                        .WithMany(f => f.FriendsUser)
                        .HasForeignKey(fu => fu.FriendId),
                    j =>
                    {
                        j.ToTable("Friends");
                        j.HasKey(t => new { t.UserId, t.FriendId });
                    });
            //Si esta cosa sirve me van a tener que besar la mano
            //Y si no sirve es culpa de Microsoft
            #endregion

            #region prop config

            #region users
            model.Entity<User>().Property(t => t.Name).IsRequired();
            model.Entity<User>().Property(t => t.LastName).IsRequired();
            model.Entity<User>().Property(t => t.Phone).IsRequired();
            model.Entity<User>().Property(t => t.ProfilePicture).IsRequired();
            model.Entity<User>().Property(t => t.Email).IsRequired();
            model.Entity<User>().Property(t => t.Username).IsRequired();
            model.Entity<User>().Property(t => t.Password).IsRequired();
            model.Entity<User>().Property(t => t.Active).IsRequired();
            model.Entity<User>().Property(t => t.Active).HasDefaultValue(false);
            #endregion

            #region posts
            model.Entity<Post>().Property(t => t.UserId).IsRequired();
            model.Entity<Post>().Property(t => t.Created).IsRequired();
            model.Entity<Post>().Property(t => t.PostTitle).IsRequired();
            #endregion

            #region comments
            model.Entity<Comment>().Property(t => t.Text).IsRequired();
            model.Entity<Comment>().Property(t => t.UserId).IsRequired();
            model.Entity<Comment>().Property(t => t.PostId).IsRequired();
            model.Entity<Comment>().Property(t => t.Created).IsRequired();
            #endregion

            //#region friends
            //model.Entity<Friend>().Property(t => t.UserId).IsRequired();
            //model.Entity<Friend>().Property(t => t.FriendId).IsRequired();
            //#endregion

            #endregion
        }
    }
}
