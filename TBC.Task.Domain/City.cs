using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBC.Task.Domain.Interfaces.Entities;

namespace TBC.Task.Domain;

public class City : IEntitiy
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
}
