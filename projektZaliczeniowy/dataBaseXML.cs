using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace projektZaliczeniowy
{
	public class DataBaseXML
	{
		public XDocument dataBaseFileX;
		public string dataBasePath = string.Empty;
		public DataBaseXML(string path)
		{
			try
			{
				this.dataBaseFileX = XDocument.Load(path);
			}
			catch (Exception)
			{
				throw;
			}
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
		public List<DBStructureViewModel> LoadDB()
		{
			try
			{
				return dateBaseLoader(dataBaseFileX);
			}
			catch (Exception)
			{
				throw;
			}
		}
		/// <summary>
		/// bierzemy XDocument z pliku, wrzucamy przez ref do listy
		/// </summary>
		/// <param name="dataBaseFileX">plik wczytany w main</param>
		/// <param name="dataBase">main list DB structure</param>
		private List<DBStructureViewModel> dateBaseLoader(XDocument dataBaseFileX)
		{
			//TODO: walidacja danych tutaj?
			try
			{
				var tmpXML = dataBaseFileX.Element("persons");
				return (from xml in tmpXML.Elements("person")
						select new DBStructureViewModel
						{
							Id = Convert.ToInt32(xml.Element("ID").Value),
							Name = xml.Element("name").Value,
							FamilyName = xml.Element("familyName").Value,
							BirthDate = Convert.ToDateTime(xml.Element("birthDate").Value),
							Pesel = xml.Element("pesel").Value,
							Phone = xml.Element("phone").Value
						}).ToList();
			}
			catch (Exception)
			{
				throw;
			}
		}		
		public void addToDataBaseXML(DBStructureViewModel record)
		{
			try
			{
				this.dataBaseFileX.Element("persons").Add(
					new XElement("person",
						new XElement("ID", record.Id), 
						new XElement("pesel", record.Pesel),
						new XElement("name", record.Name),
						new XElement("familyName", record.FamilyName),
						new XElement("birthDate", record.BirthDate),
						new XElement("phone", record.Phone)
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
			Logger.Instance.LogInfo(string.Format("File saved in {0}", p));
		}
	}
}
