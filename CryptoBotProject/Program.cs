﻿namespace CryptoBotProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            WorkProcessor workProcessor = WorkProcessor.Instance;
            await workProcessor.Start();
        }
    }
}