using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp5
{
    class Program
    {
            static void Main(string[] args)
            {
                Search();
               Console.ReadKey();
            }
             class Vacanc
            {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Salary { get; set; }
            public List<string> Level = new List<string>();
            }

             class Currency
             {
            public string Kod { get; set; }
            public double Tariff { get; set; }

            }
        static void Search()
            {
                            
                List<Vacanc> vacanc = GetVacancies();
                Console.WriteLine("Зарплата >= 120000 рублей");
                foreach (var vacancy in vacanc)
                {
                    if (vacancy.Salary >= 120000)
                    {
                        Console.WriteLine("Название профессии=> " + vacancy.Name);
                        Console.WriteLine("Зарплата==> " + vacancy.Salary);
                        SetKeySkillsForVacancy(vacancy);
                        for (int i = 0; i < vacancy.Level.Count; i++)
                        {
                            Console.WriteLine("Ключевой навык " + (i + 1) + ": " + vacancy.Level[i]);
                        }
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Зарплата < 15000 рублей");
                foreach (var vacancy in vacanc)
                {
                    if (vacancy.Salary < 15000)
                    {
                        Console.WriteLine("Название профессии=> " + vacancy.Name);
                        Console.WriteLine("Зарплата==> " + vacancy.Salary);
                        SetKeySkillsForVacancy(vacancy);
                        for (int i = 0; i < vacancy.Level.Count; i++)
                        {
                            Console.WriteLine(" Навык " + (i + 1) + ": " + vacancy.Level[i]);
                        }
                    }
                }
                Console.WriteLine();
            }

            static void SetKeySkillsForVacancy(Vacanc vacancy)
            {
                string vacResponse = SendRequest("https://api.hh.ru/", "vacancies/" + vacancy.Id).Result;

                dynamic vacResults = JsonConvert.DeserializeObject<dynamic>(vacResponse);
                if (vacResults.key_skills != null)
                {
                    foreach (var key_skill in vacResults.key_skills)
                    {
                        vacancy.Level.Add((string)key_skill.name);
                    }
                }
            }

            static List<Vacanc> GetVacancies()
            {
                Console.WriteLine("Cбор данных=>");
                List<Currency> currencies = new List<Currency>();

                Console.WriteLine("Курса валют=>");
                string curRes = SendRequest("https://api.hh.ru/", "dictionaries").Result;
                dynamic curResults = JsonConvert.DeserializeObject<dynamic>(curRes);
                if (curResults.currency != null)
                {
                    foreach (var cur in curResults.currency)
                    {
                        Currency c = new Currency();
                        if (cur.code != null) c.Kod = (string)cur.code;
                        if (cur.rate != null) c.Tariff = (double)cur.rate;
                        currencies.Add(c);
                    }
                }

                List<Vacanc> vacancies = new List<Vacanc>();

                Console.WriteLine("Вакансии");
                for (int i = 0; i < 25; i++)
                {
                    Console.WriteLine("Запрос 50 элементов с " + i + " страницы");
                    string format = SendRequest("https://api.hh.ru/", "vacancies?per_page=50&page=" + i).Result;
                    //Console.WriteLine(formattedResponse);

                    dynamic results = JsonConvert.DeserializeObject<dynamic>(format);
                    if (results.items != null)
                    {
                        foreach (var item in results.items)
                        {
                            if (item.salary != null)
                            {
                                Vacanc v = new Vacanc();
                                if (item.id != null) v.Id = (int)item.id;
                                if (item.name != null) v.Name = (string)item.name;

                                if (item.salary.from != null && item.salary.to != null)
                                {
                                    v.Salary = ((int)item.salary.from + (int)item.salary.to) / 2;
                                }
                                else if (item.salary.from != null)
                                {
                                    v.Salary = (int)item.salary.from;
                                }
                                else if (item.salary.to != null)
                                {
                                    v.Salary = (int)item.salary.to;
                                }

                                if (item.salary.currency != null)
                                    v.Salary = (int)(v.Salary * currencies.Find(c => c.Kod.Equals((string)item.salary.currency)).Tariff);

                                vacancies.Add(v);
                            }
                        }
                    }
                }
                Console.WriteLine();

                return vacancies;
            }

            static async Task<string> SendRequest(string uri, string requestUri)
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(uri),
                    Timeout = TimeSpan.FromSeconds(10)
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "api-test-agent");

                HttpRequestMessage requ = new HttpRequestMessage(HttpMethod.Get, requestUri);

                HttpResponseMessage response = await client.SendAsync(requ);
                response.EnsureSuccessStatusCode();

                string format = "";

                if (response.IsSuccessStatusCode)
                {
                    format = response.Content.ReadAsStringAsync().Result;
                }
                return format;
            }
      }

   
 }


