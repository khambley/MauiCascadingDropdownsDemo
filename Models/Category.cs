using System;
namespace CascadingDropdownsDemo.Models
{
	public class Category
	{
        public int catID { get; set; }
        public int catLevelNum { get; set; }
        public string AICatFullName { get; set; }
        public string CatFullName { get; set; }
        public string Name { get; set; }
        public int catParentID { get; set; }
        public List<Category> Level2Categories { get; set; } = new();
    }
}

