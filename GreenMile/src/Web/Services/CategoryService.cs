using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class CategoryService
    {

        private readonly DataContext _dataContext;
        private readonly List<Category> _defaultCategories= new List<Category>()
        {
            new Category()
            {
                Name = "Fruits",
                Description = "This is fruits"

                
            },
             new Category()
            {
                Name = "Vegetables",
                Description = "This is vegetables"


            },
              new Category()
            {
                Name = "Meat",
                Description = "This is meat"


            },
               new Category()
            {
                Name = "Dairy",
                Description = "This is dairy"



            },
                   new Category()
            {
                Name = "Others",
                Description = "This is others"



            }
        };
        public CategoryService(DataContext dataContext) {
        _dataContext= dataContext;
        }

        public async Task Prepopulate()
        {
            var list = _dataContext.Categories.ToList().Select(x => x.Name);
     
            foreach (var category in _defaultCategories)
            {
                if(!list.Contains(category.Name)) {
                    await _dataContext.Categories.AddAsync(category);
                    await _dataContext.SaveChangesAsync();
                }
            }
        
        }



        public async Task<List<Category>> RetrieveCategories()
        {
            return _dataContext.Categories.ToList();
        }

        public async Task<Category> RetrieveCategory(int id)
        {
            return await _dataContext.Categories.FindAsync(id); 
        }
    }
}
