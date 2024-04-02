using System;
using System.IO;
using System.Xml;

namespace EventOrganizer
{
    class Event
    {
        public string Name { get; set; }
        public string DateTime { get; set; }
        public string Description { get; set; }
        public string UniqueID { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Виберіть опцію:");
                Console.WriteLine("1. Додати подію");
                Console.WriteLine("2. Редагувати подію");
                Console.WriteLine("3. Видалити подію");
                Console.WriteLine("4. Показати всі події");
                Console.WriteLine("5. Вийти");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EventOrginizer.AddEvent();
                        break;
                    case "2":
                        EventOrginizer.EditEvent();
                        break;
                    case "3":
                        EventOrginizer.DeleteEvent();
                        break;
                    case "4":
                        EventOrginizer.ShowAllEvents();
                        break;
                    case "5":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Будь ласка, спробуйте ще раз.");
                        break;
                }
            }
        }

        static int GetNextEventID(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            int maxID = 0;

            foreach (XmlNode node in doc.SelectNodes("/Events/Event"))
            {
                int currentID = int.Parse(node.SelectSingleNode("UniqueID").InnerText);
                if (currentID > maxID)
                {
                    maxID = currentID;
                }
            }

            return maxID + 1;
        }

        class EventOrginizer
        {

            public static void AddEvent()
            {
                Console.WriteLine("Введіть дані нової події:");
                Console.Write("Назва: ");
                string name = Console.ReadLine();
                Console.Write("Дата та час : ");
                string dateTime = Console.ReadLine();
                Console.Write("Опис: ");
                string description = Console.ReadLine();

                string fileName = "Events.xml";

                int nextEventID = GetNextEventID(fileName);

                Event newEvent = new Event { Name = name, DateTime = dateTime, Description = description, UniqueID = nextEventID.ToString() };

                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlElement eventElement = doc.CreateElement("Event");
                doc.DocumentElement.AppendChild(eventElement);

                XmlElement nameElement = doc.CreateElement("Name");
                nameElement.InnerText = newEvent.Name;
                eventElement.AppendChild(nameElement);

                XmlElement dateTimeElement = doc.CreateElement("DateTime");
                dateTimeElement.InnerText = newEvent.DateTime;
                eventElement.AppendChild(dateTimeElement);

                XmlElement descriptionElement = doc.CreateElement("Description");
                descriptionElement.InnerText = newEvent.Description;
                eventElement.AppendChild(descriptionElement);

                XmlElement uniqueIDElement = doc.CreateElement("UniqueID");
                uniqueIDElement.InnerText = newEvent.UniqueID;
                eventElement.AppendChild(uniqueIDElement);

                doc.Save(fileName);

                Console.WriteLine("Подія додана успішно.");
            }



            public static void EditEvent()
            {
                Console.WriteLine("Введіть ID події, яку ви хочете відредагувати:");
                string uniqueID = Console.ReadLine();

                XmlDocument doc = new XmlDocument();
                doc.Load("Events.xml");
                XmlNode selectedEvent = doc.SelectSingleNode("/Events/Event[UniqueID='" + uniqueID + "']");
                if (selectedEvent != null)
                {
                    Console.WriteLine("Введіть нові дані для події:");
                    Console.Write("Назва: ");
                    selectedEvent.SelectSingleNode("Name").InnerText = Console.ReadLine();
                    Console.Write("Дата та час: ");
                    selectedEvent.SelectSingleNode("DateTime").InnerText = Console.ReadLine();
                    Console.Write("Опис: ");
                    selectedEvent.SelectSingleNode("Description").InnerText = Console.ReadLine();

                    doc.Save("Events.xml");
                    Console.WriteLine("Дані події відредаговані успішно.");
                }
                else
                {
                    Console.WriteLine("Подію з таким ID не знайдено.");
                }
            }

            public static void DeleteEvent()
            {
                Console.WriteLine("Введіть ID події, яку ви хочете видалити:");
                string uniqueID = Console.ReadLine();

                XmlDocument doc = new XmlDocument();
                doc.Load("Events.xml");
                XmlNode selectedEvent = doc.SelectSingleNode("/Events/Event[UniqueID='" + uniqueID + "']");
                if (selectedEvent != null)
                {
                    selectedEvent.ParentNode.RemoveChild(selectedEvent);
                    doc.Save("Events.xml");
                    Console.WriteLine("Подія видалена успішно.");
                }
                else
                {
                    Console.WriteLine("Подію з таким ID не знайдено.");
                }
            }

            public static void ShowAllEvents()
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Events.xml");
                foreach (XmlNode node in doc.SelectNodes("/Events/Event"))
                {
                    Console.WriteLine($"Назва: {node.SelectSingleNode("Name").InnerText}");
                    Console.WriteLine($"Дата та час: {node.SelectSingleNode("DateTime").InnerText}");
                    Console.WriteLine($"Опис: {node.SelectSingleNode("Description").InnerText}");
                    Console.WriteLine($"ID: {node.SelectSingleNode("UniqueID").InnerText}\n");
                }
            }
        }
    }
}
