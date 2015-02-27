using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VehicleDB
{
    class Program
    {
        static string Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private delegate void TravelVehicle(string destination);
        private static void DummyTravelVehicle(string destination){ }

        static string genChar(char c, int times = 10)
        {
            string ret = "";
            for (int i = 0; i < times; i++)
            {
                ret += c;
            }
            return ret;
        }

        static void printFrame(string[] txt, bool center = true, short padding = 3)
        {
            if (padding < 1) padding = 1;
            int fl = txt.OrderByDescending(s => s.Length).First().Length + (center ? 2 : 1) * padding + (center ? 0 : 3);
            if (center && (fl % 2 != 0)) fl++;
            Console.Write(genChar('-', fl));
            foreach (string l in txt)
            {
                int bl = (fl - l.Length - 2);
                if(center) Console.Write("\n|" + genChar(' ', (int)Math.Floor((double)bl / 2)) + l + genChar(' ', (int)Math.Ceiling((double)bl / 2)) + "|\n");
                else Console.Write("\n|" + genChar(' ', padding) + l + genChar(' ', bl - padding) + "|\n");
            }
            Console.WriteLine(genChar('-', fl));
        }

        static bool ReMatch(string input, string pattern)
        { 
            return Regex.IsMatch(input, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Title = "VehicleDB " + Program.Version;
                VehiclesDB vDB = new VehiclesDB("vdb.dat");
                printFrame(new string[] { "Car - Motorcycle - Aeroplane", "VehicleDB " + Program.Version, "Christoph Witzko 2013"});
                if (!vDB.LoadDB()) throw new Exception("Could not load database!");
                Console.WriteLine("{0} database entries loaded.", vDB.Count);
                bool running = true;
                string cmd = "";
                while (running)
                {
                    try
                    {
                        Console.Write("VehicleDB> ");
                        cmd = Console.ReadLine().Trim();
                        if (ReMatch(cmd, "^exit$") || ReMatch(cmd, "^quit$") || ReMatch(cmd, "^e$") || ReMatch(cmd, "^q$"))
                        {
                            vDB.SaveDB();
                            running = false;
                        }
                        else if (ReMatch(cmd, "^save$") || ReMatch(cmd, "^s$"))
                        {
                            if (vDB.SaveDB()) Console.WriteLine("Database saved.");
                            else throw new Exception("Could not save database!");
                        }
                        else if (ReMatch(cmd, "^reload$") || ReMatch(cmd, "^r$") || ReMatch(cmd, "^load$"))
                        {
                            if (vDB.LoadDB()) Console.WriteLine("Database loaded.\n{0} database entries loaded.", vDB.Count);
                            else throw new Exception("Could not load database!");
                        }
                        else if (ReMatch(cmd, "^count"))
                        {
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 1)
                            {
                                Console.WriteLine("The database has {0} entries.", vDB.FindVType(pr[1]).Count());
                            }
                            else
                            {
                                Console.WriteLine("The database has {0} entries.", vDB.Count);
                            }
                            
                        }
                        else if (ReMatch(cmd, "^ls") || ReMatch(cmd, "^list") || ReMatch(cmd, "^show") || ReMatch(cmd, "^l"))
                        {
                            if (vDB.Count < 1)
                            {
                                Console.WriteLine("Database is empty!");
                                continue;
                            }
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 1)
                            {
                                foreach (VehicleBase v in vDB.FindVType(pr[1]))
                                {
                                    printFrame(v.GetDescription().Split('\n'), false, 2);
                                }
                            }
                            else
                            { 
                                foreach (VehicleBase v in vDB.Vehicles) 
                                { 
                                    printFrame(v.GetDescription().Split('\n'), false, 2);
                                }
                            }
                        }
                        else if (ReMatch(cmd, "^create") || ReMatch(cmd, "^new ") || ReMatch(cmd, "^c "))
                        {
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 1)
                            {
                                VehicleBase nv;
                                if (ReMatch(pr[1], "^c"))
                                {
                                    nv = new Vehicle.Car();
                                }
                                else if (ReMatch(pr[1], "^a"))
                                {
                                    nv = new Vehicle.Aeroplane();
                                }
                                else if (ReMatch(pr[1], "^m"))
                                {
                                    nv = new Vehicle.Motorcycle();
                                }
                                else 
                                {
                                    throw new Exception("Vehicle-type not defined!");
                                }
                                Console.WriteLine("Create an new {0}:", nv.GetType().Name.ToLower());
                                if (nv.GetUserInput()) 
                                {
                                    vDB.Add(nv);
                                    Console.WriteLine("A {0} has been created.", nv.GetType().Name.ToLower());
                                }
                               
                            }
                            else
                            {
                                Console.WriteLine("usage: create <car, aeroplane, motorcycle>");
                            }
                        }
                        else if (ReMatch(cmd, "^delete"))
                        {
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 1)
                            {
                                VehicleBase fv = vDB.FindFirstID(pr[1]);
                                if (fv == null) throw new Exception("No vehicle found!");
                                vDB.Remove(fv);
                                Console.WriteLine("Vehicle has been removed.");
                            }
                            else
                            {
                                Console.WriteLine("usage: delete <ID>");
                            }
                        }
                        else if (ReMatch(cmd, "^fly") || ReMatch(cmd, "^f "))
                        {
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 2)
                            {
                                string dest = String.Join(" ", pr, 2, pr.Length - 2);
                                VehicleBase fv = vDB.FindFirstID(pr[1]);
                                if (fv == null) throw new Exception("No vehicle found!");
                                if (!(fv is IFlyable)) throw new Exception("Vehicle is not flyable!");
                                ((IFlyable)fv).FlyTo(dest);
                            }
                            else
                            {
                                Console.WriteLine("usage: fly <ID> <destination>");
                            }
                        }
                        else if (ReMatch(cmd, "^drive") || ReMatch(cmd, "^d "))
                        {
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 2)
                            {
                                string dest = String.Join(" ", pr, 2, pr.Length - 2);
                                VehicleBase fv = vDB.FindFirstID(pr[1]);
                                if (fv == null) throw new Exception("No vehicle found!");
                                if (!(fv is IRidable)) throw new Exception("Vehicle is not ridable!");
                                ((IRidable)fv).DriveTo(dest);
                            }
                            else
                            {
                                Console.WriteLine("usage: drive <ID> <destination>");
                            }
                        }
                        else if (ReMatch(cmd, "^travel") || ReMatch(cmd, "^t "))
                        {
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 2)
                            {
                                string dest = String.Join(" ", pr, 2, pr.Length - 2);
                                VehicleBase fv = vDB.FindFirstID(pr[1]);
                                if (fv == null) throw new Exception("No vehicle found!");
                                TravelVehicle trvl = new TravelVehicle(DummyTravelVehicle);
                                if (fv is IRidable) trvl += new TravelVehicle(((IRidable)fv).DriveTo);
                                if (fv is IFlyable) trvl += new TravelVehicle(((IFlyable)fv).FlyTo);
                                if (trvl.GetInvocationList().GetLength(0) == 1) throw new Exception("No travel methode found for this vehicle!");
                                trvl(dest);
                            }
                            else
                            {
                                Console.WriteLine("usage: drive <ID> <destination>");
                            }
                        }
                        else if (ReMatch(cmd, "^find"))
                        {
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 1)
                            {
                                IEnumerable<VehicleBase> fv = vDB.FindIDs(pr[1]);
                                if (fv.Count() < 1) throw new Exception("No vehicles found!");
                                foreach (VehicleBase v in fv)
                                {
                                    Console.WriteLine("{0}: {1}", v._ID, v.GetType().Name);
                                }
                            }
                            else
                            {
                                Console.WriteLine("usage: find <ID>");
                            }
                        }
                        else if (ReMatch(cmd, "^search"))
                        {
                            string[] pr = cmd.Split(' ');
                            if (pr.Length > 1)
                            {
                                string sv = String.Join(" ", pr, 1, pr.Length - 1);
                                IEnumerable<VehicleBase> fv = vDB.Search(sv);
                                if (fv.Count() < 1) throw new Exception("No vehicles found!");
                                foreach (VehicleBase v in fv)
                                {
                                    printFrame(v.GetDescription().Split('\n'), false, 2);
                                }
                            }
                            else
                            {
                                Console.WriteLine("usage: search <keyword>");
                            }
                        }
                        else if (ReMatch(cmd, "^help$") || ReMatch(cmd, "^h$"))
                        {
                            printFrame(new string[] { "Help for VehicleDB (Version " + Program.Version + ")", "", "exit[e, quit, q]: closes the application", "save[s]: saves the database", "reload[r, load]: reloads the database", "count: prints the amout of vehicles", "list[l, ls, show] (car, aeroplane, motorcycle): shows the vehicles", "create[c, new] <car, aeroplane, motorcycle>: creates a new vehicle", "delete <ID>: deletes a vehicle", "drive[d] <ID> <destination>: drive a ridable vehicle", "fly[f] <ID> <destination>: fly a flyable vehicle", "travel[t] <ID> <destination>: fly or ride a vehicle", "find <ID>: finds vehicles by a id", "search <keyword>: finds vehicles by a keyword" }, false, 1);
                        }
                        else
                        {
                            throw new Exception("Command not found!");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[ERROR]: " + e.Message);
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]: " + e.Message);
            }
        }
    }
}
