using Faculty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Faculty.Filters
{
    public class XmlFileManager
    {
        public void XmlSave(LogData data) {

            XmlDocument xDoc = new XmlDocument();
            try
            {

            xDoc.Load("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/Faculty/Faculty/XmlLogStorage/XmlLog.xml");
            
            XmlElement xRoot = xDoc.DocumentElement;


            XmlElement session = xDoc.CreateElement("Session");
            XmlElement userName = xDoc.CreateElement("UserName");
            XmlElement userAction = xDoc.CreateElement("UserAction");
            XmlElement Date = xDoc.CreateElement("Date");

            XmlText name = xDoc.CreateTextNode($"{data.UserName}");
            XmlText action = xDoc.CreateTextNode($"{data.UserAction}");
            XmlText date = xDoc.CreateTextNode($"{data.Date}");

            userName.AppendChild(name);
            userAction.AppendChild(action);
            Date.AppendChild(date);

            session.AppendChild(userName);
            session.AppendChild(userAction);
            session.AppendChild(Date);
            xRoot.AppendChild(session);

            xDoc.Save("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/Faculty/Faculty/XmlLogStorage/XmlLog.xml");

            }
            catch (FileNotFoundException ex) {
                throw ex;
            }
        }

        public void XmlSave(ExceptionLogModel data)
        {

            XmlDocument xDoc = new XmlDocument();
            try
            {

                xDoc.Load("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/Faculty/Faculty/XmlLogStorage/XmlLog.xml");

                XmlElement xRoot = xDoc.DocumentElement;


                XmlElement error = xDoc.CreateElement("Exception");
                XmlElement userName = xDoc.CreateElement("UserName");
                XmlElement exMessage = xDoc.CreateElement("ExceptionMessage");
                XmlElement userAction = xDoc.CreateElement("ControllerName");
                XmlElement stackTrace = xDoc.CreateElement("ExceptionStackTrace");
                XmlElement Date = xDoc.CreateElement("LogTime");

                XmlText name = xDoc.CreateTextNode($"{data.UserName}");
                XmlText exMes = xDoc.CreateTextNode($"{data.ExceptionMessage}");
                XmlText action = xDoc.CreateTextNode($"{data.ControllerName}");
                XmlText sTrace = xDoc.CreateTextNode($"{data.ExceptionStackTrace}");
                XmlText date = xDoc.CreateTextNode($"{data.LogTime}");

                userName.AppendChild(name);
                exMessage.AppendChild(exMes);
                userAction.AppendChild(action);
                stackTrace.AppendChild(sTrace);
                Date.AppendChild(date);

                error.AppendChild(userName);
                error.AppendChild(exMessage);
                error.AppendChild(userAction);
                error.AppendChild(stackTrace);
                error.AppendChild(Date);
                xRoot.AppendChild(error);

                xDoc.Save("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/Faculty/Faculty/XmlLogStorage/XmlLog.xml");


            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }

        public IEnumerable<LogData> GetXmlData() {

        try { 

            List<LogData> data = new List<LogData>();
            XmlDocument doc = new XmlDocument();

            doc.Load("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/Faculty/Faculty/XmlLogStorage/XmlLog.xml");


                foreach (XmlNode node in doc.SelectNodes("/Sessions/Session"))
            {
                data.Add(new LogData
                {
                    UserName = node["UserName"].InnerText,
                    UserAction = node["UserAction"].InnerText,
                    Date = DateTime.Parse(node["Date"].InnerText)
                });
            }

            return data;
        }

       catch (FileNotFoundException ex) {
                throw ex;
            }
}

        public IEnumerable<ExceptionLogModel> GetErrorData()
        {

            try
            {

                List<ExceptionLogModel> data = new List<ExceptionLogModel>();
                XmlDocument doc = new XmlDocument();

                doc.Load("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/Faculty/Faculty/XmlLogStorage/XmlLog.xml");


                foreach (XmlNode node in doc.SelectNodes("/Sessions/Exception"))
                {
                    data.Add(new ExceptionLogModel
                    {
                        UserName = node["UserName"].InnerText,
                        ExceptionMessage= node["ExceptionMessage"].InnerText,
                        ControllerName = node["ControllerName"].InnerText,
                        ExceptionStackTrace = node["ExceptionStackTrace"].InnerText,
                        LogTime = DateTime.Parse(node["Date"].InnerText)
                    });
                }

                return data;
            }

            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }
    }

    class FileNotFoundException : Exception
    {
        public FileNotFoundException():base("Файл не найден")
        {
        }
    }
}