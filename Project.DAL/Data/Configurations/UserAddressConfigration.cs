using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Data.Configurations
{
	internal class UserAddressConfigration : IEntityTypeConfiguration<UserAddress>
	{
		public void Configure(EntityTypeBuilder<UserAddress> builder)
		{
			builder.HasKey(U => U.ApplicationUserId);


			builder.HasOne(U => U.ApplicationUser)
				.WithMany();
				
		}
	}
}
