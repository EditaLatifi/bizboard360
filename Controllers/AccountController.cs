using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finlab.Data;
using Finlab.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Finlab.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Account/Login
        public async Task<IActionResult> Login()
        {
            // Create default admin user if no users exist
            await CreateDefaultUserIfNeeded();
            
            // Redirect if user is already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Finlab");
            }
            return View();
        }

        private async Task CreateDefaultUserIfNeeded()
        {
            try
            {
                if (!await _context.Users.AnyAsync())
                {
                    var defaultUser = new User
                    {
                        Username = "admin",
                        Email = "info@bizboard.eu",
                        PasswordHash = HashPassword("Admin5"),
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.Users.Add(defaultUser);
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("Default admin user created: {Email}", defaultUser.Email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating default user");
            }
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => 
                            (u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail) && 
                            u.IsActive);

                    if (user != null && VerifyPassword(model.Password, user.PasswordHash))
                    {
                        // Log successful login
                        _logger.LogInformation("User {Username} logged in successfully", user.Username);
                        
                        // TODO: Implement proper authentication (sessions, JWT tokens, etc.)
                        // For now, we'll just redirect to the dashboard
                        return RedirectToAction("Index", "Finlab");
                    }

                    // Log failed login attempt
                    _logger.LogWarning("Failed login attempt for username/email: {UsernameOrEmail}", model.UsernameOrEmail);
                    
                    // Add generic error message for security
                    ModelState.AddModelError("", "Invalid username/email or password");
                    
                    // Set error message in TempData for display
                    TempData["ErrorMessage"] = "Invalid username/email or password. Please try again.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during login process");
                    ModelState.AddModelError("", "An error occurred during login. Please try again.");
                    TempData["ErrorMessage"] = "An error occurred during login. Please try again.";
                }
            }

            return View(model);
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            // Redirect if user is already logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Finlab");
            }
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if username or email already exists
                    if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                    {
                        ModelState.AddModelError("Username", "Username already exists");
                        return View(model);
                    }

                    if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                    {
                        ModelState.AddModelError("Email", "Email already exists");
                        return View(model);
                    }

                    // Create new user
                    var user = new User
                    {
                        Username = model.Username,
                        Email = model.Email,
                        PasswordHash = HashPassword(model.Password),
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // Log successful registration
                    _logger.LogInformation("New user registered: {Username}", user.Username);

                    // Redirect to login page
                    TempData["SuccessMessage"] = "Registration successful! Please log in with your new account.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during user registration");
                    ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                    TempData["ErrorMessage"] = "An error occurred during registration. Please try again.";
                }
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            try
            {
                // Log logout
                if (User.Identity?.IsAuthenticated == true)
                {
                    _logger.LogInformation("User logged out");
                }
                
                // TODO: Implement proper logout (clear sessions, etc.)
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return RedirectToAction("Login");
            }
        }

        // GET: /Account/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Please enter your email address.";
                return View();
            }

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
                
                if (user != null)
                {
                    // TODO: Implement password reset functionality
                    // Send reset email, generate reset token, etc.
                    _logger.LogInformation("Password reset requested for user: {Email}", email);
                    
                    TempData["SuccessMessage"] = "If an account with that email exists, we've sent a password reset link.";
                }
                else
                {
                    // Don't reveal if email exists or not for security
                    TempData["SuccessMessage"] = "If an account with that email exists, we've sent a password reset link.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during forgot password process");
                TempData["ErrorMessage"] = "An error occurred. Please try again.";
            }

            return View();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }
}

