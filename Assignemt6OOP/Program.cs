using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment06OOP
{
    #region First Project: 3D Point Class
    class Point3D : ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point3D() : this(0, 0, 0) { }

        public Point3D(int x, int y) : this(x, y, 0) { }

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"Point Coordinates: ({X}, {Y}, {Z})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Point3D other)
            {
                return X == other.X && Y == other.Y && Z == other.Z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator ==(Point3D p1, Point3D p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Point3D p1, Point3D p2)
        {
            return !p1.Equals(p2);
        }

        public object Clone()
        {
            return new Point3D(X, Y, Z);
        }
    }
    #endregion

    #region Second Project: Maths Class with Static Methods
    static class Maths
    {
        public static int Add(int a, int b) => a + b;

        public static int Subtract(int a, int b) => a - b;

        public static int Multiply(int a, int b) => a * b;

        public static double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }
    }
    #endregion

    #region Third Project: E-Commerce Discounts
    abstract class Discount
    {
        public string Name { get; set; }

        public abstract decimal CalculateDiscount(decimal price, int quantity);
    }

    class PercentageDiscount : Discount
    {
        private readonly decimal percentage;

        public PercentageDiscount(decimal percentage)
        {
            this.percentage = percentage;
            Name = $"{percentage}% Discount";
        }

        public override decimal CalculateDiscount(decimal price, int quantity)
        {
            return price * quantity * (percentage / 100);
        }
    }

    class FlatDiscount : Discount
    {
        private readonly decimal flatAmount;

        public FlatDiscount(decimal flatAmount)
        {
            this.flatAmount = flatAmount;
            Name = $"Flat Discount: ${flatAmount}";
        }

        public override decimal CalculateDiscount(decimal price, int quantity)
        {
            return flatAmount * Math.Min(quantity, 1);
        }
    }

    class BuyOneGetOneDiscount : Discount
    {
        public BuyOneGetOneDiscount()
        {
            Name = "Buy One Get One Discount";
        }

        public override decimal CalculateDiscount(decimal price, int quantity)
        {
            return (price / 2) * (quantity / 2);
        }
    }

    abstract class User
    {
        public string Name { get; set; }

        public abstract Discount GetDiscount();
    }

    class RegularUser : User
    {
        public override Discount GetDiscount()
        {
            return new PercentageDiscount(5);
        }
    }

    class PremiumUser : User
    {
        public override Discount GetDiscount()
        {
            return new FlatDiscount(100);
        }
    }

    class GuestUser : User
    {
        public override Discount GetDiscount()
        {
            return null;
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            #region First Project: Test Point3D
            Console.WriteLine("First Project: 3D Point\n");

            try
            {
                Console.Write("Enter coordinates for P1 (x y z): ");
                string[] p1Input = Console.ReadLine().Split(' ');
                Point3D P1 = new Point3D(int.Parse(p1Input[0]), int.Parse(p1Input[1]), int.Parse(p1Input[2]));

                Console.Write("Enter coordinates for P2 (x y z): ");
                string[] p2Input = Console.ReadLine().Split(' ');
                Point3D P2 = new Point3D(int.Parse(p2Input[0]), int.Parse(p2Input[1]), int.Parse(p2Input[2]));

                Console.WriteLine($"P1: {P1}");
                Console.WriteLine($"P2: {P2}");
                Console.WriteLine(P1 == P2 ? "P1 is equal to P2" : "P1 is not equal to P2");

                Point3D[] points = { P1, P2, new Point3D(1, 2, 3), new Point3D(3, 2, 1) };
                Array.Sort(points, (a, b) => a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));

                Console.WriteLine("Sorted Points:");
                foreach (var point in points)
                {
                    Console.WriteLine(point);
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter numeric coordinates.");
            }
            #endregion

            #region Second Project: Test Maths Class
            Console.WriteLine("\nSecond Project: Maths Operations\n");
            Console.WriteLine("Add: " + Maths.Add(10, 5));
            Console.WriteLine("Subtract: " + Maths.Subtract(10, 5));
            Console.WriteLine("Multiply: " + Maths.Multiply(10, 5));
            Console.WriteLine("Divide: " + Maths.Divide(10, 5));
            #endregion

            #region Third Project: Test E-Commerce Discounts
            Console.WriteLine("\nThird Project: Discounts\n");

            Console.Write("Enter user type (Regular, Premium, Guest): ");
            string userType = Console.ReadLine();
            User user = userType switch
            {
                "Regular" => new RegularUser { Name = "Regular User" },
                "Premium" => new PremiumUser { Name = "Premium User" },
                _ => new GuestUser { Name = "Guest User" },
            };

            Console.Write("Enter product price: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter product quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            Discount discount = user.GetDiscount();
            decimal discountAmount = discount?.CalculateDiscount(price, quantity) ?? 0;
            decimal finalPrice = (price * quantity) - discountAmount;

            Console.WriteLine($"Discount Applied: {discount?.Name ?? "No Discount"}");
            Console.WriteLine($"Total Discount: {discountAmount:C}");
            Console.WriteLine($"Final Price: {finalPrice:C}");
            #endregion
        }
    }
}
