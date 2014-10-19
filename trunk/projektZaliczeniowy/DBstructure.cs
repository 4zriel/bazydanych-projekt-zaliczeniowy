using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektZaliczeniowy
{
	public class DBstructure
	{
		public int ID { get; set; }
		public string name { get; set; }
		public string familyName { get; set; }
		public DateTime birthDate { get; set; }
		public int phone { get; set; }
		public int pesel { get; set; }
	}
}
