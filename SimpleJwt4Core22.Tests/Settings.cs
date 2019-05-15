using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJwt4Core22.Tests
{
    public class Settings
    {
        public const string JWT_SIGNING_KEY_KEY = "Jwt:SigningKey";
        public const string JWT_SIGNING_KEY_VALUE = "To be or not to be, that's the question!";
        public const string JWT_EXPEPRY_KEY = "Jwt:ExperyInMinutes";
        public const string JWT_EXPEPRY_VALUE = "1";
        public const string JWT_SITE_KEY = "Jwt:Site";
        public const string JWT_SITE_VALUE = "http://example.com";
    }
}
