using System;
using System.Diagnostics; // To jest wbudowane, zadziała od razu

namespace BibliotekaAPI.Models
{
    public class VulnerableService
    {
        // BŁĄD 2: Security Hotspot (Hardcoded Credentials)
        // SonarQube wykryje to jako ryzyko wycieku klucza
        private const string ApiKey = "12345-ABCDE-SECRET-KEY";

        // BŁĄD 1: Vulnerability (OS Command Injection)
        // Zamiast bazy danych, symulujemy uruchomienie komendy systemowej
        public void RunSystemCommand(string userInput)
        {
            // SonarQube wykryje: "Make sure this argument is properly validated" (Injection)
            // Uruchamianie procesu z danymi od użytkownika to krytyczna dziura
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + userInput; // Sklejanie komendy = atak

            Process.Start(startInfo);
        }

        // BŁĄD 3: Code Smell (Cognitive Complexity + Spaghetti Code)
        // To zostawiamy bez zmian, bo ładnie generuje Code Smell
        public void ComplexLogic(int x, int y, bool check)
        {
            if (x > 0)
            {
                if (y < 100)
                {
                    for (int i = 0; i < x; i++)
                    {
                        if (check)
                        {
                            if (i % 2 == 0)
                            {
                                Console.WriteLine("Even");
                            }
                            else
                            {
                                // Pusty blok else
                            }
                        }
                    }
                }
            }
        }
    }
}