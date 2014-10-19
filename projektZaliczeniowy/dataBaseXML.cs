using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace projektZaliczeniowy
{
	public class dataBaseXML
	{
		public XmlDocument dataBaseFile;
		public XDocument dataBaseFileX; 
		public string dataBasePath = string.Empty;

		/// <summary>
		/// konstruktor dla istniejącej bazy
		/// </summary>
		/// <param name="openedPath"></param>
		public dataBaseXML(string openedPath)
		{
			this.dataBasePath = openedPath;
			try
			{
				this.dataBaseFile = new XmlDocument();
				this.dataBaseFile.Load(dataBasePath);
			}
			catch (Exception ex) 
			{
				throw ex;
			}
		}
		public dataBaseXML()
		{
			try
			{
				this.dataBaseFileX = new XDocument(
					new XDeclaration("1.0", "utf-8", "yes"));	
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
						new XElement("ID", record.ID),
						new XElement("pesel",record.pesel),
						new XElement("name",record.name),
						new XElement("familyName",record.familyName),
						new XElement("birthDate",record.birthDate),
						new XElement("phone",record.phone)
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
			//this.dataBaseFile.Save(p);
		}
	}
}
