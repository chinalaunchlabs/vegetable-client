using System;

namespace Vegeta.Models
{
	public class Vegetable
	{
	
		public int ID { get; set; }
		public string Name { get; set; }
		public Photo Photo { get; set; }
		public DateTime UpdatedAt { get; set; }
		public DateTime CreatedAt { get; set; }
	
	}

	public class Photo {
		public string Url { get; set; }
	}
}

