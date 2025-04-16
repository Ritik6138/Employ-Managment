using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace EmployeeManagement.Controllers
{
    /// <summary>
    /// API Controller for managing user data and providing SOAP and JSON responses.
    /// Created by Ritik Kumar.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly MockUserRepository _repo = MockUserRepository.Instance;

        /// <summary>
        /// Returns user data as a SOAP-formatted XML response.
        /// </summary>
        /// <returns>SOAP response containing user data.</returns>
        [HttpGet("soap")]
        public IActionResult GetSoapResponse()
        {
            // Fetch users from the repository and map to SOAP-compatible format
            var users = _repo.GetAllUsers().Select(u => new SoapUser
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToList();

            // Create SOAP response structure
            var soapResponse = new SoapResponse
            {
                Body = new SoapBody
                {
                    Response = new SoapData
                    {
                        Message = "User data retrieved successfully",
                        Status = "OK",
                        Users = users
                    }
                }
            };

            // Serialize SOAP response to XML
            var serializer = new XmlSerializer(typeof(SoapResponse));
            var memoryStream = new MemoryStream(); // Do not wrap this in a `using` block
            serializer.Serialize(memoryStream, soapResponse);
            memoryStream.Position = 0; // Reset the stream position to the beginning

            // Return XML file
            return File(memoryStream, "application/xml", "SoapResponse.xml"); // Specify the filename if required
        }

        /// <summary>
        /// Returns a simple JSON response with a success message and timestamp.
        /// </summary>
        /// <returns>JSON object containing message, status, and timestamp.</returns>
        [HttpGet("json")]
        public IActionResult GetJsonResponse()
        {
            var response = new
            {
                Message = "JSON response successful",
                Status = "OK",
                Timestamp = DateTime.UtcNow // Current UTC time
            };

            return Ok(response); // Return HTTP 200 with JSON payload
        }

        /// <summary>
        /// Returns all users as a JSON response.
        /// </summary>
        /// <returns>List of users in JSON format.</returns>
        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            // Fetch all users from the repository
            var users = _repo.GetAllUsers();

            return Ok(users); // Return HTTP 200 with user list
        }
    }
}
