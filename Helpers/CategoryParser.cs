using System;
using CascadingDropdownsDemo.Models;

namespace CascadingDropdownsDemo.Helpers
{
	public static class CategoryParser
	{
		public static List<Category> BuildCategoryHierarchy(IList<Category> categories, bool useAIName = false)
		{
			var categoryTree = new List<Category>();

            // For quick lookups, more performant
            var categoryLookup = new Dictionary<string, Category>();

			foreach (var category in categories)
			{
				// Split string based on delimiter
				string[] parts = useAIName ? category.AICatFullName.Split('-') : category.CatFullName.Split(" > ");

				Category parent = null;

				string fullPath = "";

                // We only need catId assigned at the last (4th level) category
                for (int i = 0; i < parts.Length; i++)
                {
                    string part = parts[i];
                    fullPath = string.IsNullOrEmpty(fullPath) ? part : $"{fullPath} > {part}";

                    if (!categoryLookup.ContainsKey(fullPath))
                    {
                        var newCategory = new Category
                        {
                            Name = part,
                            catID = (i == parts.Length - 1) ? category.catID : 0,
                            Level2Categories = new List<Category>()
                        };

                        categoryLookup[fullPath] = newCategory;

                        if (parent == null)
                        {
                            // Top level category
                            categoryTree.Add(newCategory);
                        }
                        else
                        {
                            // Add as a subcategory
                            parent.Level2Categories.Add(newCategory);
                        }
                    }
                    parent = categoryLookup[fullPath];
                }

            }
			return categoryTree;
		}
	}
}

