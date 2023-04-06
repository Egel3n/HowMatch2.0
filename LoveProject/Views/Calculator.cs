using System.Xml.Linq;

namespace LoveProject.Views
{
    public static class Calculator
    {

        
        public static int Calculate(string name1,string name2) {

            int points1 = PointCalculator(name1);
            int points2 = PointCalculator(name2);
            return Math.Abs((points1-points2));
        }


        public static int PointCalculator(string name)
        {
            int point = 0;

            foreach (var i in name)
            {

                switch (i.ToString().ToLower())
                {

                    case "a":
                        point +=1 ;
                            break;
                    case "b":
                        point += 2;
                        break;
                    case "c":
                        point +=3 ;
                        break;
                    case "ç":
                        point +=4 ;
                        break;
                    case "d":
                        point +=5 ;
                        break;
                    case "e":
                        point +=6 ;
                        break;
                    case "f":
                        point += 7;
                        break;
                    case "g":
                        point += 8;
                        break;
                    case "ğ":
                        point += 9;
                        break;
                    case "h":
                        point += 10;
                        break;
                    case "ı":
                        point += 11;
                        break;
                    case "i":
                        point += 12;
                        break;
                    case "j":
                        point += 13;
                        break;
                    case "k":
                        point += 14;
                        break;
                    case "l":
                        point += 15;
                        break;
                    case "m":
                        point += 14;
                        break;
                    case "n":
                        point += 13;
                        break;
                    case "o":
                        point += 12;
                        break;
                    case "ö":
                        point += 11;
                        break;
                    case "p":
                        point += 10;
                        break;
                    case "r":
                        point += 9;
                        break;
                    case "s":
                        point += 8;
                        break;
                    case "ş":
                        point += 7;
                        break;
                    case "t":
                        point += 6;
                        break;
                    case "u":
                        point += 5;
                        break;
                    case "ü":
                        point += 4;
                        break;
                    case "v":
                        point += 3;
                        break;
                    case "y":
                        point += 2;
                        break;
                    case "z":
                        point += 1;
                        break;
                    


                }

               
            }

            return point;

        }

        
       









    }
}
