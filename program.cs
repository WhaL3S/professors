class Program
    {
        // Initial data file 
        const string CFd = "U7.txt";

        // File to output results
        const string CFr = "Results.txt";

        // Method to read initial data file
        static void Read(string fv, ref Modules modules)
        {
            using (StreamReader reader = new StreamReader(fv))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    string moduleName = parts[0];
                    string profSN = parts[1];
                    int creditCount = int.Parse(parts[2]);
                    string[] studSNGs = parts[3].Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    Module module = new Module(moduleName, profSN, creditCount);
                    foreach (string s in studSNGs)
                    {
                        string studSNG = s;
                        module.AddStudSNG(studSNG);
                    }
                    modules.AddModule(module);
                }
            }
        }

        // Method to print into the results file
        static void Print(string header, string fv, Modules modules)
        {
            string top =
             "--------------------------------------------------------------------------------\r\n"
             + "  Module                 Prof              Credit Count          Students \r\n"
             + "--------------------------------------------------------------------------------";
            using (StreamWriter writer = new StreamWriter(fv, true))
            {
                writer.WriteLine(header);
                writer.WriteLine(top);
                for (int i = 0; i < modules.Get(); i++)
                    writer.WriteLine("{0}", modules.Get(i).ToString());
                writer.WriteLine("--------------------------------------------------------------------------------\r\n");
            }
        }

        // Main part of the program where all methods are used and the whole program runs
        static void Main(string[] args)
        {
            // Checking if the results file exists, and deletes if so
            if (File.Exists(CFr)) File.Delete(CFr);

            // Creation and filling Modules container class 
            Modules modules = new Modules();
            Read(CFd, ref modules);

            // Printing the initial data
            Print("Initial data" , CFr, modules);

            // Creation new container and filling with previous container's values
            Modules sortModules = modules;

            // Sorting and removing modules that dont undergo condition
            sortModules.Sort();
            sortModules.Remove();

            // Creation and filling array of professors
            sortModules.FillArrayProfs();

            // Printing completely sorted container
            Print("Sorted and Modules with only one students are removed", CFr, sortModules);

            // Printing professor with the most number of modules (the first one)
            using (StreamWriter writer = new StreamWriter(CFr, true))
            {
                writer.WriteLine("Professor with the most number of modules: " + sortModules.ProfMostModules() + "\n");
            }

            // Variable for searching
            string professor;

            // Entering searched professor by the user
            Console.WriteLine("Enter the professor's full name");
            professor = Console.ReadLine();

            // Finding entered professor. If he exists, print his modules. If not, print relevant information 
            sortModules.Search(professor);
            using (StreamWriter writer = new StreamWriter(CFr, true))
            {
                writer.WriteLine("Searched professor: " + professor);
            }
            if (sortModules.Get() == 0)
            {
                using (StreamWriter writer = new StreamWriter(CFr, true))
                {
                    writer.WriteLine("There is no such professor");
                }
            }
            else 
            {
                Print(professor + " is responsible for this/these module(s) ", CFr, sortModules);
            }
        }
    }
