namespace MuslimAds.Controllers;
using Microsoft.AspNetCore.Mvc;
using MuslimAds;
// using Microsoft.Data.Sqlite; // Cette directive n'est plus nécessaire

[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly ApplicationDbContext _context; // Utilisation de ApplicationDbContext

    public UserController(ApplicationDbContext context) // Injection de ApplicationDbContext
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Users.ToList());
    }

    [HttpPost]
    public IActionResult Post(string mail, string password, string company)
    {
        _context.Users.Add(new User { Email = mail, Password = password, Company = company });
        _context.SaveChanges();
        return Ok("User created");
    }

    // Le reste du code commenté reste inchangé
    // [HttpPut]
    // public IActionResult Put(string mail, string password)
    // {
    //     connection.Open();
    //     var data = connection.CreateCommand();
    //     data.CommandText = "UPDATE Users SET PasswordHash = @password WHERE Email = @mail";
    //     data.Parameters.AddWithValue("@mail", mail);
    //     data.Parameters.AddWithValue("@password", password);
    //     data.ExecuteNonQuery();
    //     connection.Close();
    //     return Ok("User updated");
    // }

    // [HttpDelete]
    // public IActionResult Delete(string mail)
    // {
    //     connection.Open();
    //     var data = connection.CreateCommand();
    //     data.CommandText = "DELETE FROM Users WHERE Email = @mail";
    //     data.Parameters.AddWithValue("@mail", mail);
    //     data.ExecuteNonQuery();
    //     connection.Close();
    //     return Ok("User deleted");
    // }
}