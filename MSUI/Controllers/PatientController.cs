﻿//using Microsoft.AspNetCore.Mvc;

//namespace MSUI.Controllers
//{
//    public class PatientController : Controller
//    {
//        private readonly HttpClient _httpClient;
//        public PatientController(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//            _httpClient.BaseAddress = new Uri("https://Localhost:7001");
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using MSUI.Models;
using Newtonsoft.Json;
using System.Text;

namespace MSUI.Controllers
{
    public class PatientController : Controller
    {
        private readonly HttpClient _httpClient;

        public PatientController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://Localhost:7002");
            //    _httpClient.DefaultRequestHeaders.Accept.Clear();
            //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //}
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Patient");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<Patient>>(responseData);
                return View(patients);
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> GetPatient(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Patient/{id}");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var patient = JsonConvert.DeserializeObject<Patient>(responseData);
                return View(patient);
            }
            else
            {
                return View("Error");
            }
        }

        ///// <summary>
        /////permet d' afficher le formulaire "Create"
        ///// </summary>
        ///// <param name="patient"></param>
        ///// <returns></returns>
        /// 
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient); // Retourne la vue avec les erreurs de validation
            }

            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/api/Patient", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}. Détails : {errorMessage}");
            }
        }

        public async Task<IActionResult> UpdatePatient(int id, Patient patient)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Patient/{id}", patient);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllPatients");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Patient/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }
    }
}