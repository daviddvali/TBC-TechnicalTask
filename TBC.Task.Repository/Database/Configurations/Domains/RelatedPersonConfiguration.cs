using Microsoft.EntityFrameworkCore;
using TBC.Task.Domain;
using TBC.Task.Repository.Database.Interfaces;

namespace TBC.Task.Repository.Database.Configurations.Domains;

internal class RelatedPersonConfiguration : IEntityConfiguration
{
	private ModelBuilder _modelBuilder;

	public RelatedPersonConfiguration(ModelBuilder modelBuilder) =>
		_modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

	public bool Configure()
	{
		_modelBuilder.Entity<RelatedPerson>()
			.HasKey(e => new { e.FromId, e.ToId });

		_modelBuilder.Entity<RelatedPerson>()
			.HasOne(e => e.From)
			.WithMany(e => e.RelatedTo)
			.HasForeignKey(e => e.FromId)
			.OnDelete(DeleteBehavior.NoAction);
		_modelBuilder.Entity<RelatedPerson>()
			.HasOne(e => e.To)
			.WithMany(e => e.RelatedFrom)
			.HasForeignKey(e => e.ToId)
			.OnDelete(DeleteBehavior.NoAction);

		return true;
	}
}
