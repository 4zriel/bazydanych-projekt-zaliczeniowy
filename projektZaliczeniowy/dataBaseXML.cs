using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace projektZaliczeniowy
{
	public class DataBaseXML
	{
		public XDocument dataBaseFileX; 
		public string dataBasePath = string.Empty;

		/// <summary>
		/// konstruktor dla istniejącej bazy
		/// </summary>
		/// <param name="openedPath"></param>

		public DataBaseXML()
		{
			try
			{
				this.dataBaseFileX = new XDocument(
					new XDeclaration("1.0", "utf-8", "yes"),
					new XElement("persons"));	
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void addToDataBaseXML(DBstructure record)
		{
			try
			{
				this.dataBaseFileX.Element("persons").Add(
					new XElement("person",
						//new XElement("ID", record.ID), TODO: static zapisywany czy nadawany dynamicznie? Raczej dynamicznie? Check-it
						new XElement("pesel",record.peselNumber),
						new XElement("name",record.firstName),
						new XElement("familyName",record.lastName),
						new XElement("birthDate",record.birth),
						new XElement("phone",record.phoneNumber)
						)
					);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		internal void Save(string p)
		{
			this.dataBaseFileX.Save(p);
		}
	}
}
