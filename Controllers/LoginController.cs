using Microsoft.AspNetCore.Mvc;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.Service;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers;

public class LoginController : Controller
{

    private readonly MvcMovieContext _context;

    public LoginController(MvcMovieContext context)
    {
        _context = context;
    }
    // GET: /HelloWorld/
    public IActionResult Index()
    {
        return View();
    }
    // 
    // GET: /HelloWorld/Welcome/{name=Scott}&{numTimes=1}


    [HttpPost]
    public IActionResult LoginIn([Bind("Email,Password")] User user)
    {
        var Users = _context.User.FirstOrDefault(u => u.Email == user.Email);
        if (Users == null)
        {
            return RedirectToAction("Index", "Login");
        }

        if (user.Email == Users.Email && Users.Password == HashPassword(user.Password))
        {
            var token = TokenService.GenerateToken(Users);
            return RedirectToAction("/Home", new { token = token });
        }
        else
        {
            return BadRequest();
        }
    }
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Converte a senha em bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Calcula o hash SHA256
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

            // Converte o hash em uma string hexadecimal
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hashedBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
