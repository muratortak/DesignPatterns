using System;
using System.Collections.Generic;
/**
* Why use Factories:
*  Object creation logic becomes too convoluted
*  Constructor is not descriptive
*      Name mandated by name of containing type
*      Cannot overload with same sets of arguments with different names
*      Can turn into 'optional parameter hell' 
*  Object creation (non-piecewise, unlike Builder) can be outsourced to
*      A separate function (Factory Method)
*      That may exist in a separate class (Factory)
*      Can create hierarchy of factories with Abstract Factory
* 
* Factory: 
*  A component responsible solely for the wholesale (not piecewise) creation of objects.
* **/
namespace Factories
{
    #region ConcreteFactory
    //public enum CoordinateSystem
    //{
    //    Cartesian,
    //    Polar
    //}
    public class Point
    {
        private double x,y;
        /// <summary>
        /// Initializes a point from EITHER Cartesian or Polar.
        /// PROBLEM HERE:
        ///     There is too much going on with the Ctor
        ///     Constructor signature is ambigous in terms of which paramter is assigned to a or b
        /// </summary>
        /// <param name="a">x if Cartesian, rho if polar</param>
        /// <param name="b"></param>
        /// <param name="system"></param>
        //public Point(double a, double b, 
        //    CoordinateSystem system = CoordinateSystem.Cartesian)
        //{
        //    switch (system)
        //    {
        //        case CoordinateSystem.Cartesian:
        //            x = a;
        //            y = b;
        //            break;
        //        case CoordinateSystem.Polar:
        //            x = a * Math.Cos(b);
        //            y = b * Math.Sin(b);
        //            break;
        //    }
        //}

        // factory method
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
        /// <summary>
        /// Advantage of Factory Method:
        ///     You get to have an overload with same set of arguments. But they have DIFFERENT, descriptive name so API tells you
        ///         what arguments you are providing.
        ///     Names of the Factory Methods are also UNIQUE.
        ///    
        /// OVERALL THIS IS AN API IMPROVEMENT.
        ///     
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }

        // Arrow makes it a property. Everytime you try to access the Point Origin, you will be getting a new coor with 0,0.
        //and you would access it in the main var origin = Point.Origin; If you want to instantiate a new object any time anybody ask for something
        //then a property is a good method.
        public static Point Origin => new Point(0, 0);

        // This is just a field. You initialize a static field once. It is avaliable everywhere  
        public static Point Origin2 = new Point(0, 0); // better

        //public static PointFactory Factory => new PointFactory();
        // If you make PointFactory make the class and its method Static. Better approach!
        //public class PointFactory
        public static class Factory
        {
            //public Point NewCartesianPoint(double x, double y)
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            //public Point NewPolarPoint(double rho, double theta)
            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }

    }

    // FOR SMALL, SIMPLE OBJECTS FACTORY METHOD WITH CTOR IS OKAY. BUT IF YOU NEEDED A FACTORY, IT IS BETTER TO HAVE FACTORY CLASS.

    //public class PointFactory
    //{
    //    public static Point NewCartesianPoint(double x, double y)
    //    {
    //        return new Point(x, y);
    //    }

    //    public static Point NewPolarPoint(double rho, double theta)
    //    {
    //        return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
    //    }
    //}

    #endregion

    #region AbstractFactory
    // Only real use of Abstract Factory is to give out Abstract Object as opposed to Concrete Object.
    // In Abstract you are not returning the type you are creating. You are returning Abstract classes or Interfaces.
    public interface IHotDrink
    {
        void Consume();
    }

    // Internal is only used in the class so they won't be given out to client but no factory methods won't return tea or cofee but will return IHotDrink.
    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This tea is nice.");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This coffee is nice.");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Put in a tea bag, boil water, pout {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Grind some beans, boil water, pout {amount} ml, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        //Enum violates Open-Close principle. You have to add a new type of drink. That means changing the code.
        //public enum AvailableDrink
        //{
        //    Coffee, Tea
        //}

        //private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();

        //public HotDrinkMachine()
        //{
        //    foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
        //    {
        //        var factory = (IHotDrinkFactory)Activator.CreateInstance(
        //            Type.GetType("Factories." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory")
        //            );
        //        factories.Add(drink, factory);
        //    }
        //}

        //public IHotDrink MakeDrink( AvailableDrink drink, int amount)
        //{
        //    return factories[drink].Prepare(amount);
        //}

        private List<Tuple<string, IHotDrinkFactory>> factories = new List<Tuple<string, IHotDrinkFactory>>();
        public HotDrinkMachine()
        {
            foreach(var t in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if(typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    factories.Add(Tuple.Create(
                        t.Name.Replace("Factory", string.Empty),
                        (IHotDrinkFactory)Activator.CreateInstance(t)
                        ));
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            Console.WriteLine("Available Drinks: ");
            for(var index = 0; index < factories.Count; index++)
            {
                var tuple = factories[index];
                Console.WriteLine($"{index}: {tuple.Item1}");
            }

            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null && int.TryParse(s, out int i) && i >= 0 && i < factories.Count)
                {
                    Console.WriteLine("Specify amount: ");
                    s = Console.ReadLine();
                    if(s != null && int.TryParse(s, out int amount) && amount > 0)
                    {
                        return factories[i].Item2.Prepare(amount);
                    }
                }
                Console.WriteLine("Incorrect input, try again!");
            }
        }

    }

    #endregion
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World from Factories Project!");
            #region ConcreteFactoryClient
            //var point = Point.NewPolarPoint(1.0, Math.PI / 2);
            //var point = Point.Factory.NewPolarPoint(1.0, Math.PI / 2);
            //var point = Point.Factory.NewPolarPoint(1.0, Math.PI / 2);
            //Console.WriteLine(point);
            #endregion

            #region AbstractFactoryClient
            var machine = new HotDrinkMachine();
            //var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, 100);
            //drink.Consume();
            var drink = machine.MakeDrink();
            drink.Consume();
            #endregion
        }
    }
}
