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
		public DataBaseXML(string path, ref List<DBstructure>dateBase)
		{
			try
			{
				this.dataBaseFileX = XDocument.Load(path);
				dateBaseLoader(dataBaseFileX, ref dateBase);
			}
			catch (Exception)
			{
				
				throw;
			}
		}

		private void dateBaseLoader(XDocument dataBaseFileX, ref List<DBstructure>dataBase)
		{
			//TODO: load z pliku do struktury
			dataBase = (from xml in dataBaseFileX.Elements("person")
						select new DBstructure
						{
							firstName = xml.Element("name").Value,
							lastName = xml.Element("familyName").Value,
							birth = Convert.ToDateTime(xml.Element("birthDate").Value),
							peselNumber = Convert.ToInt32(xml.Element("pesel").Value),
							phoneNumber = Convert.ToInt32(xml.Element("phone").Value)
						}).ToList();

		}
		public DataBaseXML()
		{
			try
			{
				this.dataBaseFileX = new XDocument(
					new XDeclaration("1.0", "utf-8", "yes"),
					new XElement("persons"));	
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void addToDataBaseXML(DBstructure record)
		{
			try
			{
				this.dataBaseFileX.Element("persons").Add(
					new XElement("person",
						//new XElement("ID", record.ID), 
						//TODO:  id? static zapisywany czy nadawany dynamicznie? Raczej dynamicznie? Check-it
						new XElement("pesel",record.peselNumber),
						new XElement("name",record.firstName),
						new XElement("familyName",record.lastName),
						new XElement("birthDate",record.birth),
						new XElement("phone",record.phoneNumber)
						)
					);
			}
			catch (Exception)
			{
				throw;
			}
		}
		internal void Save(string p)
		{
			this.dataBaseFileX.Save(p);
			Logger.LogInstance.LogInfo(string.Format("File saved in {0}", p));
		}
	}
}
