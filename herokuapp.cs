using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace herokuappnuit.tests
{
    public class Users
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("first name")]
        public string first_name { get; set; }
        [JsonProperty("last name")]
        public string last_name { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("ip address")]
        public string ip_address { get; set; }
        [JsonProperty("latitude")]
        public object latitude { get; set; }
        [JsonProperty("longitude")]
        public object longitude { get; set; }
    }

    
    public class HerokuappTests
    {
        
        private readonly string RestUrl = "http://bpdts-test-app-v2.herokuapp.com";
        [SetUp]
        public void Setup()
        {            
        }

       [Test]
        [TestCase("London", TestName = "Check user exist in London")]
        [TestCase("Birmingham", TestName = "Check user exist in Birmingham")]
        public void GetUsersbyCity(string city) 
        {
          
            RestClient restClient = new RestClient(RestUrl);
            RestRequest restRequest = new RestRequest($"/city/{city}/users", Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            JsonDeserializer deserial = new JsonDeserializer();

            var response = deserial.Deserialize<List<Users>>(restResponse);
            if (city == "London")
            {
                Assert.That(response, Is.Not.Empty);
            }
            else
            { Assert.That(response, Is.Empty); }
            
        }

        [Test]
        [TestCase("/instructions", HttpStatusCode.OK, TestName = "Check instructions URL possitive test")]
        [TestCase("/instruct", HttpStatusCode.NotFound, TestName = "Check instructions URL negative test")]
        public void GetInstructions(string instructUrl,HttpStatusCode expectedHttpStatusCode)
        {
            RestClient restClient = new RestClient(RestUrl);

            //Creating request to get data from server
            RestRequest restRequest = new RestRequest($"{instructUrl}", Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // assert
            Assert.That(restResponse.StatusCode, Is.EqualTo(expectedHttpStatusCode));

        }

        [Test]
        [TestCase("/users", HttpStatusCode.OK, TestName = "Check instructions URL possitive test")]
        [TestCase("/user", HttpStatusCode.NotFound, TestName = "Check instructions URL negative test")]
        public void GetUsersStatus(string usersUrl, HttpStatusCode expectedHttpStatusCode) {

            RestClient restClient = new RestClient(RestUrl);
            RestRequest restRequest = new RestRequest($"{usersUrl}", Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // assert
            Assert.That(restResponse.StatusCode, Is.EqualTo(expectedHttpStatusCode));

          


        }

        [Test]
        [TestCase("/users", TestName = "Check if users exist")]
        public void GetUsers(string usersUrl)
        {

            RestClient restClient = new RestClient(RestUrl);
            RestRequest restRequest = new RestRequest($"{usersUrl}", Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

          
            JsonDeserializer deserial = new JsonDeserializer();

            var response = deserial.Deserialize<List<Users>>(restResponse);
            Assert.That(response, Is.Not.Empty);


        }

        [Test]
        [TestCase("1", TestName = "Check if user with id 1 exist")]
        public void GetUsersbyId(string userid)
        {

            RestClient restClient = new RestClient(RestUrl);
            RestRequest restRequest = new RestRequest($"/user/{userid}", Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);


            JsonDeserializer deserial = new JsonDeserializer();

            var response = deserial.Deserialize<List<Users>>(restResponse);
            int id = Convert.ToInt32(userid);
            Assert.That(response[0].id, Is.EqualTo(id));


        }
    }
}