// See https://aka.ms/new-console-template for more information
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Api.Models;
var r = new Random();
Console.WriteLine("Stress Test Begin");
var tasks = Enumerable.Range(0, 100).ToList().Select(n =>
{
    var client = new HttpClient();
    var dto = new FibRequestDto()
    {
        StartIndex = 0,
        EndIndex = r.Next(1, 50),
        Cache = r.Next(0, 1) == 0,
        MaxTime = r.Next(0, 1000)
    };
    return client.PostAsJsonAsync("https://localhost:7120/IntFib/FibByIndex", dto);
}).ToList();
var complete = await Task.WhenAll(tasks);
var success = complete.Where(r => r.StatusCode == HttpStatusCode.OK).Count();
Console.WriteLine("Stress Test End");
Console.WriteLine($"Success/Total {success}/{complete.Length}");
