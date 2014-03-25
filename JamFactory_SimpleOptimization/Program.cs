using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamFactory_SimpleOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAlgorithm tester = new TestAlgorithm();

            tester.RunTest();

            Console.ReadLine();
        }
    }

    class TestAlgorithm
    {
        List<RawMaterial> rawMaterials;
        List<Delivery> deliveries;
        List<Recipe> recipes;

        public TestAlgorithm()
        {
            rawMaterials = new List<RawMaterial>();
            deliveries = new List<Delivery>();
            recipes = new List<Recipe>();

        }

        public void RunTest()
        {
            rawMaterials.Add(new RawMaterial("Hyben"));
            rawMaterials.Add(new RawMaterial("Æble"));
            rawMaterials.Add(new RawMaterial("Solbær"));
            rawMaterials.Add(new RawMaterial("Sukker"));

            deliveries.Add(new Delivery { RawMaterial = rawMaterials[0], Amount = 100, Price = 10 });
            deliveries.Add(new Delivery { RawMaterial = rawMaterials[1], Amount = 100, Price = 5 });
            deliveries.Add(new Delivery { RawMaterial = rawMaterials[2], Amount = 100, Price = 8 });
            deliveries.Add(new Delivery { RawMaterial = rawMaterials[3], Amount = 1000, Price = 7 });

            Recipe recipe1 = new Recipe("Solbær marmelade");
            recipe1.Ingredients.Add(new Ingredient {RawMaterial = rawMaterials[2], Amount = 0.45});
            recipe1.Ingredients.Add(new Ingredient {RawMaterial = rawMaterials[3], Amount = 0.45});
            recipes.Add(recipe1);

            Recipe recipe2 = new Recipe("Hyben/Æble marmelade");
            recipe2.Ingredients.Add(new Ingredient {RawMaterial = rawMaterials[0], Amount = 0.225});
            recipe2.Ingredients.Add(new Ingredient {RawMaterial = rawMaterials[1], Amount = 0.225});
            recipe2.Ingredients.Add(new Ingredient {RawMaterial = rawMaterials[3], Amount = 0.45});
            recipes.Add(recipe2);

            List<Recipe> cheapestRecipes = orderRecipesByPrice();

            foreach (Recipe recipe in cheapestRecipes)
            {
                Console.WriteLine(recipe.Name);
            }

            // calculate the amount to produce of each recipe
            // to handle multiple deliveries for one raw material: need productions, 
            // of which there can be many candidate productions per recipe
        }

        private List<Recipe> orderRecipesByPrice()
        {
            List<Recipe> cheapestRecipes = new List<Recipe>();
            Dictionary<Recipe, decimal> recipePrices = new Dictionary<Recipe,decimal>();

            foreach (Recipe recipe in recipes)
            {
                decimal price = calculateRecipePrice(recipe);
                recipePrices[recipe] = price;
            }

            foreach (var recipePrice in recipePrices.OrderBy(i => i.Value))
            {
                cheapestRecipes.Add(recipePrice.Key);
            }

            return cheapestRecipes;
        }

        private decimal calculateRecipePrice(Recipe recipe)
        {
            decimal price = 0;

            foreach (Ingredient ingredient in recipe.Ingredients)
            {
                List<Delivery> matchingDeliveries = getDeliveriesForRawMaterial(ingredient.RawMaterial);

                // broken - what about multiple deliveries for same raw material?
                price += (decimal)ingredient.Amount * matchingDeliveries[0].Price;
            }

            Console.WriteLine("Recipe: " + recipe.Name + " Price: " + price);
            return price;
        }

        private List<Delivery> getDeliveriesForRawMaterial(RawMaterial targetRawMaterial)
        {
            List<Delivery> matchingDeliveries = new List<Delivery>();

            foreach (Delivery delivery in deliveries)
            {
                if (delivery.RawMaterial == targetRawMaterial)
                {
                    matchingDeliveries.Add(delivery);
                }
            }

            return matchingDeliveries;
        }
    }

    class Delivery
    {
        public RawMaterial RawMaterial { get; set; }
        public double Amount { get; set; }
        public decimal Price { get; set; }
    }

    class Recipe
    {
        public List<Ingredient> Ingredients { get; private set; }
        public string Name { get; set; }

        public Recipe(string name)
        {
            Ingredients = new List<Ingredient>();
            Name = name;
        }
    }

    class Ingredient
    {
        public RawMaterial RawMaterial { get; set; }
        public double Amount { get; set; }
    }

    class RawMaterial
    {
        public string Name { get; set; }

        public RawMaterial(string name)
        {
            Name = name;
        }
    }
}
