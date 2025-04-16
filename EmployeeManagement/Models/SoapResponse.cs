using System.Collections.Generic;
using System.Xml.Serialization;

namespace EmployeeManagement.Models
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapResponse
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapBody Body { get; set; }
    }

    public class SoapBody
    {
        [XmlElement(ElementName = "Response", Namespace = "http://example.com/soap")]
        public SoapData Response { get; set; }
    }

    public class SoapData
    {
        [XmlElement("Message")]
        public string Message { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }

        [XmlArray("Users")]
        [XmlArrayItem("User")]
        public List<SoapUser> Users { get; set; }
    }
    public class SoapUser
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }
    }
}
