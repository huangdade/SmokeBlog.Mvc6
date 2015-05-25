using Microsoft.Data.Entity.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace SmokeBlog.Core.Data
{
    public partial class SmokeBlogContext
    {
        private static void MappingUser(EntityTypeBuilder<User> builder)
        {
            builder.ForSqlServer().Table("User");

            builder.Key(t => t.ID);
            builder.Property(t => t.ID).ForSqlServer().UseIdentity();

            builder.Property(t => t.UserName).Required().MaxLength(20);
            builder.Property(t => t.Password).Required().MaxLength(32);
            builder.Property(t => t.Nickname).Required().MaxLength(20);
            builder.Property(t => t.Email).Required().MaxLength(100);
            builder.Property(t => t.CreateDate).Required();
            builder.Property(t => t.Avatar).MaxLength(100);
            builder.Property(t => t.Token).MaxLength(32);
        }
    }
}
