﻿namespace StockTracking.API.Settings
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;

        public double ExpiresInMinutes { get; set; }


    }
}