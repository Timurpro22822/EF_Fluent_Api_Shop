using System;

namespace Course_2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new StoreContext())
            {
                // Ініціалізація бази даних
                DbInitializer.Initialize(context);
            }
        }
    }
}