using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine(@"
   __          __           _ _      _     _    _____            
   \ \        / /          | | |    (_)   | |  / ____|           
    \ \  /\  / /__  _ __ __| | |     _ ___| |_| |  __  ___ _ __  
     \ \/  \/ / _ \| '__/ _` | |    | / __| __| | |_ |/ _ \ '_ \ 
      \  /\  / (_) | | | (_| | |____| \__ \ |_| |__| |  __/ | | |
       \/  \/ \___/|_|  \__,_|______|_|___/\__|\_____|\___|_| |_|
       github.com/Igthz
        ");
        Console.WriteLine("Digite as palavras para gerar a wordlist (separadas por vírgulas):");
        string input = Console.ReadLine();

        List<string> words = new List<string>();

        if (!string.IsNullOrWhiteSpace(input))
        {
            string[] wordArray = input.Split(',');

            foreach (string word in wordArray)
            {
                string trimmedWord = word.Trim();
                if (!string.IsNullOrWhiteSpace(trimmedWord))
                {
                    words.Add(trimmedWord);
                }
            }
        }
        else
        {
            Console.WriteLine("Nenhuma palavra foi inserida. O programa será encerrado.");
            Console.WriteLine("Pressione qualquer tecla para sair.");
            Console.ReadKey();
            return;
        }

        string fileName = GetFileName();

        try
        {
            GenerateWordlist(words, fileName);

            Console.WriteLine("Wordlist gerada com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao gerar a wordlist: {ex.Message}");
        }

        Console.WriteLine("Pressione qualquer tecla para sair.");
        Console.ReadKey();
    }

    static string GetFileName()
    {
        Console.WriteLine("Digite o caminho completo e o nome do arquivo de saída:");
        string fileName = Console.ReadLine();

        while (true)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine("O caminho e o nome do arquivo não podem estar vazios. Tente novamente.");
            }
            else if (!Path.HasExtension(fileName) || Path.GetExtension(fileName) != ".txt")
            {
                Console.WriteLine("O arquivo de saída deve ter uma extensão .txt. Tente novamente.");
            }
            else
            {
                string directory = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(directory))
                {
                    Console.WriteLine("O diretório especificado não existe. Deseja criar o diretório? (S/N)");
                    string createDirInput = Console.ReadLine();

                    if (createDirInput.Equals("S", StringComparison.OrdinalIgnoreCase))
                    {
                        Directory.CreateDirectory(directory);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("O programa será encerrado.");
                        Console.WriteLine("Pressione qualquer tecla para sair.");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }

                break;
            }

            Console.WriteLine("Digite o caminho completo e o nome do arquivo de saída:");
            fileName = Console.ReadLine();
        }

        return fileName;
    }

    static void GenerateWordlist(List<string> words, string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            GenerateCombinations(words, writer);
        }
    }

    static void GenerateCombinations(List<string> words, StreamWriter writer)
    {
        int totalWords = words.Count;

        long totalCombinations = (long)Math.Pow(totalWords, totalWords);

        for (long i = 0; i < totalCombinations; i++)
        {
            long index = i;
            List<string> combination = new List<string>();

            for (int j = 0; j < totalWords; j++)
            {
                int wordIndex = (int)(index % totalWords);
                combination.Add(words[wordIndex]);

                index /= totalWords;
            }

            combination.Reverse();
            string wordlistEntry = string.Join("", combination);
            writer.WriteLine(wordlistEntry);
        }
    }
}
