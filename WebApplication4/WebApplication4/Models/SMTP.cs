using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApplication4.Models
{
    public class SMTP
    {
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static string Email { get; set; }

        public static void LoadSettings(IConfiguration configuration)
        {
            Host = configuration["SMTPSettings:Host"];
            Port = Convert.ToInt32(configuration["SMTPSettings:Port"]);
            Login = configuration["SMTPSettings:Login"];
            Password = configuration["SMTPSettings:Password"];
            Email = configuration["SMTPSettings:Email"];
        }
    }
}

