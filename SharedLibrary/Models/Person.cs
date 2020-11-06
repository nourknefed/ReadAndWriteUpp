using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
   public class Person
    {
        [JsonProperty(propertyName: "firstname")]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "lastname")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "age")]
        public string Age { get; set; }

        [JsonProperty(propertyName: "country")]
        public string Country { get; set; }

        [JsonProperty(propertyName: "city")]
        public string City { get; set; }

        [JsonProperty(propertyName: "jobb")]
        public string Jobb { get; set; }

        public string FullName => $"{FirstName} {LastName} ";

        public Person()
        {
            
        }


        public Person(string firstname, string lastname, string age, string country, string city, string jobb)
        {
            FirstName = firstname;
            LastName = lastname;
            Age = age;
            Country = country;
            City = city;
            Jobb = jobb;
        }



    }
}

