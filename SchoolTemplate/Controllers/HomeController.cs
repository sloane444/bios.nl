using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SchoolTemplate.Database;
using SchoolTemplate.Models;

namespace SchoolTemplate.Controllers
{
    public class PersonModel
    {
        public string Voornaam { get; set; }
        
            [Required]
        public string Achternaam { get; set; }
        
            [Required]
            [EmailAddress]
        public string Email { get; set; }

        public DateTime Geboortedatum{ get; set; }

    }
    public class HomeController : Controller
    {
        // zorg ervoor dat je hier je gebruikersnaam (leerlingnummer) en wachtwoord invult
        string connectionString = "Server=172.16.160.21;Port=3306;Database=110032;Uid=110032;Pwd=YOUsTUBi;";

        public IActionResult Index()
        {
            List<Product> products = new List<Product>();
            // uncomment deze regel om producten uit je database toe te voegen
            products = GetProducts();

            return View(products);
        }

        private List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from product", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product p = new Product
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Calorieen = float.Parse(reader["calorieen"].ToString()),
                            Formaat = reader["Formaat"].ToString(),
                            Gewicht = Convert.ToInt32(reader["Gewicht"].ToString()),
                            Prijs = Decimal.Parse(reader["Prijs"].ToString())
                        };
                        products.Add(p);
                    }
                }
            }

            return products;
        }

        private List<Film> GetFilms()
        {
            List<Film> films = new List<Film>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from film", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Film p = new Film
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Datum = DateTime.Parse(reader["Datum"].ToString())
                        };
                        films.Add(p);
                    }
                }
            }

            return films;
        }

        private Film GetFilms(string id)
        {
            List<Film> films = new List<Film>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from product where id = {id}", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Film p = new Film
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Datum = DateTime.Parse(reader["Datum"].ToString())
                        };
                        films.Add(p);
                    }
                }
            }

            return films[0];
        }

        [Route("film/{id}")]
        public IActionResult Film(string id)
        {
            ViewData["id"] = id;

            return View();
        }

        [Route("film/{id}")]
        public IActionResult Films(string id)
        {
            var model = GetFilms(id);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

            [Route("films")]
        public IActionResult Films()
        {
            var films = GetFilms();
            return View(films);
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("gelukt")]
        public IActionResult Gelukt()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("contact")]
        [HttpPost]
        public IActionResult Contact(PersonModel model)
        {
            if(!ModelState.IsValid)
              return View(model);

            SavePerson(model);

            ViewData["formsucces"] = "oke";

            return Redirect("/gelukt");
        }

        private void SavePerson(PersonModel person)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO klant(naam, achternaam, emailadres, geb_datum) VALUES(?voornaam,?achternaam,?email,?geb_datum)", conn);

                cmd.Parameters.Add("?voornaam", MySqlDbType.VarChar).Value = person.Voornaam;
                cmd.Parameters.Add("?achternaam", MySqlDbType.VarChar).Value = person.Achternaam;
                cmd.Parameters.Add("?email", MySqlDbType.VarChar).Value = person.Email;
                cmd.Parameters.Add("?geb_datum", MySqlDbType.Date).Value = person.Geboortedatum;
                cmd.ExecuteNonQuery();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}

